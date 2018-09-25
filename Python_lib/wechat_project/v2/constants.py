#!/usr/bin/env python3

# createdby: Darren Zhao Xie on 9/23/2018
# module: constants.py

from __future__ import unicode_literals

######################## Server Configuration ############################
HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
PORT = 64422        # Port to listen on (non-privileged ports are > 1023)
BUFFER_SIZE = 1024
OWNER = 'Darren Z. Xie'

MSG_HOST = '127.0.0.1'  # Standard loopback interface address (localhost)
MSG_PORT = 64422        # Port to listen on (non-privileged ports are > 1023)
MSG_BUFFER_SIZE = 1024

######################### Friends Categories #################################################
common = ['Kellen', 'Even\'s Dad', 'Mike Xie', 'MOMO', 'Fenlan', 'Liu Doctor', 'ZhangLaoshi']
jiaozhou_friends = ['Dongyou', 'LiqiangSun', 'WeiSun']
relatives = ['ChongXie', 'WeiXie', 'Shang', 'Merry', 'Monica', 'Haixia', 'Chunlin', 'Angang', 'Huifeng',
			 'Huixian', 'YulingSun', 'FengWang', 'Laolao', 'Laoye', 'HangLi', 'BoLiu',
			 'DuoZhang', 'WD', 'WJ', 'ChuangXie', 'Jianmiao', 'cici', 'YonghuaYang', 'Yaoyao',
			 'Yisa', 'XuZhang']  # total 26
scsu_friends = ['Chen, Mo', 'ChengLuo', 'Hu,Jie', 'Machine', 'Shengti', 'RuiZhao', 
				'ClashOfClan', 'Dejian', 'HangYin', 'Jerry', 'Jim Chen',  'ShengQian',
				'QianWang', 'ChaoXiong', 'Weiwei', 'MengqingHe', 'Zhao Ji']  # total 17
sdcy_friends = ['Cui,', 'Changying', 'Hu,Hang', 'LW', 'Xiangwen', 'ChunxiaTan', 'LiWang', 'Xiangjun',
				'Yabin', 'XinhuaYang', 'Yin Wen', 'Changsong', 'LongYu', 'knospe','Hongbo']  # total 15
g97 = ['WeiWang', 'RenzhongHu', 'Xiangzhen', 'zhangmeng']
tccdc_friends = ['Che..', 'QianXu', 'JingLi', 'Taoyuan', 'JianlingYuan', 'Rosemary']
classmates = jiaozhou_friends + sdcy_friends + g97

phone_friends = ['Shang']
weather_friends = ['Chen, Mo', 'Machine']

############################ Messages #########################################################
scsu_moon_msg = u'值此中秋佳节到来之际，祝您及全家节日快乐！身体健康！万事如意！ Happy Moon Cake Day!'
relatives_moon_msg = u'值此中秋佳节到来之际，祝您及咱们全家节日快乐！身体健康！万事如意！'
classmates_moon_msg = u'值此中秋佳节到来之际，谢昭祝老同学及全家节日快乐！身体健康！万事如意！'
common_moon_msg = u'值此中秋佳节到来之际，祝您及全家节日快乐！身体健康！万事如意！'

phone_msg = u'友情提示： 该交话费了！:)'
night_msg = u'晚安 :)'
merry_bth_msg = u'感谢上苍在今天给了我一个特别的礼物,就是你。长长的人生旅程，有你相伴是我一生的幸福。老婆,祝你生日快乐! Love you for ever!!!'
