#!/usr/bin/python
from scapy.all import *

vic = ['192.168.1.121','192.168.192.130']
pl = ['Payload0','Payload1','Payload2','Payload3','Payload4']
#pl = "Payload0Payload1Payload2Payload3Payload4"
arrayLen = len(pl)

for i in range(len(vic)):
	f = 1
	print '\n'
	ip = IP(dst=vic[i],proto=1,id=1117,len=42,flags=f)
	icmp = ICMP(type=8,code=0,chksum=0xe517)
	pkt = ip/icmp/pl[0]
	print hexdump(str(pkt))	
	answer,unanswer = sr(pkt,timeout=1)
	answer.summary()
	unanswer.summary()

	for j in xrange(1,arrayLen):
		if j == arrayLen-1:
			f = 0
		ip = IP(dst=vic[i],proto=1,id=1117,len=42,flags=f,frag=j)
		pkt = ip/pl[j]
		print hexdump(str(pkt))
		answer,unanswer = sr(pkt,timeout=1)
		answer.summary()
		unanswer.summary()
