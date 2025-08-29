#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from stearns_boomerang.collectors import ActivityCollector
import pytest


class ActivityCollectorTestSuite(unittest.TestCase):

    def test_exceptions(self):
        with pytest.raises(NotImplementedError):
            ActivityCollector().get_recap()
        with pytest.raises(NotImplementedError):
            ActivityCollector().handle(None)
