#!/usr/bin/env python3

# Developer: Darren Zhao Xie
# Module: constants

from __future__ import unicode_literals


class Constants:
    """constants for wechat server"""
    HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
    PORT = 64422        # Port to listen on (non-privileged ports are > 1023)
    BUFFER_SIZE = 1024
    OWNER = 'Darren Z. Xie'

    # msg = 'It\'s time to pay your phone fee!'
    phone_msg = u'友情提示： 该交话费了！:)'
    phone_friends = ['Shang']
    night_msg = u'晚安 :)'

