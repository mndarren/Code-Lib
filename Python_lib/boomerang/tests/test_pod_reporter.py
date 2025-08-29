#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest

from stearns_boomerang.handlers import ActivityHandler, LogKittyActivityHandler, ConsoleActivityHandler, \
    LoggerActivityHandler
from stearns_boomerang.reporters import ExternalRecordReporter, cast, Reporter
from tests.test_basic_reporter import NewDomain


class BasicReporterTestSuite(unittest.TestCase):

    def test_pod_reporter(self):
        reporter = Reporter()
        reporter.add_handlers([LogKittyActivityHandler(), ConsoleActivityHandler(), LoggerActivityHandler()])
        (cast(ExternalRecordReporter, reporter)).report(activity=NewDomain(dippy='DomainGermain'),
                                                        ext_system='ITI',
                                                        ext_system_id='2001')
