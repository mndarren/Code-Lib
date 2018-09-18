#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_shang
"""
from wechat.wechat_client import WechatClient
from wechat.constants import Constants

client = WechatClient(friends=Constants.phone_friends,
                      msg=Constants.phone_msg,
                      need_news=False)
client.run()

