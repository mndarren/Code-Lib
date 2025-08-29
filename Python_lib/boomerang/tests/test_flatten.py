#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from stearns_boomerang.handlers import flatten
import pytest


class BasicReporterTestSuite(unittest.TestCase):

    def test_flatten_none(self):
        x = flatten(None)
        self.assertEqual({}, x)

    def test_flatten_args(self):
        x = flatten({
            'args': (1, 2, 3)
        })
        self.assertEqual({'args.1': 1, 'args.2': 2, 'args.3': 3}, x)
