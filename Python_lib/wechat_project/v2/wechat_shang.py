#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_shang
"""
from wechat_client import WechatClient
from constants import *
from public_msg_server import get_news, get_weather

msgs = get_weather() + [phone_msg]
client = WechatClient(friends=phone_friends,
                      msgs=msgs)
client.run()
