# Kafka Training
=====================================
0. Concepts
```
Topic,
Key -> Example: msg(1, v1), msg(1, v2). 1 is the key
Broker
Consumer -> Consume from a topic,
Producer -> Produce to a topic. 2 formats: batch.size and 1 msg/time
Zookeeper -> load balancer
```
1. Partitions
```
It's cheap. (5 min, 10 mid, 20 max recommend)
topic settings
```
2. Consumer
```
Consumer(
	groupid: my_id,
	auto.offset.reset: smallest - largest
	)  # based on # of msg and # MB/msg
```
3. Lag of consumer
```
Examples: 100/input
          0-10 11 12-100
Terminated after 3 tries
```
4. Logstash does not have error handling, so python code should handle the errors
5. Security note: one who can write to Kafka can read it.
6. Don't use REST API to communicate with Elastic.
7. Version note: 6.0.1 to 6.1.0 probably gets error. 6.0.1 to 6.2.1 no problem.
8. for loop to access Kafka will fire the threshold.
