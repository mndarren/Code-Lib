#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_shang
"""
from wechat_client import WechatClient
from constants import *

client = WechatClient(friends=phone_friends,
                      msg=phone_msg,
                      need_news=False)
client.run()

