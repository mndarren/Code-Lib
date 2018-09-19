#!/usr/bin/env python

# Developer: Darren Zhao Xie
# Module: wechat_crontab
# Purpose: schedule run script every 1st of each month
# /var/spool/cron/crontabs  as root since -T sticky for customer schedule
# /etc/crontab and /etc/cron.d/jobs for system schedule
# sudo service cron reload  # reload cron after having updated

from crontab import CronTab

tab = """30 9 1,18 * * python3 wechat_client.py"""
cron = CronTab(tab=tab, user='dxie')
cron.write()
