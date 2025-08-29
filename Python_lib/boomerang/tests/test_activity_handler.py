#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from functools import partial
from typing import Callable
from stearns_boomerang.activities import Domain, Activity, Severity
from stearns_boomerang.handlers import ActivityHandler, LoggerActivityHandler, KafkaActivityHandler
import pytest
from unittest.mock import patch

my_domain = Domain(
    code='{b10fa050-4c9c-4d5e-8244-0c3ec08d7bea}',
    label='TestMeDomain',
    description='Encapsulates basic application lifecycle events'
)

TaskStart: Callable[[], Activity] = partial(
    Activity, code='{4554e7d4-751d-4ccf-a07a-e202f5b2af3d}', domain=my_domain,
    label='Task Starting',
    description='Called when task initially starts',
    severity=Severity.INFO,
    message_format=None)

task_start: Activity = TaskStart(task_name='AwesomeTask', source='Test')


class ActivityCollectorTestSuite(unittest.TestCase):

    def test_exceptions(self):
        with pytest.raises(NotImplementedError):
            ActivityHandler().handle(None)

    def test_null_message_logger(self):
        LoggerActivityHandler().handle(task_start)

    def test_prepare(self):
        ActivityHandler().prepare()

    def test_finalize(self):
        ActivityHandler().finalize()

    @patch('kafka.KafkaProducer')
    def test_kafka_activity_handler(self, mock_producer):
        with pytest.raises(AttributeError):
            handler = KafkaActivityHandler(
                application='boomerang',
                elastic_index='testing',
                brokers=None,
            )
        with pytest.raises(AttributeError):
            handler = KafkaActivityHandler(
                application='boomerang',
                elastic_index='testing',
                brokers=['0.0.0.0'],
                topic=None
            )
        with pytest.raises(AttributeError):
            handler = KafkaActivityHandler([], None, '')
        with pytest.raises(AttributeError):
            handler = KafkaActivityHandler(['0.0.0.0'], None, None, None)
        handler = KafkaActivityHandler(
            brokers=['0.0.0.0'],
            application='boomerang',
            elastic_index='testing',
            topic='testing',
            extend_life=True,
            producer=mock_producer
        )

        for i in range(10000):
            handler.handle(task_start, test=True, name='Caleb', i=i)
        handler.finalize()
