#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_shang
"""
from wechat_client import WechatClient
from constants import *

client = WechatClient(friends=scsu_friends,
                      msg=scsu_moon_msg,
                      need_news=False)
client.run()

client = WechatClient(friends=relatives,
                      msg=relatives_moon_msg,
                      need_news=False)
client.run()

client = WechatClient(friends=classmates,
                      msg=classmates_moon_msg,
                      need_news=False)
client.run()

client = WechatClient(friends=common,
                      msg=common_moon_msg,
                      need_news=False)
client.run()
