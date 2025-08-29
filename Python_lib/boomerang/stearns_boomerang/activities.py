#!/usr/bin/env python
# -*- coding: utf-8 -*-

# created by Stearns Financial Center
"""
.. currentmodule:: activities
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

"""
from enum import Enum
import logging
from typing import Any, Dict, Type
import uuid


class Domain:
    """
    An activity domain is used to group activities that perform activities that can be considered to be part.
    """
    def __init__(self,
                 code: str or uuid.UUID,
                 label: str,
                 description: str = None):
        # Sanity check:
        if code is None:
            raise ValueError("'code' cannot be None.")
        self._code = uuid if isinstance(uuid, uuid.UUID) else uuid.UUID(code)
        self._label = label
        self._description = description if description is not None else label

    def to_dict(self) -> Dict[str, Any]:
        """

        :return:
        """
        data = {
            'code': str(self._code),
            'label': self._label,
            'description': self._description
        }
        return data

    @property
    def code(self) -> uuid.UUID:
        """
        Getter to the unique identifier for the Domain
        :return: Unique Domain Identifier
        """
        return self._code

    @property
    def label(self) -> str:
        """
        Getter to the label for the Domain
        :return: Label of the Domain
        """
        return self._label

    @property
    def description(self) -> str:
        """
        Getter to the description of the Domain
        :return: Description of the Domain
        """
        return self._description


class Label(Enum):
    """
    This represents who the log is targeted to
    """
    APPLICATION = 1
    DATA_QUALITY = 2


class Severity(Enum):
    """
    These values indicate the general severity of an activity performed within a task.  Each value corresponds to a
    python :py:mod:`logging` level.
    """
    DEBUG = logging.DEBUG  #: The activity was only performed as part of a debugging operation
    INFO = logging.INFO  #: The activity succeeded.
    WARNING = logging.WARNING  #: The activity succeeded but generated a warning.
    ERROR = logging.ERROR  #: The activity failed due to an error.
    CRITICAL = logging.CRITICAL  #: The activity failed, causing the complete failure of the task.


class Activity:
    """
    An Activity is a child of a parent domain, it is used to uniquely identify and
    report on a specific activity/event code
    """
    def __init__(self,
                 code: str or uuid.UUID,
                 domain: Domain,
                 severity: Severity,
                 label: str or Label,
                 message_format: str = '{message}',
                 description: str = None,
                 source: Type or str = None,
                 **kwargs):
        """

        :param code:
        :param domain:
        :param severity:
        :param label:
        :param message_format:
        :param description:
        :param source:
        :param kwargs:
        """

        self._domain = domain
        self._code = uuid if isinstance(uuid, uuid.UUID) else code
        self._severity = severity
        self._message_format = message_format
        self._label = label
        self._description = description if description is not None else label
        self._kwargs = dict(**kwargs)
        # Augment variable keyword arguments with the actual keyword arguments.
        self._kwargs['code'] = str(self._code)
        self._kwargs['domain'] = str(self._domain.code) if hasattr(self._domain, 'code') else self._domain
        self._kwargs['label'] = self._label
        self._kwargs['description'] = self.description
        # Store the severity enumeration name.
        self._kwargs['severity'] = self._severity.name if hasattr(self._severity, 'name') else self._severity
        # Establish the source.
        self._source: str = None if source is None else (
            source if isinstance(source, str) else "{}.{}".format(source.__module__, source.__name__)
        )
        # Update the keyword arguments for the sake of formatting.
        self._kwargs['source'] = self._source

    @property
    def code(self) -> uuid.UUID:
        """
        Getter to the unique identifier for the Activity
        :return: Unique Identifier for the Activity
        """
        return self._code

    @property
    def domain(self) -> Domain:
        """
        Getter to the Domain for the Activity
        :return: Domain Object for the Activity
        """
        return self._domain

    @property
    def label(self) -> str:
        """
        Getter to the label for the Activity
        :return: Label of the Activity
        """
        return self._label

    @property
    def description(self) -> str:
        """
        Getter to the description of the Activity
        :return: Description of the Activity
        """
        return self._description

    @property
    def source(self) -> str:
        """
        Getter to the source of the Activity
        :return: Calling Source of the Activity
        """
        return self._source

    @property
    def severity(self) -> Severity:
        """
        Getter to the severity of the Activity
        :return: Severity of the Activity
        """
        return self._severity

    def get_message(self) -> str or None:
        """
        Gets the formatted message of the Activity
        :return: Formatted Message of the Activity
        """
        if not self._message_format:
            return None
        return self._message_format.format(**self._kwargs)
