#!/usr/bin/env python3

"""
createdby: Darren Zhao Xie on 9/23/2018
module: wecha_weather.py
"""
from wechat_client import WechatClient
from constants import *
from public_msg_server import get_news, get_weather

msgs = get_weather() + [night_msg]
client = WechatClient(friends=weather_friends,
                      msgs=msgs)
client.run()
