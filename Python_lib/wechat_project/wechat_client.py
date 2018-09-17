#!/usr/bin/env python3

"""
Developer: Darren Zhao Xie
Module: wechat_client
"""
import socket

HOST = '127.0.0.1'  # The server's hostname or IP address
PORT = 64422        # The port used by the server
msg = b"It's time to pay your phone fee!"

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))
    s.sendall(msg)
    data = s.recv(1024)

print('Received', repr(data.decode('utf-8')))
