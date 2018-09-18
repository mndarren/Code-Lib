#!/usr/bin/env python3

# Developer: Darren Zhao Xie
# Module: wechat_server

from __future__ import unicode_literals
from wxpy import *
import requests
import socket
import json

HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
PORT = 64422        # Port to listen on (non-privileged ports are > 1023)
BUFFER_SIZE = 1024
OWNER = 'Darren Z. Xie'
friends = ['Merry']
# bot = Bot()  # in Windows
# linux执行登陆请调用下面的这句
bot = Bot(console_qr=2,cache_path="botoo.pkl")


def get_news():
    """获取金山词霸每日一句，英文和翻译"""
    url = "http://open.iciba.com/dsapi/"
    r = requests.get(url)
    content = r.json()['content']
    note = r.json()['note']
    return content, note


def send_news(msg, friends=friends, need_news=True):
    try:
        for friend in friends:
            # 你朋友的微信名称，不是备注，也不是微信帐号。
            my_friend = bot.friends().search(friend)[0]
            if need_news:
                contents = get_news()
                my_friend.send(contents[0])
                my_friend.send(contents[1])
            my_friend.send(msg)
    except Exception as e:
        # 你的微信名称，不是微信帐号。
        my_friend = bot.friends().search(OWNER)[0]
        my_friend.send(u"今天消息发送失败了")
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
                    msg = data_dict['msg']
                    print(msg)
                    send_news(**data_dict)
                    conn.sendall(msg.encode())


if __name__ == "__main__":
    run_server()

