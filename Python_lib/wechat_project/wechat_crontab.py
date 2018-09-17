#!/usr/bin/env python

# Developer: Darren Zhao Xie
# Module: wechat_server
# Purpose: schedule run script every 1st of each month

from crontab import CronTab

# tab = """30 9 1 * * python3 wechat_client.py"""

cron = CronTab(user='dxie')
cron_job = cron.new(command='python3 wechat_client.py', comment='wechat_schedule')
cron_job.minute.on(0)
cron_job.hour.on(0)
cron_job.day.on(1)
cron_job.month.during(1,12)
cron_job.enable()
cron.write()
print(cron_job.is_valid())
