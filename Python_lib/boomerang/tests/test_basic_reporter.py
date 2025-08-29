#!/usr/bin/env python
# -*- coding: utf-8 -*-
import tempfile
import unittest
from functools import partial
import os
from stearns_boomerang.activities import Domain, Severity, Activity
from stearns_boomerang.collectors import LocalActivityCollector
from stearns_boomerang.handlers import LogKittyActivityHandler, LoggerActivityHandler, ConsoleActivityHandler
from stearns_boomerang.producers import TableProducer
from stearns_boomerang.reporters import Reporter, DevNullReporter
from typing import Callable
import pytest

my_domain = Domain(
    code='{b10fa050-4c9c-4d5e-8244-0c3ec08d7bea}',
    label='TestMeDomain',
    description='Encapsulates basic application lifecycle events'
)

another_domain = Domain(
    code='{b10fa050-4c9c-4d5e-8244-0c3ec08d6bea}',
    label='WowGreatDomain',
    description='Serves to create another instance of a domain'
)

TaskStart: Callable[[], Activity] = partial(
    Activity, code='{4554e7d4-751d-4ccf-a07a-e202f5b2af3d}', domain=my_domain,
    label='Task Starting',
    description='Called when task initially starts',
    severity=Severity.INFO,
    message_format='{task_name} is starting')

TaskFailure: Callable[[], Activity] = partial(
    Activity, code='{4554e7d4-751d-4ccf-a07a-e202f5b2af3e}', domain=my_domain,
    label='Task Failing',
    description='Called when task fails',
    severity=Severity.ERROR,
    message_format='{task_name} has failed')

NewDomain: Callable[[], Activity] = partial(
    Activity, code='{4554e7d3-751d-4ccf-a07a-e202f5b2af3e}', domain=another_domain,
    label='New Domain Created',
    description='Called when a new domain is created',
    severity=Severity.WARNING,
    message_format='{dippy} created')


class BasicReporterTestSuite(unittest.TestCase):

    def test_collector(self):
        with tempfile.TemporaryDirectory() as temp:
            temp_dir = temp
        with LocalActivityCollector(data_path=temp_dir) as collector:
            reporter = Reporter([collector])
            reporter.add_handlers([LogKittyActivityHandler(), ConsoleActivityHandler(), LoggerActivityHandler()])

            task_start: Activity = TaskStart(task_name='AwesomeTask', source='Test')
            task_failed: Activity = TaskFailure(task_name='AwesomeTask')
            domain_created: Activity = NewDomain(dippy='DomainGermain')
            reporter.report(domain_created, None)
            reporter.report(domain_created, None)
            reporter.report(task_start, '123')
            reporter.report(task_start, '123')
            reporter.report(task_failed, None)
            reporter.report(task_failed, None)

            recap = collector.get_recap()
            self.assertNotEqual(recap, None)
            self.assertEqual(2, len(recap.domain_dict.keys()))
            for severity, count in recap.get_severity_counts().items():
                self.assertGreaterEqual(count, 0)
            for domain_code, activity_recap_list in recap.domain_dict.items():
                for activity_recap in activity_recap_list:
                    self.assertNotEqual(None, activity_recap.activity.domain.description)

            producer = TableProducer(collector)
            producer.produce(flatten_level=2)
            producer.create_html_tables(output=os.path.join(temp_dir, 'output_table.html'), flatten_level=2)

    def test_null_domain_code(self):
        with pytest.raises(ValueError):
            another_domain = Domain(
                code=None,
                label='WowGreatDomain',
                description='Serves to create another instance of a domain'
            )

    def test_null_activity_message(self):
        null_me: Activity = TaskStart(task_name='AwesomeTask', source='Test', message_format=None)
        self.assertEqual(None, null_me.get_message())

    def test_dev_null_reporter(self):
        with pytest.raises(RuntimeError):
            DevNullReporter().report(None)
            DevNullReporter().add_handlers()


