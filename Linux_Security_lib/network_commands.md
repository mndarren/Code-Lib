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
   $netstat -an                   # print litsening and established ports
   $netstat -i                    # like $ifconfig
   $netstat -r                    # routing table
   $sudo netstat -apeen | more    # all protocols ethernet number format
   ```
4. Trace IP or Domain hops
   ```
   $mtr www.google.com       # how many hops to get the IP
   $traceroute www.google.com
   ```
5. $telnet 127.0.0.1         # communicate with a host
   $