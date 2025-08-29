#!/usr/bin/env python
# -*- coding: utf-8 -*-

import pytest
import unittest
from stearns_boomerang.producers import HtmlTableHandler

class HtmlTableHandlerTestSuite(unittest.TestCase):

    def test_duplicate_headers_exceptions(self):
        with pytest.raises(KeyError):
            table_handler = HtmlTableHandler(output_file='/tmp/test.html')
            table_handler.add_headers('domain_table.title', 'domain_table.field_names')
            table_handler.add_headers('domain_table.title', 'domain_table.field_names2')
            table_handler.create_html()

    def test_duplicate_row_exceptions(self):
        table_handler = HtmlTableHandler(output_file='/tmp/test.html')
        table_handler.add_rows('domain_table.title', 'domain_table._rows1')  # pylint: disable=protected-access
        table_handler.add_rows('domain_table.title', 'domain_table._rows2')  # pylint: disable=protected-access
        table_handler.create_html()
