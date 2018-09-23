#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_merry
"""
from wechat_client import WechatClient
from constants import night_msg

client = WechatClient(msg=night_msg)
client.run()

