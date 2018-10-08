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
    1) Desktop Sharing from search (Alt+F1) # gnome-control-center sharing
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
    sudo apt install python3-pip
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
11. How to set up mount
    ```
    sudo mkdir /extracts
    sudo chown dxie:dxie -R /extracts
    sudo mkdir /extracts/any
    sudo nano /etc/fstab  # append # //network/info      /extracts/any cifs    credentials=/extracts/crutch.res,noauto,users      0       0
    sudo nano /extracts/crutsh.res  # add username= password= domain= in 3 separated lines
    mount /extracts/any
    ```
12. How to set up DBVisualizer
    ```
    # Download DBVisualizer rpm version first
    sudo rpm -i download_file
    dbvis
    ```
13. tmux
    ```
    tmux
    tmux ls
    tmux attach -t 0
    tmux new -s database  # session name: database
    tmux rename-session -t 0 database  # rename session
    tmux attach -t database  # attach session
    tmux detach  # return basic terminal
    tmux kill-sever  # destroy all sessions and kill all processes
    ```
14. install grails
    ```
    curl -s https://get.sdkman.io | bash
    source "$HOME/.sdkman/bin/sdkman-init.sh"
    sdk install grails
    grails -version
    grails create-app com.vogella.grails.guestbook  # @ grails-app/conf/application.yml/ to speccify a default package
    grails list-profiles  # list all profiles
    grails create-app myApp -profile rest-api
    grails create-app myApp -profile org.grails.profiles:react:1.0.2
    grails profile-info plugin  # get detaied info about a profile
    grailsw run-app  # without installed grails
    grails run-app   # which installed grails
    http://localhost:8080/com.vogella.grails.guestbook
    ```
15. crontab  # from the Greek word "Chronos", which means "time", tab - table
    ```
    ┌───────────── minute (0 - 59)
    │ ┌───────────── hour (0 - 23) 
    │ │ ┌───────────── day of month (1 - 31)
    │ │ │ ┌───────────── month (1 - 12)
    │ │ │ │ ┌───────────── day of week (0 - 6) (Sunday to Saturday;
    │ │ │ │ │                           7 is also Sunday on some systems)
    │ │ │ │ │
    │ │ │ │ │
    * * * * *  command to execute
    0 16 1,10,22 * * tells cron to run a task at 4 PM (which is the 16th hour) on the 1st, 10th and 22nd day of every month.
    @yearly, @annually Run once a year at midnight of January 1 (0 0 1 1 *)
    @monthly Run once a month, at midnight of the first day of the month (0 0 1 * *)
    @weekly Run once a week at midnight of Sunday (0 0 * * 0)
    @daily Run once a day at midnight (0 0 * * *)
    @hourly Run at the beginning of every hour (0 * * * *)
    @reboot Run once at startup
    /var/spool/cron/crontabs  as root since -T sticky for customer schedule
    /etc/crontab and /etc/cron.d/jobs for system schedule
    pip install python-crontab
    sudo service cron reload  # reload cron after having updated
    crontab -e  # create a crontab file
    crontab -u username -e
    crontab -l  # list schedule jobs
    crontab -r  # remove current cron
    crontab /path/to/the/file/containing/cronjobs.txt
    echo ALL > /etc/cron.deny  # if created /etc/cron.allow file, don't need /etc/cron.deny file
    ps aux | grep crond  # daemon is running
    ```
16. remove and reinstall mysql-server
```
sudo apt-get remove --purge mysql* -y
sudo apt-get purge mysql*
sudo apt-get autoremove -y
sudo apt-get autoclean
sudo apt-get remove dbconfig-mysql
sudo apt-get dist-upgrade
sudo apt-get install mysql-server -y
```
17. How to setup time zone to UTC
```
sudo dpkg-reconfigure tzdata
# select Etc or 'None of the above', in the 2nd list choose UTC
```
18. How to setup OpenVPN server on Ubuntu 16.04
```
# Find and note down your public IP address
# Download openvpn-install.sh script
wget https://git.io/vpn -O openvpn-install.sh
sudo bash openvpn-install.sh

Run openvpn-install.sh to install OpenVPN server
Connect an OpenVPN server using IOS/Android/Linux/Windows client
Verify your connectivity
```

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
    deb http://us.archive.ubuntu.com/ubuntu/ maverick multiverse    restricted universe main
    deb-src http://us.archive.ubuntu.com/ubuntu/ maverick multiverse    restricted universe main #Added by software-properties
    ```
3. List all users `cut -d: -f1 /etc/passws`
4. 

# Cool Commands 4 CentOS7
================================================
1. How to use scp to transfer file from Ubuntu to CentOS VM of VirtualBox
    ```
    # situation: firewall never let 22(SSH) port go thru, Guest plugin was installed failed
    1) re-setup dynamic IP
       $ dhclient -r
       $ dhclient
    2) Network setting (VirtualBox) -> NAT & Port Forwarding
       Host IP   Host Port   Guest IP   Guest Port
       127.0.0.1  2222       127.0.0.1   22
    3) scp -P 2222 source_file dxie@127.0.0.1:/home/dxie
    ```

