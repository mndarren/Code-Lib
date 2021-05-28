# Linux Commands
===========================  
1. Git Commands
```Git
git remote update; git reset --hard origin/integration
git clone <repo_url> local_dir_name                    # can rename the git repo local folder
git reset --hard HEAD                                  # revert file change to origin
git clean -ndx                                         # n: not really remove; d: recurse; x: don't use gitignore
git clean -fdx                                         # f: force to delete
git lfs fetch --all                                    # fetch all binary files
git lfs pull --exclude= --include=ubuntu-iso/ubuntu-20.04.1-legacy-server-amd64.iso  # just pull one .iso
git lfs ls-files                                       # show all lfs files, not work
cat .git/config                                        # look at git config file
```
2. `losetup  -a`                                       # will show all /dev/loop
3. `less file1.txt`                                    # view a file one screen at a time
4. `dpkg -l | grep git-lfs`                            # show git-lfs version info
5. `find . -name ubuntu-20\*.is\* -exec ls -l {} \;`   # search ubuntu version 20 .iso files
6. USB Drive Action
```USB
dmesg | tail                                           # list all USB ports
cat /proc/partitions                                   # check out the partitions of Linux
sudo mount /dev/sdb1 /mnt/                             # mount sdb1 to /mnt/ dir
cp /mnt/ubuntu-20.04.2-live-server-amd64.iso holtr-test/ubuntu-iso/ubuntu-20.04.1-legacy-server-amd64.iso
umount /mnt
sudo mount /dev/sdb1 /mnt/
rsync -aHPv ubuntu-20.04.1-legacy-server-amd64.iso  /mnt/  # override xx.iso to USB drive
umount /mnt
df                                                     # show all devices
```
7. Setup sudoer, not working
```sudoer
sudo su -
cd /etc/sudoers.d/
visudo mis_tester
# content
mis_tester      ALL=(ALL) NOPASSWD: ALL
logout
```
8. Install Guest Addition
```
# Insert Guest Additions DC image
sudo mount /dev/cdrom /media
cd /media
sudo ./VBoxLinuxAdditions.run
sudo reboot
```
10. Check if firefox is running `ps aux | grep firefox | grep -v grep`
11. Check config file by keyword `grep -i x11 /etc/ssh/sshd_config`
12. Solve the IP v4 cannot find issue (release and renew IP address
```
sudo dhclient -r eth0
sudo dhclient eth0
```
13. Network restart
```
# for networkd /etc/netplan/01-netcfg.yaml
sudo netplan apply
sudo ip link set eth0 down
sudo ip link set eth0 up

# for NetworkManager
sudo nmcli networking off
sudo nmcli networking on

# layer1
ip link show
ip -s link show eth0   # statistic
# layer2: data link (ARP table)
ip neighbor show
ip neighbor delete 192.168.122.170 dev eth0
# layer3: internet
ip -br address show
ping -c 1 www.google.com
traceroute www.google.com
ip route show
nslookup www.google.com
# layer4: transport
ss -tunlp4
telnet database.example.com 3306
nc 192.168.122.1 -u 80
```
