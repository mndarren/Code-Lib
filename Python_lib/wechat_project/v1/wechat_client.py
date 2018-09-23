#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_client
"""
from __future__ import unicode_literals
import socket
import json
from constants import *


class WechatClient:
    """docstring for WechatClient"""
    def __init__(self, msg, friends=None, need_news=True):
        self.json_msg = {'msg': msg}
        if friends:
            self.json_msg['friends'] = friends
        if not need_news:
            self.json_msg['need_news'] = need_news

    def run(self):
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((HOST, PORT))
            # convert dict to str and then convert str to bytes
            s.sendall((json.dumps(self.json_msg)).encode())
            data = s.recv(BUFFER_SIZE)

        print('Received', repr(data.decode('utf-8')))

