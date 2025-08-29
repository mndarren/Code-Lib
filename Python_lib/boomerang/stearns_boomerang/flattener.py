#!/usr/bin/env python
# -*- coding: utf-8 -*-

# created by Julia Ortiz
"""
.. currentmodule:: module
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

Briefly describe the module.
"""

from typing import Mapping
import jsonpickle


def flatten(data, level: int = 1, reducer: str = '.'):
    """
    Encode the data
    :param data:
    :param level:
    :param reducer:
    :return:
    """
    if data is None:
        return {}

    data = jsonpickle.decode(jsonpickle.encode(data, unpicklable=False))

    flat_dict = {}

    def _flatten(d, current_level: int, parent=None, json_level: int = 0):
        """

        :param d:
        :param current_level:
        :param parent:
        :param json_level:
        :return:
        """
        for key, val in d.items():
            flat_key: str = parent + reducer + key if parent else key
            if key.upper() == 'ARGS':
                for i in range(len(val)):  # pylint: disable=consider-using-enumerate
                    flat_dict[flat_key + reducer + str(i + 1)] = val[i]
            else:
                if 'kwargs' + reducer in flat_key:
                    flat_key = flat_key.replace('kwargs' + reducer, '')
                if isinstance(val, Mapping) and current_level < json_level:
                    _flatten(val, parent=flat_key, current_level=current_level+1, json_level=level)
                else:
                    flat_dict[flat_key] = val
    if level > 0:
        _flatten(data, current_level=0, json_level=level)
    else:
        flat_dict['data'] = data
    return flat_dict
