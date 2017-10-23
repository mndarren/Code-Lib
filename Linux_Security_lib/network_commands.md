# Basic Network Commands

1. `$arp -a`    # Print out ARP table  
2. Check IP or Domain info
   ```
   $dig stcloudstate.edu     # get IP address (UDP)
   $dig -x 199.17.59.234     # get domain related to IP
   $nslookup  199.17.59.234  # the same to previous
   $whois stcloudstate.edu   # print out info related to the IP or domain
   ```
3. Check netstat  
   ```
   $netstat -an                        # print litsening and established ports
   $netstat -i                         # like $ifconfig
   $netstat -r                         # routing table
   $sudo netstat -apeen | grep java    # all protocols ethernet number format for java
   ```
4. Trace IP or Domain hops
   ```
   $mtr -n www.google.com       # how many hops to get the IP
   $traceroute www.google.com
   ```
5. Communicate to a host with TCP/UDP
   ```
   $telnet 127.0.0.1         # communicate with a host (TCP)
   $lynx www.google.com      # browser info for World Wide Web (TCP)
   ```
6. Check processes
   ```
   $ps -aux | grep sshd      # find sshd pid
   $ps -ALF | grep python    # list all python related
   ```
7. List stuff
   ```
   $ls -l    # list all in current folder with permission
   $lsmod    # list modules
   $lsof     # list open files
   ```
8. `whereis rpcinfo`       # find where is the rpcinfo  
   `$/usr/sbin/rpcinfo`    # rpc call to rpc server, show all registered services on rpcbind

9. Check inportant info
   ```
   $vmstat        # VM stat
   $mpstat -P ALL # Multiprocessing stat for all CPU (if 0, for CPU 0)
   $mpstat -A     # all CPU stat (R-running,D-Uninterruptible Sleep,S-sleep,T-stopped,W-pagging,X-dead,Z-zombie)
   $uname -a      # info of Linux
   ```
10. `mount | grep nfs`   # find location of nfs
11. Check file metadata
   ```
   $file <filename>      # check file type format
   $cat  <filename>      # show file content if ASCII
   $stat <filename>      # show file detail metadata
   $xxd  <filename>      # check file primary data
   ```
12. Kill  
   1) `$kill -9 <pid#>`           # kill process anyway  
   2) `$kill -s <signal> <pid#>`  # kill can send signal to a process, default TERM
13. Fuser  --find which process is using a file or directory or a socket  
   1) `nc -l -p 1234`   #create a port listening
   2) `fuser -v -n tcp 1234` 
   3) `fuser -k 1234/tcp`   #kill process which created the port
14. `$history | grep nc`    #list all about nc command
15. `$klist`                #list ticket cache of Kerberos **Hacker like this**
16. `$w` OR `$who -uH`      #**Hacker like IDLE**
17. sha512sum
   ```
   $sha512sum xxx > sha512xxx
   $sha512sum -c sha512xxx        #check it **the xxx should be in the same folder**
   $cat sha512xxx | sha512sum -c  #echo check
   ```
18. `$top -d 0.00001`             #most severe denial of service **Hacker like it**
19. 