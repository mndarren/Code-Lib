# Python Test
==========================================
1. Mock env vars with mock lib
```
@mock.patch.dict(os.environ, {'KAFKA_SERVERS': '', 'KAFKA_DIRTY_ADDRESS_CONSOLIDATOR_TOPIC': '',
                                  'KAFKA_CLEAN_ADDRESS_CONSOLIDATOR_TOPIC': ''})
def setUp(self):
    set_env()
    pod_obj = AddressConsolidatorPod(
        kafka_servers=get_env('KAFKA_SERVERS'),
        producer_topic=get_env('KAFKA_DIRTY_ADDRESS_CONSOLIDATOR_TOPIC'),
        consumer_topic=get_env('KAFKA_CLEAN_ADDRESS_CONSOLIDATOR_TOPIC')
    )
    self.mock_kafka = MockKafka(pod_object=pod_obj)
    self.task_pod = self.mock_kafka
    self.logic_pod = self.mock_kafka

    self.logic_pod.start_daemon()
```
2. Mock env vars with EnvironmentVarGuard
```
from test.support import EnvironmentVarGuard # Python >=3
from unittest import TestCase

class MyTestCase(TestCase):
    def setUp(self):
        self.env = EnvironmentVarGuard()
        self.env.set('VAR', 'value')

    def test_something(self):
        with self.env:
            # ... perform tests here ... #
            pass
```
