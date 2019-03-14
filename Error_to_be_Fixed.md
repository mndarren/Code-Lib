# Errors to be Fixed
=====================================
1. ERROR: VT-x is disabled in the BIOS for all CPU modes
```
Fixed: Go to BIOS (with F1)  change virtualization settings to ‘enable’
```
2. Error: installing IntelliJ. CANNOT FIND JDK even install openjdk in linux
```
Fixed: have to download jdk1.8 and setup IntelliJ path to there
```
3. Error: VM win10 IP setting cannot connect internet after setting
```
Fixed: changed VM network type from NAT to Bridge.
```
4. Error: IP SETTING in Ubuntu
```
Fixed: couldn’t set up both /etc/network/interfaces and UI network setting. Deleted interfaces and reboot.
```
5. Pip3 Error: since the python3 packages are different from python, like pip and setuptools
```
Fixed: sudo apt-get install python3-pip
pip3 install --upgrade pip setuptools    #upgrade pip and setuptools package
```
6. Error: Cannot  Find Mysql
```
Sudo apt-get install python3-mysql.connector
```
7. How to Share Clipboard between Linux and Windows with Virtualbox
```
Devices -> shared clipboard -> bidirectional
```
8. Tip for Save Doc or file with key board: left Ctrl + S, not right Ctrl + S
9. couldn’t Full Screen Win10
```
Install virtualbox guest additions in windows10
```
10. windows 10 accidentally becomes White And Black
```
Settings -> color -> high contrast -> turn off the “Apply color filter”
```
11. Error: Database Error: could Not Open Extension control file "/usr/share/postgresql/9.6/extension/postgis.control": No such file or directory
```
Add the postgresql-9.6-postgis-scripts module:
apt-get install postgresql-9.6-postgis-scripts
```
12. Special character Error
```
Fixed with latin-1 instead of utf-8
```
13. Could not find package alabaster Error
```
Repository link is wrong: index = http://pypi.sfsi.stearnsbank.net/repository/pypi/pypi  (npm. Changed by accidentally). Go to .pip/pip.conf
```
14. Could not find cooper Error: make build
15. Parser function not work Error
```
Because one previous parser function terminated the process.
```
16. Updated PyCharm, and then have to type password when git push every time
```
Because the git cache timeout changed. Command:
Sudo git config --global credential.helper ‘cache –timeout 3600’
```
17. Error for variable initial
```
In Python, we have to initialize variables before use since type is dynamic.
```
18. CentOS install minishift: (just put Minishift in Ubuntu physical machine, works)
```
Sudo dhclient
Sudo yum install gcc
Sudo yum groupinstall "GNOME Desktop" "Graphical Administration Tools"
ln -sf /lib/systemd/system/runlevel5.target /etc/systemd/system/default.target
reboot
```
19. Evolution installation for Calendar sync
```
Just install the version evolution-ews
```
20. Google Chrome cannot access internet
```
Fixed add company certificate to the browser
```
21. Cannot access BCRL cloud
```
      1) set up wifi dns 8.8.8.8 8.8.4.4
      2) open OpenVPN as admin
```
22. MYSQL error: #2002 - Only one usage of each socket address (protocol/network address/port) is normally permitted. &mdash; The server is not responding (or the local server's socket is not correctly configured). 
```
The reason is in windows:
    1. netsh int ipv4 set dynamicport tcp start=1025 num=64511
    2. HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\TcpTimedWaitDelay          With value 30 (decimal)
```
23. How to fix ERR_SSL_VERSION_INTERFERENCE Chrome Error
```
(1) type chrome://flags/, press Enter
(2) Search TLS, and Disable TSL 1.3, then Relaunch
```
24. Too many client already for Postgresql
```
sudo sed -i 's/max_connections = 100/max_connections = 300/g' /etc/postgresql/9.4/main/postgresql.conf # Allow more than 100 connections to DB
sudo service postgresql restart
```
25. Postman passing in data json type should double quotes.
26. java.io.IOException: User limit of inotify watches reached
```
cat /proc/sys/fs/inotify/max_user_watches
echo 16384 | sudo tee /proc/sys/fs/inotify/max_user_watches
# permanently change
echo fs.inotify.max_user_watches=16384 | sudo tee -a /etc/sysctl.conf
sudo sysctl -p
```
27. snap-confine refuses to launch application or install application
```
# not good
sudo apt purge snapd snap-confine && sudo apt install -y snapd
# just check the home dir, someone solved issue via this
cat /etc/apparmor.d/tunables/home.d/ubuntu
# the following fixed the issue
reboot -> Enter / Esc -> in grub -> advanced option -> choose kernel 4.15.0-43 version (44 not work) -> run command to delete 44 version:
sudo apt-get purge linux-image-4.15.0-44-generic -f
```
28. UrlMappings matches Controller name in Grails app
```
if Controller name = 'CountriesController' (class name in controller) 
=> controller: 'countries' (name in UrlMappings)
```
29. SSL CERTIFICATE_VERIFY_FAILED
```
# create request_verify.py file
import warnings
import contextlib

import requests
from urllib3.exceptions import InsecureRequestWarning

try:
    from functools import partialmethod
except ImportError:
    # Python 2 fallback: https://gist.github.com/carymrobbins/8940382
    from functools import partial

    class partialmethod(partial):
        def __get__(self, instance, owner):
            if instance is None:
                return self

            return partial(self.func, instance, *(self.args or ()), **(self.keywords or {}))


@contextlib.contextmanager
def no_ssl_verification(session=requests.Session):
    old_request = session.request
    session.request = partialmethod(old_request, verify=False)

    with warnings.catch_warnings():
        warnings.simplefilter('ignore', InsecureRequestWarning)
        yield

    session.request = old_request
# the following is how to use it
with no_ssl_verification():
    request.post()
```
30. How to fix project itself not found error -- 'ModuleNotFoundError'?
```
pip install -e .
```
31. Cannot find IP address for CentOS
```
/etc/sysconfig/network-stripts/ifcfg-something 
make sure ONBOOT=yes
```
32. pkg-resources==0.0.0  # a bug from ubuntu, just delete it from requirements.txt
33. cache does not work: because "This can save time when a function is often called with the same arguments."