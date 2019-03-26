# Good Points Python
==========================================
1. De-dup List[Dict]
```
[dict(t) for t in {tuple(d.items()) for d in rel_lsit}]
```
2. List IntEnum class numbers
```
list(map(int, <IntEnumClass>))
```
3. csv to json
```
def csv_to_json(csv_file, json_file, sample_data_dict):
    import csv
    import json

    csvfile = open(csv_file, 'r')
    jsonfile = open(json_file, 'w')

    fieldnames = tuple(list(sample_data_dict.keys()))
    next(csvfile)
    reader = csv.DictReader(csvfile, fieldnames)
    jsonfile.write('[')
    for row in reader:
        json.dump(row, jsonfile)
        jsonfile.write(',\n')
    jsonfile.write(']')

    csvfile.close()
    jsonfile.close()
```
4. get the path of a file in project
```
import os
from os.path import join, abspath, dirname
current_path = dirname(abspath(__file__))
config_path = join(abspath(join(current_path, os.pardir)),
                   'something_external_event_notifier/notifier_config.yml')
```
5. email notifier
```
import smtplib
import yaml


class EmailConfig:
    """
    Email Config properties.
    """
    def __init__(self, yml_config='notifier_config.yml'):
        """
        Read values from config yaml file
        :param yml_config:
        """
        with open(yml_config, 'r') as ymlfile:
            cfg = yaml.safe_load(ymlfile)['email']
            self.host = cfg['host']
            self.port = cfg['port']
            self.username = cfg['username']
            self.password = cfg['password']
            self.recipients = cfg['recipients']


class Email:
    """
    Notification Email server.
    """
    def __init__(self, config: EmailConfig):
        """
        Initial Email for notification.
        :param config: Email Authentication info
        """
        self._config = config

    def send(self, subject: str, body: str):
        """
        Send message to recipients.
        :param subject: message subject
        :param body: message body
        :return:
        """
        try:
            smtp_server = smtplib.SMTP(host=self._config.host, port=self._config.port)
            smtp_server.starttls()
            smtp_server.login(self._config.username, self._config.password)

            msg = f'Subject: {subject}\nFrom: {self._config.username}\nTo: {self._config.recipients}\n\n{body}'
            smtp_server.sendmail(self._config.username, self._config.recipients, msg)
        except smtplib.SMTPException as e:
            print(str(e))
#####
config.yml
---
email:
  host: 'smtp.gmail.com'
  port: 587
  username: 'user@gmail.com'
  password: 'xxx'
  recipients:
    - 'user1@gmail.com'
    - 'user2@gmail.com'
#####
how to use it:
Email(EmailConfig()).send(subject="ALERT: SOME NOTIFIER ALERT",
                          body=f"Exception for anything, MESSAGE: {ex}")
```
6. get package version command
```
pip freeze | grep <package_name>
```
