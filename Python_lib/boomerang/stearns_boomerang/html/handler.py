#!/usr/bin/env python
# -*- coding: utf-8 -*-

# created by Stearns Financial Center
"""
.. currentmodule:: html_handler
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

"""
from typing import List, Any, Dict
import os
from distutils.dir_util import copy_tree  # pylint: disable=no-name-in-module, import-error


class HtmlTable:
    """
    HTML table
    """

    def __init__(self, name: str, headers: List[str] = None, rows: List[List[Any]] = None):
        """

        :param name:
        :param headers:
        :param rows:
        """
        self.name = name
        self.headers = headers if headers is not None else []
        self.rows: List[List[Any]] = rows if rows is not None else []

    def add_header(self, name: str):
        """

        :param name:
        :return:
        """
        if name not in self.headers:
            self.headers.append(name)
        else:
            raise KeyError('Header ({header}) already exists...'.format(header=name))

    def add_row(self, row: List[Any]):
        """

        :param row:
        :return:
        """
        if row is not None:
            self.rows.append(row)


class HtmlTableHandler:
    """
    HTML Table Handler
    """

    def __init__(self, output_file: str):
        """

        :param output_file:
        """
        self.output_file = output_file
        self.tables: Dict[str, HtmlTable] = {}

    def create_html(self):
        """

        :return:
        """
        html = """
                <!DOCTYPE html>
                <html lang="en">
                <head>
                <meta charset="UTF-8">
                <title>Report</title>
                <link rel="stylesheet" href="./mdl/material.min.css">
                <link rel="stylesheet" href="./mdl/site.css">
                <script src="./mdl/material.min.js"></script>
                </head>
                <body>
                    <div class="mdl-tabs mdl-js-tabs mdl-js-ripple-effect">
                    {tabs}
                    {tables}
                    </div>
                </body>
                </html>
               """.format(tabs=self.get_tabs(), tables=self.get_tables())

        # Copy Assets
        os.makedirs(os.path.dirname(self.output_file), exist_ok=True)
        copy_tree(os.path.join(os.path.dirname(__file__), 'assets'), os.path.dirname(self.output_file))
        with open(self.output_file, 'w') as file:
            file.write(html)

    def add_headers(self, table_name: str, headers: List[str]):
        """

        :param table_name:
        :param headers:
        :return:
        """
        for header in headers:
            self.add_header(table_name=table_name, header=header)

    def add_header(self, table_name: str, header: str):
        """

        :param table_name:
        :param header:
        :return:
        """
        if table_name in self.tables:
            self.tables[table_name].add_header(header)
        else:
            self.tables[table_name] = HtmlTable(name=table_name, headers=[header])

    def add_rows(self, table_name: str, rows: List[List[str]]):
        """

        :param table_name:
        :param rows:
        :return:
        """
        for row in rows:
            self.add_row(table_name=table_name, row=row)

    def add_row(self, table_name: str, row: List[Any]):
        """

        :param table_name:
        :param row:
        :return:
        """
        if table_name in self.tables:
            self.tables[table_name].add_row(row)
        else:
            self.tables[table_name] = HtmlTable(name=table_name, rows=[row])

    def get_tabs(self) -> str:
        """
        get tabs
        """
        tabs = ''
        first = True
        for table in self.tables:
            if first:
                tabs += '\t\t\t<a href="#{name}" class="mdl-tabs__tab is-active">{name}</a>\n'.format(
                    name=table
                )
                first = False
            else:
                tabs += '\t\t\t<a href="#{name}" class="mdl-tabs__tab">{name}</a>\n'.format(
                    name=table
                )
        return tabs

    def get_tables(self) -> str:
        """

        :return:
        """
        tabs = ''
        first = True
        for key, table in self.tables.items():
            if first:
                tabs += '\t\t<div class="mdl-tabs__panel is-active" id="{name}">\n{table}\n\t\t</div>'.format(
                    name=key,
                    table=self._get_table(table)
                )
                first = False
            else:
                tabs += '\t\t<div class="mdl-tabs__panel" id="{name}">\n{table}\n\t\t</div>'.format(
                    name=key,
                    table=self._get_table(table)
                )
        return tabs

    def _get_table(self, table: HtmlTable) -> str:
        """

        :param table:
        :return:
        """
        table = \
            '<table class="wide-table mdl-data-table mdl-js-data-table mdl-data-table--selectable mdl-shadow--2dp">' \
            '<thead>{headers}</thead>' \
            '<tbody>{body}</tbody>' \
            '</table>'.format(
                headers=self._get_table_headers(table),
                body=self._get_table_rows(table)
            )
        return table

    @staticmethod
    def _get_table_headers(table: HtmlTable) -> str:
        """

        :param table:
        :return:
        """
        headers = ''
        for header in table.headers:
            headers += '<th>{header}</th>'.format(header=header)
        table_headers = '<tr>{headers}</tr>'.format(headers=headers)
        return table_headers

    @staticmethod
    def _get_table_rows(table: HtmlTable) -> str:
        """

        :param table:
        :return:
        """
        rows = ''
        for row in table.rows:
            data = ''
            for entry in row:
                data += '<td>{entry}</td>'.format(entry=entry)
            rows += '<tr>{data}</tr>\n'.format(data=data)
        return rows
