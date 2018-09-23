#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_client
Purpose: I'm a Messager. Tell me messages and related friends, 
         I'll send messages to your friends.
"""
from __future__ import unicode_literals
import socket
import json
from constants import *


class WechatClient:
    """docstring for WechatClient"""
    def __init__(self, msgs, friends=None):
        self.json_msg = {'msgs': msgs}
        if friends:
            self.json_msg['friends'] = friends

    def run(self):
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((HOST, PORT))
            # convert dict to str and then convert str to bytes
            s.sendall((json.dumps(self.json_msg)).encode('utf-8'))
            data = s.recv(BUFFER_SIZE)

        print('Received', repr(json.loads(data.decode('utf-8'))))

