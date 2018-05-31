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
	Alt + F1    # open search
	Alt + F2    # open file explorer
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
	1) sudo apt-get install xrdp -y  # install xrdp
	2) echo mate-session> ~/.xsession
	3) sudo apt-get install mate-core
	5) test using sesman-XVnc by RDP of Windows
	```
6. Install SSH on Ubuntu
	```
	sudo apt-get install openssh-server
	sudo service ssh status
	sudo nano /etc/ssh/sshd_config
	```
7. Install pip
	```
	sudo apt-get install python-pip python-dev build-essential
	sudo pip install --upgrade pip
	sudo pip install --upgrade virtualenv
	```
8. Install python3.6
	```
	sudo add-apt-repository ppa:deadsnakes/ppa   # OR ppa:jonathonf/python-3.6
	sudo apt-get update
	sudo apt-get install python3.6
	```
9. 