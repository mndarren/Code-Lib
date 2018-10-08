#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_merry
"""
from wechat_client import WechatClient
from constants import night_msg
from public_msg_server import get_news, get_weather


weather = get_weather()
weather[0]["weather_image"] = True
print(weather)
msgs = get_news() + weather + [night_msg]
# print(msgs)
client = WechatClient(msgs=msgs)
client.run()

