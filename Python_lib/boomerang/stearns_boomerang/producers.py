#!/usr/bin/env python
# -*- coding: utf-8 -*-

# created by Stearns Financial Center
"""
.. currentmodule:: producers
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

"""
from typing import List
from abc import ABCMeta
from prettytable import PrettyTable
from stearns_boomerang.collectors import ActivityCollector
from stearns_boomerang.flattener import flatten
from .html.handler import HtmlTableHandler


class Producer:
    """
    producer
    """

    __metaclass__ = ABCMeta

    def __init__(self, collector: ActivityCollector):
        """

        :param collector:
        """
        self._collector: ActivityCollector = collector

    def produce(self, *args, **kwargs):
        """

        :param args:
        :param kwargs:
        :return:
        """
        raise NotImplementedError('Whoops! You need to implement the produce method...')


class TableProducer(Producer):
    """
    table producer
    """

    def produce(self, flatten_level: int = 1, *args, **kwargs):  # pylint: disable=keyword-arg-before-vararg,arguments-differ
        """

        :param flatten_level:
        :param args:
        :param kwargs:
        :return:
        """
        print(self.get_severity_table())
        domain_tables = self.get_domain_recap_tables(flatten_level)
        for domain_table in domain_tables:
            print(domain_table)

    def get_severity_table(self) -> PrettyTable:
        """

        :return:
        """
        severity_table: PrettyTable = PrettyTable(['Severity', 'Invocations'])
        severity_table.align['Severity'] = 'l'
        for severity, count in self._collector.get_recap().get_severity_counts().items():
            severity_table.add_row([severity, count])
        return severity_table

    def get_domain_recap_tables(self, flatten_level: int) -> List[PrettyTable]:
        """

        :param flatten_level:
        :return:
        """
        domain_recap_tables: List[PrettyTable] = []
        for _, activity_recap_list in self._collector.get_recap().domain_dict.items():
            default_headers = ['Activity ID', 'Name', 'Invoked Time', 'Severity']
            data_headers = self._get_data_headers(flatten_level)
            domain_table = PrettyTable(default_headers + data_headers)
            domain_table.align = 'l'
            domain = None
            for activity_recap in activity_recap_list:
                if domain is None:
                    domain = activity_recap.activity.domain
                default_row = [
                    activity_recap.activity.code,
                    activity_recap.activity.label,
                    activity_recap.invoked,
                    activity_recap.activity.severity
                ]
                activity_data = flatten(activity_recap.data, level=flatten_level)
                data_row = []
                for key in data_headers:
                    data_row.append(activity_data[key] if key in activity_data else None)
                domain_table.add_row(default_row + data_row)
            if domain is not None:
                domain_table.title = domain.code
            domain_recap_tables.append(domain_table)
        return domain_recap_tables

    def _get_data_headers(self, flatten_level: int) -> List[str]:
        """

        :param flatten_level:
        :return:
        """
        headers: List[str] = []
        domain_dict = self._collector.get_recap().domain_dict
        for _, activity_recap_list in domain_dict.items():
            for activity_recap in activity_recap_list:
                flattened_keys = flatten(activity_recap.data, level=flatten_level).keys()
                for key in flattened_keys:
                    if key not in headers:
                        headers.append(key)
        headers.sort()
        return headers

    def create_html_tables(self, output: str = 'html_tables.html', flatten_level: int = 1):
        """

        :param output:
        :param flatten_level:
        :return:
        """
        table_handler: HtmlTableHandler = HtmlTableHandler(output_file=output)
        domain_tables = self.get_domain_recap_tables(flatten_level=flatten_level)
        for domain_table in domain_tables:
            table_handler.add_headers(domain_table.title, domain_table.field_names)
            table_handler.add_rows(domain_table.title, domain_table._rows)  # pylint: disable=protected-access
        table_handler.create_html()
