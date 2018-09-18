#!/usr/bin/env python

# Developer: Darren Zhao Xie
# Module: wechat_server
# Purpose: schedule run script every 1st of each month
# /var/spool/cron/crontabs  as root since -T sticky

from crontab import CronTab

tab = """30 9 1,18 * * python3 wechat_client.py"""
cron = CronTab(tab=tab, user='dxie')
cron.write()
