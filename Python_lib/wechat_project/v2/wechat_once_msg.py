#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_merry
"""
from wechat_client import WechatClient
from __future__ import unicode_literals

once_msg = u'周六别忘了要滑冰发票'

client = WechatClient(msg=once_msg)
client.run()
