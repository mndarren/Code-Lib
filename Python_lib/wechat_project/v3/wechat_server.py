#!/usr/bin/env python3

# createdby: Darren Zhao Xie
# module: wechat_server
# purpose: litsen connection, receive msg and send it to friends by wechat
# params: input data structure: {'msgs': [], 'friends': []}
# PIL: python3 -m pip install Pillow

from __future__ import unicode_literals
from wxpy import *
import socket
import json
import time
import random

HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
PORT = 64422        # Port to listen on (non-privileged ports are > 1023)
BUFFER_SIZE = 1024
OWNER = 'Darren Z. Xie'
friends = ['Merry']
# bot = Bot()  # in Windows
# linux执行登陆请调用下面的这句
bot = Bot(console_qr=2,cache_path="botoo.pkl")
# add myself as a friend
# bot.self.add()
# bot.self.accept()


def send_msgs(msgs, friends=friends, *, weather_image=False):
    try:
        for friend in friends:
            # 你朋友的微信名称，不是备注，也不是微信帐号。
            my_friend = bot.friends().search(friend)[0]
            # sleep time is for avoiding fire the WeChat permission trigger
            ran_int = random.randint(0, 5)
            time.sleep(ran_int)
            for msg in msgs:
                my_friend.send(msg)
                ran_int = random.randint(0, 5)
                time.sleep(ran_int)
        # for msg in msgs:
        #   bot.self.send(msg)
        # bot.self.send(u"以上消息发送给了" + json.dumps(friends))
    except Exception as e:
        # 你的微信名称，不是微信帐号。
        my_friend = bot.friends().search(OWNER)[0]
        my_friend.send(u"今天消息发送失败了")
        # bot.self.send(u"今天消息发送失败了")
        print(e)


def run_server():
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((HOST, PORT))
        s.listen()
        while True:
            conn, addr = s.accept()
            with conn:
                print('Connected by', addr)
                data = conn.recv(BUFFER_SIZE)
                if data:
                    # convert bytes to str then to dict
                    data_dict = json.loads(data.decode('utf-8'))
                    msgs = data_dict['msgs']
                    print(msgs)
                    send_msgs(**data_dict)
                    conn.sendall((json.dumps(msgs)).encode('utf-8'))


if __name__ == "__main__":
    run_server()

