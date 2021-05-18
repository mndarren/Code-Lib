# Linux Commands
===========================  
1. Git Commands
```Git
git clone <repo_url> local_dir_name  # can rename the git repo local folder
git reset --hard HEAD  # revert file change to origin
git clean -ndx  # n: not really remove; d: recurse; x: don't use gitignore
git clean -fdx  # f: force to delete
git lfs fetch --all  # fetch all binary files
git lfs pull --exclude= --include=ubuntu-iso/ubuntu-20.04.1-legacy-server-amd64.iso  # just pull one .iso
git lfs ls-files  # show all lfs files, not work
cat .git/config  # look at git config file
```
2. `losetup  -a`  # will show all /dev/loop
3. `less file1.txt`  # view a file one screen at a time
4. `dpkg -l | grep git-lfs`  # show git-lfs version info
5. `find . -name ubuntu-20\*.is\* -exec ls -l {} \;`  # search ubuntu version 20 .iso files
6. USB Drive Action
```USB
dmesg | tail  # list all USB ports
sudo mount /dev/sdb1 /mnt/  # mount sdb1 to /mnt/ dir
cp /mnt/ubuntu-20.04.2-live-server-amd64.iso holtr-test/ubuntu-iso/ubuntu-20.04.1-legacy-server-amd64.iso
umount /mnt
sudo mount /dev/sdb1 /mnt/
rsync -aHPv ubuntu-20.04.1-legacy-server-amd64.iso  /mnt/  # override xx.iso to USB drive
umount /mnt
`df`  # show all devices
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
