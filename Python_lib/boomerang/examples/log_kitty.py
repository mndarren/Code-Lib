from functools import partial
from stearns_boomerang.activities import Domain, Severity, Activity
from stearns_boomerang.handlers import LogKittyActivityHandler
from stearns_boomerang.collectors import LocalActivityCollector
from stearns_boomerang.reporters import Reporter
from stearns_boomerang.producers import TableProducer
from typing import Callable
import tempfile
import os


temp_dir = tempfile.TemporaryDirectory().name
with LocalActivityCollector(data_path=temp_dir) as collector:
    reporter = Reporter([collector, LogKittyActivityHandler()])


    class FancyTest(object):

        def __init__(self, a, b, c):
            self.a = a
            self.b = b
            self.c = c


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

    task_start: Activity = TaskStart(task_name='AwesomeTask')
    task_failed: Activity = TaskFailure(task_name='AwesomeTask')
    domain_created: Activity = NewDomain(dippy='DomainGermain')
    reporter.report(task_failed, test=FancyTest(False, 123, 'TREE'))
    reporter.report(task_start, test=FancyTest(True, 123, 'TREE'))
    reporter.report(task_start, test=FancyTest(True, 123, 'TREE'))
    reporter.report(task_failed, test=FancyTest(True, 123, 'TREE'))
    reporter.report(domain_created, test=FancyTest(True, 123, 'TREE'))

    producer = TableProducer(collector)
    producer.produce(flatten_level=2)
    producer.create_html_tables(output=os.path.join(temp_dir, 'output_table.html'), flatten_level=2)
