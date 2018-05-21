#!/usr/bin/env python
# -*- coding: utf-8 -*-

# created by Darren Xie
"""
.. currentmodule:: little_tools
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

Address Reconciler
"""

from typing import List
import dateparser
import datetime


class LittleTools(object):

    @staticmethod
    def is_empty_string(string_to_check: str or None):
        """"""
        if string_to_check is None or len(string_to_check.strip()) == 0:
            return True
        return False

    @staticmethod
    def get_first_index_of_list(a_list: List, target):
        """"""
        if a_list is not None and target is not None:
            for i in range(0, len(a_list)):
                if target == a_list[i].item:
                    return i
        return -1

    @staticmethod
    def parse_date_from_string(date_string: str):
        """"""
        if date_string is not None:
            return dateparser.parse(date_string,
                                    languages=['en'],
                                    settings={'TIMEZONE': 'UTC', 'TO_TIMEZONE': 'UTC'})
        return None

    @staticmethod
    def parse_string_from_date(date: datetime):
        """
        Parse the date into a string
        :return:
        """
        if date is not None and isinstance(date, datetime):
            return date.strftime("%Y-%m-%d %H:%M:%S")
        return date

    @staticmethod
    def right_rotation_for_list(a_list: List):
        """[:] will delete original list to save memory space"""
        if len(a_list) > 0:
            a_list[:] = a_list[1:] + [a_list[0]]
        return a_list

    @staticmethod
    def left_rotation_for_list(a_list: List):
        """[:] will delete original list to save memory space"""
        if len(a_list) > 0:
            a_list[:] = a_list[-1] + [a_list[:-2]]
        return a_list

    @staticmethod
    def double_quotes(quote_str: str) -> str:
        """
        Single quote to double quote mark in the string.
        It's used by inserting city or place name data into DB.
        """
        list_str = list(quote_str)
        new_list = list()
        for x in list_str:
            if x != '\'':
                new_list.append(x)
            else:
                new_list.extend(['\'', '\''])
        return ''.join(new_list)
