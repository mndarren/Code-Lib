#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_merry
"""
from wechat_client import WechatClient
from constants import merry_bth_msg

client = WechatClient(msg=merry_bth_msg)
client.run()
