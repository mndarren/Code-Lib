#!/usr/bin/env python
# -*- coding: utf-8 -*-

# created by Stearns Financial Center
"""
.. currentmodule:: collectors
.. moduleauthor:: Stearns Financial Center <SoftwareDevelopment@stearnsbank.com>

"""
from abc import ABCMeta, abstractmethod
from threading import Lock
from typing import List, Dict
import tempfile
import datetime
import uuid
import pickle
import os
from stearns_boomerang.activities import Activity, Severity
from stearns_boomerang.handlers import ActivityHandler


class ActivityRecap:
    """
    An activity recap contains information about a collection of activities.
    """
    def __init__(self, activity: Activity, data: Dict = None):
        self.activity = activity
        self.data = data
        self.invoked: datetime.datetime = datetime.datetime.utcnow()


class ApplicationReview:
    """
    Formatted recap
    """
    def __init__(self):
        self._domain_dict: Dict[uuid.UUID, List[ActivityRecap]] = {}

    @property
    def domain_dict(self):
        """

        :return:
        """
        for _, value in self._domain_dict.items():
            value.sort(key=lambda x: x.invoked, reverse=False)
        return self._domain_dict.copy()

    def get_severity_counts(self) -> Dict[str, int]:
        """

        :return:
        """
        severity_dict: Dict[str, int] = {}
        for severity in Severity:
            severity_dict[severity.name] = 0
        for _, activity_recap_list in self._domain_dict.items():
            for activity_recap in activity_recap_list:
                severity_dict[activity_recap.activity.severity.name] += 1
        return severity_dict

    def add_activity_recap(self, activity_recap: ActivityRecap):
        """

        :param activity_recap:
        :return:
        """
        if activity_recap.activity.domain.code in self._domain_dict:
            self._domain_dict[activity_recap.activity.domain.code].append(activity_recap)
        else:
            self._domain_dict[activity_recap.activity.domain.code] = [activity_recap]


class ActivityCollector(ActivityHandler):
    """
    An activity recap handler is an activity handler that makes notes about all the reported activities and can provide
    a recap.
    """
    __metaclass__ = ABCMeta

    @abstractmethod
    def handle(self, activity: Activity, *args, **kwargs):
        """
        Retrieve a recap of the activities recorded.
        :return: an activity recap
        """
        raise NotImplementedError('Method Implement Method')

    @abstractmethod
    def get_recap(self) -> ApplicationReview:
        """
        Retrieve a recap of the activities recorded.
        :return: an activity recap
        """
        raise NotImplementedError('Method Implement Method')


class LocalActivityCollector(ActivityCollector):
    """
    This is a recap handler that compiles information to the local file system.
    """

    def __init__(self, data_path: str):
        super().__init__()
        if not os.path.isdir(data_path):
            os.makedirs(data_path)
        self._data_path = data_path
        self._filename = tempfile.NamedTemporaryFile('w', dir=self._data_path).name
        self._file_handle = None
        self._write_lock: Lock = Lock()

    def handle(self, activity: Activity, *args, **kwargs):
        """

        :param activity:
        :param args:
        :param kwargs:
        :return:
        """
        data = {
            'args': tuple(args),
            'kwargs': {**kwargs}
        }
        # We're expecting only one thread at a time will be writing, but it's really important that be true.
        with self._write_lock:
            pickle.dump(ActivityRecap(activity, data), self._file_handle)

    def get_recap(self) -> ApplicationReview:
        """
        Go to the data directory.
        Open *all* the files.
        Scan 'em.
        Compile a data structure.
        Return the data structure.

        :return:
        """

        # Compile a list of paths to all the data files.
        df_paths = [
            os.path.join(self._data_path, f)
            for f in os.listdir(self._data_path)
            if os.path.isfile(os.path.join(self._data_path, f))
        ]
        # Let's hold all the data items in a list as we retrieve them.
        data: ApplicationReview = ApplicationReview()
        for path in df_paths:
            with open(path, 'rb') as fin:
                while True:
                    try:
                        data.add_activity_recap(pickle.load(fin))
                    except EOFError:
                        break
        return data

    def open(self):
        """

        :return:
        """
        # If we haven't already opened the file...
        if self._file_handle is None:
            # Create the output directory (if it doesn't already exist).
            os.makedirs(self._data_path, exist_ok=True)
            # Create the new file in "append" mode.
            self._file_handle = open(os.path.join(self._data_path, self._filename), 'wb')

    def close(self):
        """

        :return:
        """
        if self._file_handle is not None:
            self._file_handle.close()

    def prepare(self):
        """

        :return:
        """
        self.open()

    def finalize(self):
        """

        :return:
        """
        self.close()

    def __enter__(self):
        """

        :return:
        """
        self.prepare()
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        """

        :param exc_type:
        :param exc_val:
        :param exc_tb:
        :return:
        """
        self.finalize()
