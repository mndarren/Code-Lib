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
	7) sudo ./vmware-install.pl
	8) sudo reboot
	9) vmware-toolbox-cmd -v
	```
3. Open file explorer from terminal
	```
	nautilus --browser ~/some/directory
	```
4. Remote Desktop Ubuntu 16.04
	```
	1) Desktop Sharing from search
	2) Check 'Allow other users to view your desktop'
			 'Allow other users to control your desktop'
			 'Require the user to enter this password'
			 'Only when someone is connnected'
	3) $gsettings set org.gnome.Vino require-encryption false
	4) Download VNC on your Windows, Open VNC and login with IP and password.
	```
5. Remote Desktop Ubuntu 16.04(another way)
	```
	1) Download Tigervnc deb package (link: https://github.com/mndarren/Code-Lib/tree/master/bash_lib/resource)
	2) cd Download
	   unzip tigervncserver_1.6.80-4_amd64.zip
	   sudo dpkg -i tigervncserver_1.6.80-4_amd64.deb
	   sudo apt-get -f install  # make sure install all dependencies
	   sudo dpkg -i tigervncserver_1.6.80-4_amd64.deb
	3) sudo apt-get install xrdp -y  # install xrdp
	   echo unity>~/.xsession  # OR $sudo sed -i.bak '/fi/a #xrdp multi-users \n unity \n' /etc/xrdp/startwm.sh
	4) run script (resource/set_kb.bash)
	5) test using sesman-XVnc
	```
6. Install SSH on Ubuntu
	```
	sudo apt-get install openssh-server
	sudo service ssh status
	sudo nano /etc/ssh/sshd_config
	```
7. 