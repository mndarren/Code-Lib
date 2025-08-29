#!/usr/bin/env python
# -*- coding: utf-8 -*-

# created by Stearns Financial Center
"""
.. currentmodule:: handlers
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

"""
import inspect
import logging
import socket
import sys
from enum import Enum
from abc import ABCMeta, abstractmethod
from pathlib import Path
from typing import List, Dict, Any
import os
import uuid
import datetime
import json

from elasticsearch import Elasticsearch
from kafka import KafkaProducer
from stearns_boomerang.activities import Activity
from stearns_boomerang.flattener import flatten
from stearns_boomerang.handler_settings import settings_validation_from_json_config, settings_validation_from_dict


class ActivityHandler:
    """
    An activity handler knows what to do when an activity arises.
    """
    __metaclass__ = ABCMeta

    @abstractmethod
    def handle(self, activity: Activity, *args, **kwargs):
        """
        Handle the activity.
        :param activity: the reported activity.
        :param extra: extra data with the activity
        :return:
        """
        raise NotImplementedError('handle method must be implemented in subclass.')

    def prepare(self):
        """

        :return:
        """
        pass

    def finalize(self):
        """

        :return:
        """
        pass


class ConsoleActivityHandler(ActivityHandler):
    """
    An ActivityHandler that simple prints the activity's message
    """

    def handle(self, activity: Activity, *args, **kwargs):
        print(activity.get_message())


class LogKittyActivityHandler(ActivityHandler):
    """
    An ActivityHandler that prints out LogKitty formatted activity calls
    """

    def handle(self, activity: Activity, *args, **kwargs):
        print('{datetime}\t{domain}:{pid}/{code}\t{severity}/{tag}: {message}'.format(
            pid=os.getpid(),
            severity=activity.severity.name[0],
            message=activity.get_message(),
            tag=activity.label,
            code=activity.code,
            datetime=datetime.datetime.now(),
            domain=activity.domain.label))


class ElasticSearchHandler(ActivityHandler):
    """
    An activity handler that produces to elastic search
    """
    PREFIX = 'ar-'
    SAVE = 'ext-'

    def __init__(
            self, brokers: str = None, elastic_index: str = None, application: str = 'data_model',
            topic: str = 'appreporting', producer: Elasticsearch = None
    ):
        setting_config_location = os.path.join(Path(sys.modules['__main__'].__file__).parent, "settings.json")

        if os.path.isfile(setting_config_location) and os.access(setting_config_location, os.R_OK):
            result = settings_validation_from_json_config(setting_config_location)
        else:  # pylint: disable=fixme
            # TODO: (Cleanup) Remove hard-coded settings
            result = settings_validation_from_dict({
                "ReportingService": {
                    "ElasticSearch": {
                        "Protocol": "http",
                        "Host": "10.7.40.81",
                        "Port": "9200"
                    },
                    "DocumentType": "appreport",
                    "Source": ''
                }
            })

        application = application.strip() if application is not None else ''
        if application == '':
            raise AttributeError('Must include application name for activity handler.')

        if brokers is not None:
            self._brokers = brokers
        elif result.reporting_service.host_url is not None:
            self._brokers = result.reporting_service.host_url
        elif self._brokers is None:
            self._brokers = ''
            raise AttributeError('Must include a broker.')

        self._elastic_index = elastic_index.strip() if elastic_index is not None else f'{self.PREFIX}{application}'

        self._topic = topic.strip() if topic is not None else '' if result.reporting_service.host_url is not None \
            else ''
        if self._topic == '':
            raise AttributeError('Must supply a elastic search topic.')

        self._producer = producer if producer is not None else Elasticsearch(hosts=self._brokers)

        self._source = Activity.source if Activity.source is not None else result.reporting_service.source

    def handle(self, activity: Activity, *args, **kwargs):
        data: Dict[str, Any] = {
            'activity': {
                'code': str(activity.code),
                'severity': ElasticSearchHandler.get_text_value(activity.severity),
                'label': ElasticSearchHandler.get_text_value(activity.label),
                'description': activity.description,
                'source': str(self._source or ''),
                'data_source': str(socket.gethostbyname(socket.gethostname()) if socket.gethostbyname(
                    socket.gethostname()) != '127.0.1.1' else 'Unknown'),
                'message': activity.get_message()
            },
            'reported': datetime.datetime.utcnow(),
            'index': self._elastic_index
        }

        if activity.domain:
            data['activity']['domain'] = activity.domain.to_dict()

        data.update(flatten(
            data={
                'args': args,
                'kwargs': kwargs
            }
        ))

        self._producer.index(index=self._elastic_index, doc_type=self._topic, body=data, request_timeout=30)

    @staticmethod
    def get_text_value(variable):
        """
        Apply capitalization over enums

        :param variable:
        :return:
        """
        if isinstance(variable, Enum):
            return " ".join(x.capitalize() for x in variable.name.split("_"))

        return variable


class LoggerActivityHandler(ActivityHandler):
    """
    An ActivityHandler for using Pythons's Logger
    """

    def handle(self, activity: Activity, *args, **kwargs):
        # Before we do anything else, get the source of the activity and the name of the logger.
        source: str = activity.source
        # If we weren't provided with a source...
        if source is None or not source.strip():
            # Inspect the stack.
            stack = inspect.stack()
            # Get the top frame.
            frm = stack[2]
            # Retrieve the module, file and line.
            mod = inspect.getmodule(frm[0]).__name__
            file = inspect.getfile(frm[0])
            line = inspect.getlineno(frm[0])
            # The source contains fairly descriptive information about the code that created the activity.
            source = "{} ({}, line {})".format(mod, file, line)
            # The logger
            logger_name = mod
        else:
            logger_name = source
        # Get the message from the activity.
        message = activity.get_message()
        # If the activity doesn't contain a message...
        if message is None:
            # ...use the source as the message.
            message = "Missing message from activity in {source}".format(source=source)
        # Get the appropriate logger.
        logger = logging.getLogger(logger_name)
        logger.log(level=activity.severity.value, msg=message)


class KafkaActivityHandler(ActivityHandler):
    """
    An activity handler that produces to a kafka topic
    """
    PREFIX = 'ar-'
    SAVE = 'ext-'
    PRODUCER_CONFIG = {
        'batch_size': 50000,
        'value_serializer': lambda m: json.dumps(m).encode(),
        'acks': 0
    }

    def __init__(
            self, brokers: List[str], elastic_index: str or None, application: str,
            topic: str = 'appreporting', extend_life: bool = False, producer: KafkaProducer = None
    ):
        application = application.strip() if application is not None else ''
        if application == '':
            raise AttributeError('Must include application name for activity handler.')
        self._brokers: List[str] = brokers if brokers is not None else []
        if not self._brokers:
            raise AttributeError('Must include a kafka broker list.')
        self._elastic_index = elastic_index.strip() if elastic_index is not None else str(uuid.uuid4())
        if not self._elastic_index.startswith(KafkaActivityHandler.PREFIX):
            self._elastic_index = \
                f'{KafkaActivityHandler.PREFIX}{KafkaActivityHandler.SAVE if extend_life else ""}' \
                    f'{application}-{self._elastic_index}-{datetime.date.today().isoformat()}'
        self._topic = topic.strip() if topic is not None else ''
        if self._topic == '':
            raise AttributeError('Must supply a kafka topic.')
        self._producer = producer if producer is not None else KafkaProducer(
            bootstrap_servers=brokers, **self.PRODUCER_CONFIG
        )

    def handle(self, activity: Activity, *args, **kwargs):
        data: Dict[str, Any] = {
            'activity': {
                'domain': activity.domain.to_dict(),
                'code': str(activity.code),
                'severity': activity.severity.name,
                'label': activity.label,
                'description': activity.description,
                'source': str(activity.source),
                'message': activity.get_message()
            },
            'reported': str(datetime.datetime.utcnow()),
            'index': self._elastic_index
        }
        data.update(flatten(
            data={
                'args': args,
                'kwargs': kwargs
            }
        ))
        self._producer.send(topic=self._topic, value=data)

    def flush_producer(self):
        """

        :return:
        """
        self._producer.flush()

    def finalize(self):
        """

        :return:
        """
        self.flush_producer()
