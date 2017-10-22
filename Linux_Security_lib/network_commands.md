# Basic Network Commands

1. `$arp -a`    # Print out ARP table  
2. Check IP or Domain info
   ```
   $dig stcloudstate.edu     # get IP address
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
   $mtr www.google.com       # how many hops to get the IP
   $traceroute www.google.com
   ```
5. Communicate to a host with TCP/UDP
   ```
   $telnet 127.0.0.1         # communicate with a host (UDP)
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
   $mpstat -P 0   # Multiprocessing stat for CPU 0
   $mpstat -A     # all CPU stat
   $uname -a      # info of Linux
   ```
10. `mount | grep nfs`   # find location of nfs
11. 