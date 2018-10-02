#!/usr/bin/env python3

# Developer: Darren Zhao Xie
# Module: public_msg.py

from __future__ import unicode_literals
from wxpy import *
from threading import Timer
import requests
import socket
import json
from constants import *

news = []
weather = []


def get_news():
    """获取金山词霸每日一句，英文和翻译"""
    url = "http://open.iciba.com/dsapi/"
    r = requests.get(url)
    news = [r.json()['content']] + [r.json()['note']]
    return news


def get_weather():
    """{
       "code": "12",
       "date": "20 Sep 2018",
       "day": "Thu",
       "high": "68",
       "low": "60",
       "text": "Rain"
      }"""
    url = "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22minneapolis%2C%20mn%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys"
    r = requests.get(url)
    three_days_weather = r.json()['query']['results']['channel']['item']['forecast'][:3]  # 3 days
    tomorrow = three_days_weather[1]
    today = three_days_weather[0]
    weather = [{"Tomorrow weather": tomorrow['text'], "High": tomorrow['high'], "Low": tomorrow['low']}]
    weather.append('test thing!')
    if int(today['high']) - int(tomorrow['high']) > 10 or int(today['low']) - int(tomorrow['low']) > 10:
        weather.append("Coldest Alert: Tomorrow is colder > 10 degree than today!")
    elif int(today['high']) - int(tomorrow['high']) > 5 or int(today['low']) - int(tomorrow['low']) > 5:
        weather.append("Colder Alert: Tomorrow is colder > 5 degree than today!")
    return weather

# pp = pprint.PrettyPrinter(indent=4)
# pp.pprint(get_weather())
print(get_news())
print(get_weather())

# def auto_update_msg():
#     get_news()
#     get_weather()
#     Timer(86400, auto_update_msg).start()


# def run_server():
#     auto_update_msg()
#     with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
#         s.bind((MSG_HOST, MSG_PORT))
#         s.listen()
#         while True:
#             conn, addr = s.accept()
#             with conn:
#                 print('Connected by', addr)
#                 data = conn.recv(MSG_BUFFER_SIZE)
#                 if data:
#                     # convert bytes to str then to dict
#                     data_list = json.loads(data.decode('utf-8'))
#                     out_data = {}
#                     if 'news' in data_list:
#                         out_data['news'] = news
#                     if 'weather' in data_list:
#                         out_data['weather'] = weather
#                     print(data_list)
#                     conn.sendall(out_data.encode())


# if __name__ == "__main__":
#     run_server()
