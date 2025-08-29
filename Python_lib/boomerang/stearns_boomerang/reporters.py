#!/usr/bin/env python
# -*- coding: utf-8 -*-

# created by Stearns Financial Center
"""
.. currentmodule:: reporters
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

"""
from typing import List, Iterable
import logging
from interface import Interface
from .handlers import ActivityHandler, ElasticSearchHandler
from .activities import Activity


class Reporter:
    """
    Boomerang Reporter
    """

    def __init__(self, *handlers: Iterable[ActivityHandler]):
        """
        :param handlers: activity handlers you would like to handle activities reported through this reporter
        """
        self._handlers: List[ActivityHandler] = list(*handlers)  #: the registered activity handlers
        self._logger: logging.Logger = self.get_logger()

    def report(self, activity: Activity, *args, **kwargs):
        """
        Report an activity.
        """
        if not self._handlers:
            self._handlers.extend([ElasticSearchHandler()])
        # pylint: disable=W0511, fixme
        # TODO: This kind of thing should eventually be async.
        for handler in self._handlers:
            handler.handle(activity, *args, **kwargs)

    def add_handlers(self, *handlers: Iterable[ActivityHandler]):
        """
        Add a handler to this reporter.
        :param handlers: activity handlers you would like to handle activities reported through this reporter
        """
        self._handlers.extend(list(*handlers))

    @classmethod
    def get_logger(cls) -> logging.Logger:
        """

        :return:
        """
        mod = cls.__module__
        name = cls.__name__
        return logging.getLogger('{mod}.{cls}'.format(mod=mod,
                                                      cls=name))


class DevNullReporter(Reporter):
    """
    Dev Null Reporter
    """

    def report(self, activity: Activity, *args, **kwargs):
        """

        :param activity:
        :param args:
        :param kwargs:
        :return:
        """
        pass

    def add_handlers(self, *handlers: Iterable[ActivityHandler]):
        """

        :param handlers:
        :return:
        """
        raise RuntimeError('Cannot add handler to DevNullReporter')


#  pylint: disable=no-init
class ExternalRecordReporter(Interface):
    """
    This is an interface that is compatible with the :py:class:`stearns_boomerang.reporters.Reporter` but
    puts additional constraints on the :py:func:`report` method.
    """

    def report(self,
               activity: Activity,
               ext_system: str,
               ext_system_id: str,
               *args, **kwargs):
        """
        Report an activity.

        :param activity: the activity
        :param ext_system: the external system indicator
        :param ext_system_id: the external system ID
        """
        pass


def cast(typ, val):  # pylint: disable=unused-argument
    """Cast a value to a type.

    This returns the value unchanged.  To the type checker this
    signals that the return value has the designated type, but at
    runtime we intentionally don't check anything (we want this
    to be as fast as possible).
    """
    return val
