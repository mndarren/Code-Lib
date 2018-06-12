# Cool Commands 4 Ubuntu
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
	7) sudo apt-get install net-tools  # for installing ifconfig
	8) sudo ./vmware-install.pl
	9) sudo reboot
	10) vmware-toolbox-cmd -v
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
	2) sudo systemctl enable xrdp
	3) echo mate-session> ~/.xsession
	4) sudo apt-get install mate-core -y
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
9. logout `who -u`  `sudo kill "pid"`
10. update version from 16 to 18
	```
	sudo apt update && sudo apt dist-upgrade && sudo apt autoremove
	sudo apt-get install update-manager-core
	sudo nano /etc/update-manager/release-upgrades  # Prompt=lts
	sudo do-release-upgrade -d  # -y
	reboot
	```
11. 

# Cool Commands 4 Redhat7
=====================================
1. How to change IP addr
	```
	1) nmcli dev status
	2) nmcli connection show
	3) nmcli con show ens160
	4) add a new network card if necessary
	5) nmcli con add con-name ens224 type ethernet \
		ifname ens224 ip4 10.31.31.222 gw4 10.31.0.1
	6) nmcli dev status
	7) nmcli con mod ens224 ipv4.addresses "10.31.31.223/16" ipv4.dns "10.10.4.6 10.10.4.7"
	8) nmcli con up ens224
	9) ifconfig ens224
	10) nmcli con del ens224   # delete iface
	```
2. Disable the virbr0 permanently `virsh net-autostart default --disable`<br/>
	OR `systemctl disable libvirtd.service` OR `virsh net-destroy default`
	Start virbr0 `virsh net-start default`
	Edit virbr0 `virsh net-edit default`

# Cool Commands 4 Debian8
================================================
1. Restart network `/etc/init.d/networking restart`
2. Set up Source list 
	```
	nano /etc/apt/source.list
	deb http://archive.canonical.com/ubuntu maverick partner
	deb-src http://archive.canonical.com/ubuntu maverick partner
	## This software is not part of Ubuntu, but is offered by third-party
	## developers who want to ship their latest software.
	deb http://extras.ubuntu.com/ubuntu maverick main
	deb-src http://extras.ubuntu.com/ubuntu maverick main
	deb http://us.archive.ubuntu.com/ubuntu/ maverick multiverse 	restricted universe main
	deb-src http://us.archive.ubuntu.com/ubuntu/ maverick multiverse 	restricted universe main #Added by software-properties
	```
3. List all users `cut -d: -f1 /etc/passws`
4. 