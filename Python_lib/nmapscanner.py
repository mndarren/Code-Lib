#!/usr/bin/python 
#nmap scanner built by Darren Xie
import nmap
import os, sys
import optparse

def nmapScan(targetHosts, targetPorts):
	try:
		scanner = nmap.PortScanner()
		scanner.scan(targetHosts, targetPorts)#get data
		#print out result data
		for targetHost in scanner.all_hosts():
			print('\n----------------------------------------------------')
			print('Host : %s (%s)\tstate : %s' % (targetHost, scanner[targetHost].hostname(),scanner[targetHost].state()))
			for proto in scanner[targetHost].all_protocols():
				print('-----------------')
				print('Protocol: ' + proto)
				if scanner[targetHost].state() == 'up':
					for targetPort in scanner[targetHost][proto]:
						portState = scanner[targetHost][proto][int(targetPort)]['state']
						portName = scanner[targetHost][proto][int(targetPort)]['name']
						print( 'Port: %s \tstate: %s  (%s)' % (str(targetPort),portState, portName))
	except Exception as e:
		print('Something bad happened during the scan: ' + str(e))
	
def main():
	parser = optparse.OptionParser("usage: %prog [-H hosts] [-p ports]")
	parser.add_option("-H", "--host", dest="hostnames",default="127.0.0.1", type="string",help="specify hostname to run on")
	parser.add_option("-p", "--port", dest="portnums", default='80',type="string", help="port number to run on")
	options, args = parser.parse_args()
	print ("length of args = " + str(len(sys.argv)))
	hosts = options.hostnames
	ports = str(options.portnums)
	print ("The hosts are: %s\nThe ports are: %s" %(hosts, ports))
	nmapScan(hosts, ports)
	
if __name__ == '__main__':
	main()
	
