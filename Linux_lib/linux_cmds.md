# Linux Commands
===========================  
1. Git Commands
```Git
git remote update; git reset --hard origin/integration
git config --global credential.helper store
git clone <repo_url> local_dir_name                    # can rename the git repo local folder
git reset --hard HEAD                                  # revert file change to origin
git clean -ndx                                         # n: not really remove; d: recurse; x: don't use gitignore
git clean -fdx                                         # f: force to delete
git lfs fetch --all                                    # fetch all binary files
git lfs pull --exclude= --include=ubuntu-iso/ubuntu-20.04.1-legacy-server-amd64.iso  # just pull one .iso
git lfs ls-files                                       # show all lfs files, not work
cat .git/config                                        # look at git config file
git checkout -f										   # discard all local changes
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
systemctl restart systemd-networkd
sudo ip link set eth0 down
sudo ip link set eth0 up
journalctl -u systemd-networkd.service
# disable ipv6
sudo sysctl -w net.ipv6.conf.all.disable_ipv6=1
sudo sysctl -w net.ipv6.conf.default.disable_ipv6=1
sysctl -a | grep ipv6.*disable
# Check
cd /run/systemd/netif/leases/
ls
cat /run/systemd/network/10-netplan-eth0.network
journalctl -u systemd-networkd | tail -20

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
ping -c 1 -4 www.google.com
traceroute www.google.com
ip route show
nslookup www.google.com
# layer4: transport
ss -tunlp4
telnet database.example.com 3306
nc 192.168.122.1 -u 80
```
14. tcpdump `tcpdump -i eth0 port 67 or port 68`
15. Docker commands
```
docker build -f DaikinSystemManagerWebService/Dockerfile.focal-5.0 .
docker build -f DaikinSystemManagerWebService/Dockerfile.focal-5.0 . | tee /tmp/build.log
docker build -f DaikinSystemManagerWebService/Dockerfile.focal-5.0  -t sm:latest . | tee /tmp/build.log
docker exec fcaf1efa2e74 ls -la /app
docker run -ti --entrypoint /bin/bash --name holtr-debugging <xxxxxxcontainerId>
docker run -p 1880:1880 --device /dev/ttyUSB0:/dev/ttyUSB0 holtr:latest
docker run --rm -v /MISystem:/MISystem -p 1880:1880 --device /dev/ttyUSB0:/dev/ttyUSB0 sm:latest
docker run -ti --rm --device /dev/ttyACM0:/dev/ttyACM0 ubuntu:focal
docker run --rm -v /MISystem:/MISystem --device /dev/ttyUSB0:/dev/ttyUSB0 --net=host sm:latest

docker image prune
docker images -a
docker rm <containerID/name>
docker stop <containerID/name>
docker ps -a
docker rmi <imageID/name>

docker network inspect bridge
docker exec -it 04f05533f606 bash
```
16. Solve merge conflict
```
git pull
git checkout -f
git reset --hard <commit id>
```
17. Azure commands
```
# Install Azure CLI before running the commands https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli
az --version
az upgrade
az extension add --name azure-iot
az extension add --name azure-devops
az --version
az extension update --name azure-iot
az login
az login --use-device-code xxxx
az account list
az account set --subscription <account_id>
az iot hub list
az iot edge deployment list -n <hub_name>
az iot edge deployment delete -n <hub_name> -d <developer_name>
az iot edge deployment create -n <hub_name> -d <developer_name> --content path\to\deployment.amd64.json --target-condition "tags.user='<developer_name>'"
# Useful commands (global args: --help, --output, --query, --verbose, --debug)
az find secret
az network nsg --help
az interactive
# 7 commands groups
az group
az vm
az storage account
az keyvault
az webapp
az sql server
az cosmosdb
# AZ config
az config set extension.use_dynamic_install=yes_without_prompt
az devops configure --defaults organization=https://dev.azure.com/MyOrganization/
az devops configure --defaults project=<ProjectName>

chmod a+x iotedge-configuration-system-manager-template
sudo apt install -y curl jq
sudo ./iotedge-configuration-system-manager-template
sudo systemctl start iotedge
sudo journalctl -u iotedge
sudo journalctl -u iotedge -f
iotedge list
iotedge logs DaikinSystemManagerWebService
```
18. Check users
```
less /etc/passwd
getent passwd
# Only show user name
cut -d: -f1 /etc/passwd
awk -F: '{ print $1}' /etc/passwd
getent passwd | awk -F: '{ print $1}'
getent passwd | cut -d: -f1
# Count users
getent passwd | wc -l
# List current logged in users
w
who
users
# List user groups
groups
id -nG
getent group | awk -F: '{ print $1}'
getent group | cut -d: -f1
# List users in group
getent group iotedge
```
