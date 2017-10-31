# Basic Network Commands
===========================  
1. `$/usr/sbin/arp -a`    # Print out ARP table  
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
   $netstat -r                         # routing table and check zones
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
   $ls -las  # list all in current folder with permission and block number
   $lsmod    # list modules
   $lsof     # list open files
   $lsblk    # list block
   ```
8. Info command
   ```
   $whereis rpcinfo                   # find where is the rpcinfo  
   $/usr/sbin/rpcinfo   -p 10.10.3.18 # rpc call to rpc server, show all registered services on rpcbind
   $modinfo nfs                       #print out module info
   ```

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
   $stat --format "%F" <filename> #the same to $file
   $cat  <filename>      # show file content if ASCII
   $stat <filename>      # show file detail metadata, "%y" --last modification, "%o" --number of blocks
   $xxd  <filename>      # check file primary data
   Note: output of stat, the link number shows the hard links.
   ```
12. Kill  
   ```
   1) $kill -9 <pid#>           # kill process anyway  
   2) $kill -s <signal> <pid#>  # kill can send signal to a process, default TERM
   	  $kill -SIGTSTP 1126   #stop pid
   	  $kill -SIGCONT 1126   #restart pid
   ```
13. Fuser  --find which process is using a file or directory or a socket
   ```
   1) nc -l -p 1234   #create a port listening
   2) fuser -v -n tcp 1234
   3) fuser -k 1234/tcp   #kill process which created the port
   ```
14. `$history | grep nc`    #list all about nc command
15. `$klist`                #list ticket cache of Kerberos **Hacker like this**
16. `$w` OR `$who -uH`      #**Hacker like IDLE**
17. sha512sum **the xxx and sha512xxx should be in the same folder**
   ```
   $sha512sum xxx > sha512xxx
   $sha512sum -c sha512xxx        #check it
   $cat sha512xxx | sha512sum -c  #echo check
   ```
18. `$top -d 0.00001`             #most severe denial of service **Hacker like it**
19. Redirection  **ctl + d** to end of file
   ```
   1) $ls > /tmp/tmp_ls  #create new file containing the output of ls
   2) $echo 'Another line' >> /tmp/tmp_ls  #append a new line
   3) $find '' 2> stderr_log.txt   #standard error
   4) $wc '' 2>> stderr_log.txt    #append error
   3) < for input, << for input append  
   ```
20. Multiple commands
   ```
   1) $ls -l ; cat /etc/passwd
   2) $date && ls -l
   ```
21. Backticks:
   ```
   $kill `cat /var/run/named/named.pid`
   ```
22. Find
   ```
   $find . -atime 7
   $find . -name core -exec rm {} \; #find all named core file and delete them
   $find . -size -100k               #find all size less than 100k files
   ```
23. Compression
   ```
   1) $gzip myfile.txt          #myfile.txt.gz
      $gzip -d myfile.txt.gz    #decompress
     `$gzip -9 *.html`         #compress multiple files
   2) bzip2    #same to the previous
   3) $tar -cf archive.tar foo bar  # Create archive.tar from files foo and bar.
      $tar -tvf archive.tar         # List all files in archive.tar verbosely.
      $tar -xf archive.tar          # Extract all files from archive.tar.
   ```
24. Filters for pipe (find, grep, wc, tee, and tr)
   ```
   $wc /etc/magic | tee magic_count.txt  #create a new file or overwrite the file
   `$ls ~ | grep *tar | tr e E >> ls_log.txt`  #find and replace e to E
   ```
25. Check if your linux using UUID (using it to manage hard drives easily)
   ```
   $cat /proc/cmdline
   $cat /etc/fstab
   $findfs UUID=a98fee64-4820-4802-8fe5-2e6e07208980
   ```
26. Getfacl
   ```
   $getfacl ~/www2/bios # permission show
   $nfs4_getfacl ~/www2/bios
   $nfs4_getfacl -H  #<type>:<flags>:<principal>:<permission>
   ```
27. ss --display more socket and state information, like `$netstat`
   ```
   $ss -itoea | grep 5000  #internal TCP info, TCP,FIN-wait-1,Detail,All
   ```
28. Watch --execute a program periodically, showing output fullscreen
   ```
   $watch -n 1 ls -l xxxx
   $chmod 777 xxxx
   ```
29. Links (**Cannot make a across partition hard link**)
   ```
   $ln -s sourceFile newSymbolicLink  #create a symbolic link
   $ln sourceFile newHardLink         #create a hard link
   $rm newSymbolicLink
   $rm newHardLink
   ```
30. Check cache and buffer speed
   ```
   $sudo hdparm -tT /dev/sda1    #cache faster than buffer
   ```
31. `$cal -j`   #days of year
32. Check out file physical size
   ```
   $ls -las data.txt   #block #
   $sudo blockdev --getbsz /dev/sda1  #block size
   #block size * # of blocks = physical size
   ```
33. `strace -p 18523`   #pid, used to trace process read and write 
34. Random Access
   ```
   $head -$((${RANDOM} % `wc -l < car.csv` + 2)) car.csv | tail -1  #random query, head: first n line, tail: last n lines
   $awk -f ./awkindexlu -v indexes="1 2 3 4 5 6 7 10" indexed.dat | sort -n
   $cat > index.txt    #2 4 6 8
   $awk -f ./awkindexlu -v indexes="`index.txt`" indexed.dat | sort -n
   $cat > indexneg.txt  #-3 -5 -7
   $awk -f ./awkindexlu -v indexes="`indexneg.txt`" indexed.dat | sort -n
   ```
35. `perf stat -B java class PrimeByVector`  #performance analysis tool
36. `$uptime`   #how long box is running
37. pmap --report memory map of a process
   ```
   $pmap -x 28961 | grep stack
   ```
38. `$id`   #show detail of current user
39. `$chmod o+t ~/stickeyDir`   #prevent deleting this directory
40. touch command, used to hide hacker foot print
   ```
   $touch filename
   $touch -m filename    #change modification time
   $touch -c -t 0101011111 filename #change access and modification time
   ```