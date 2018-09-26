#!/usr/bin/env python3

"""
createdby: Darren Zhao Xie on 9/20/2018
Module: public_msg_client.py
"""
from __future__ import unicode_literals
import socket
import json
from constants import *


class WechatClient:
    """docstring for WechatClient"""
    def __init__(self, msg):
        self.json_msg = msg

    def run(self):
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((HOST, PORT))
            # convert dict to str and then convert str to bytes
            s.sendall((json.dumps(self.json_msg)).encode())
            data = s.recv(BUFFER_SIZE)
        result = data.decode('utf-8')
        print('Received', repr(result))
        return result

