# Cool Commands
==================================
1. search a file from Linux
	```
	find / -iname pg_hba.conf
	```
2. Install VMware tools on Ubuntu
	```
	1) Guest -> install tools
	2) sudo mkdir /mnt/cdrom
	3) sudo mount /dev/cdrom /mnt/cdrom or sudo mount /dev/sr0 /mnt/cdrom
	4) ls /mnt/cdrom
	5) tar xzvf /mnt/cdrom/VMwareTools-x.x.x-xxxx.tar.gz -C /tmp/
	6) cd /tmp/vmware-tools-distrib/
	7) sudo ./vmware-install.pl -d
	8) sudo reboot
	```
3. Open file explorer from terminal
	```
	nautilus --browser ~/some/directory
	```
4. 