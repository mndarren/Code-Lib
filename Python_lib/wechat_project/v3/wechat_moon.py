#!/usr/bin/env python3

"""
createdby: Darren Zhao Xie on 9/23/2018
Module: wechat_moon.py
"""
from wechat_client import WechatClient
from constants import *
import time

note = u'虽然此短信是我编写的小程序群发的，既然您能收到，说明我心里有您，就不要在乎群发之事了！谢谢您！中秋快乐！'

client = WechatClient(friends=scsu_friends,
                      msgs=[scsu_moon_msg] + [note])
client.run()

time.sleep(60)

client = WechatClient(friends=relatives,
                      msgs=[relatives_moon_msg] + [note])
client.run()

time.sleep(120)

client = WechatClient(friends=classmates,
                      msgs=[classmates_moon_msg] + [note])
client.run()

time.sleep(180)

client = WechatClient(friends=common,
                      msgs=[common_moon_msg] + [note])
client.run()

time.sleep(180)

client = WechatClient(friends=tccdc_friends,
                      msgs=[common_moon_msg] + [note])
client.run()