#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_merry
"""
from wechat_client import WechatClient
from constants import night_msg
from public_msg_server import get_news, get_weather

msgs = get_news() + get_weather() + [night_msg]
# print(msgs)
client = WechatClient(msgs=msgs)
client.run()

