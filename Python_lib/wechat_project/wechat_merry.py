#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_merry
"""
from wechat.wechat_client import WechatClient
from wechat.constants import Constants

client = WechatClient(msg=Constants.night_msg)
client.run()

