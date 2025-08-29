#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from stearns_boomerang.handlers import LogKittyActivityHandler
from stearns_boomerang.reporters import DevNullReporter
import pytest


class BasicReporterTestSuite(unittest.TestCase):

    def test_default(self):
        reporter = DevNullReporter()
        with pytest.raises(RuntimeError):
            reporter.add_handlers([LogKittyActivityHandler()])
