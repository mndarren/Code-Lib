#!/usr/bin/env python3


#Developer: Darren Zhao Xie
#Module: wechat_server

from __future__ import unicode_literals
from threading import Timer
from wxpy import *
import requests
import random
import socket

HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
PORT = 64422        # Port to listen on (non-privileged ports are > 1023)
COUNT = 3
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


def send_news(msg, friends=['Merry']):
    try:
        for friend in friends:
            contents = get_news()
            # 你朋友的微信名称，不是备注，也不是微信帐号。
            my_friend = bot.friends().search(friend)[0]
            my_friend.send(contents[0])
            my_friend.send(contents[1])
            my_friend.send(msg)
    except Exception as e:
        # 你的微信名称，不是微信帐号。
        my_friend = bot.friends().search('Darren Z. Xie')[0]
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
                data = conn.recv(1024)
                if data:
                    send_news(data.decode('utf-8'))
                    conn.sendall(data)


if __name__ == "__main__":
    run_server()
