#!/usr/bin/env python
# -*- coding: utf-8 -*-

import unittest
from functools import partial
from typing import Callable, List
from stearns_boomerang.activities import Activity, Domain, Severity
from stearns_boomerang.producers import TableProducer, Producer
from stearns_boomerang.collectors import ActivityCollector, ApplicationReview, ActivityRecap
import tempfile
import os
import pytest


my_domain = Domain(
    code='{b10fa050-4c9c-4d5e-8244-0c3ec08d7bea}',
    label='TestMeDomain',
    description='Encapsulates basic application lifecycle events'
)

TaskStart: Callable[[], Activity] = partial(
    Activity, code='{4554e7d4-751d-4ccf-a07a-e202f5b2af3d}', domain=my_domain,
    label='Task Starting',
    description='Called when task initially starts',
    severity=Severity.INFO,
    message_format='{task_name} is starting')

task_start: Activity = TaskStart(task_name='AwesomeTask', source='Test')


class TestCollector(ActivityCollector):

    def get_all_activities(self) -> List[ActivityRecap]:
        activities: List[ActivityRecap] = []

        for domain, activity_recap_list in self.get_recap().domain_dict.items():
            for activity in activity_recap_list:
                activities.append(activity)

        return activities

    def handle(self, activity: Activity, *args, **kwargs):
        pass

    def get_recap(self) -> ApplicationReview:
        app_review: ApplicationReview = ApplicationReview()
        app_review.add_activity_recap(
            activity_recap=ActivityRecap(
                activity=task_start,
                data={
                    'args': (1, True, False),
                    'kwargs': {
                        'test': '123'
                    }
                }
            )
        )
        app_review.add_activity_recap(
            activity_recap=ActivityRecap(activity=task_start)
        )
        return app_review


class ProducersTestSuite(unittest.TestCase):

    collector: ActivityCollector = TestCollector()

    def test_table_producer(self):
        producer = TableProducer(ProducersTestSuite.collector)
        producer.produce()
        self.assertEqual(1, len(producer.get_domain_recap_tables(flatten_level=0)))
        with tempfile.TemporaryDirectory() as temp:
            temp_dir = temp
        temp_file_name = os.path.join(temp_dir, 'fake.html')
        producer.create_html_tables(output=temp_file_name)
        self.assertEqual(True, os.path.isfile(temp_file_name))

    def test_producer(self):
        with pytest.raises(NotImplementedError):
            Producer(ProducersTestSuite.collector).produce()
