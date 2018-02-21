# Attacker Tools and Security Tools
======================================  
1. Tcpdump (Attacker and Security) (Good packet sniffer)
   ```
   1) $sudo tcpdump port 23 -e -n -vvv -X -i lo -c 33 -w filename
   2) $sudo tcpdump arp -n -vvv -X -c6     #icmp as well
   3) tcpdump filter --<protocol header> [offset:length] <relation> <value>  
      (1) $tcpdump 'ip[9] = 1'  # find ICMP records
      (2) $tcpdump 'ip[0] & 0x0f > 5' or `$tcpdump 'ip[0] & 15 > 5'` #TOS
      (3) $tcpdump 'tcp'    #collect TCP records
      (4) $tcpdump 'ip[19] = 0xff' OR $tcpdump 'ip[19] = 255'
      (5) $tcpdump '(tcp[13] & 0x02 = 2) && (ip[2:2] - tcp[12] & 0xf0>>4*4 - ip[0] & 0x0f*4 > 0)'
          # data shouldn't be transported in a SYN packet
   ```
2. Nmap (Attacker)
   ```
   1) $time nmap -sV -p 1-200 127.0.0.1 -T 1  #record time, show service/Version info, port 1-200, localhost
   2) -sS (TCP SYN scan),  
      -sT (TCP connection scan),  
      -sU (UDP scans),
      -sN (TCP NULL scan),
      -sF (TCP FIN scan),
      -sA (TCP ACK scan),
      -sW (TCP Window scan),
      -sX (TCP Xmas scan: including FIN, PSH and URG),
      -sM (TCP Mainon scan: including NULL, FIN, and Xmas)
      -sI (Idle scan)
      -sO (IP protocol scan)
      -sP (ping scan, ICMP ECHO REQUEST and then TCP ping if no ICMP reply)
      -sn (ping scan, no port scan)
      -sR (RPC scan)
      -Pn (no ping, treat all host online)
      -b (FTP bounce scan)
      -oN/-oX/-oS/-oG <file> (output in nomal,XML,script,Grepable format)
      -oA <basename> (output in 3 major formats)
      -T (paranoid0|sneaky1|polite2|normal3|aggressive4|insane5) (1,2 for evasion of IDS) Set a timing template
      -O (enable OS detection)
      -A (Enable OS detection,version detection,script scanning,and traceroute)
   3) $time nmap -sX -p 0-100 -vvv 199.17.59.234/24 -T 5  #scan more IP
   ```
3. Netcat (Attacker and security) (Swiss Army Knife) (netcat,ncat,nc)  
   (connect to somewhere, listen for inbound, tunnel to somewhere)
   ```
   1) $nc -v -o xxx2 127.0.0.1 22    #connect somewhere, output a file
   2) $nc -v -c 'echo this is a test' -l -p 1235 -t   #create bogus service
      $telnet 127.0.0.1 1235
   3) $nc -vv -l -c './logtrap.sh' localhost -p 1236 -t -o xxzz  #run a script file
      $sudo tcpdump port 1236 -n -vvv -X -i lo -c 66 -s 200
      $netstat -apeen | grep nc    #grab pid
      $ps -aux | grep <pid>
      $telnet localhost 1236
      $sudo netstat -apeen | grep 1236
   4) $nc -l 1237 | nc 127.0.0.1 111     #redirect port, user connectting 1237 will go to 111.
      $nc -l 12345 | nc www.google.com 80  
      #request will be sent to google, but the response will not be sent to the browser.
   5) $mkfifo backpipe
      $nc -l 12345 0<backpipe | nc www.google.com 80 1>backpipe   #redirect input and output
   6) $ncat -l 12345 -c 'nc www.google.com 80'                    #one shot output
   7) $nc -v -n -z -w 1 127.0.0.1 1-1000   
      #port scanning, -n prevents DNS lookup, -z not receive any data, -w 1 connection timeout after 1 second of inactivity
   8) $nc -l -p 1234 -e /bin/sh    #run this command in IP 192.168.1.2, -e redirected input and output via network socket
      $nc 192.168.1.2 1234         #in another computer of the same network
      $ls -las                     #Making any process a server
   9) $ncat --sh-exec "ncat example.org 80" -l 8080 --keep-open  #redirect port 8080 on local port 80
   10)$ncat --exec "/bin/bash" -l 8081 --keep-open                #for world access freely
   11)$ncat --exec "/bin/bash" --max-conns 3 --allow 192.168.0.0/24 -l 8081 --keep-open
      #limit access to hosts on local network, max connection to 3
   12)$ncat -l --proxy-type http localhost 8888 #create an HTTP proxy server on localhost port 8888
   13)user@HOST1$ncat -l 9899 > outputfile
      user@HOST2$ncat HOST1 9899 < inputfile   #one-file sending server
   14)user@HOST1$ncat -l 9899 < inputfile
      user@HOST2$ncat HOST1 9899 > outputfile  #one-file receiving server
   15)Encrypted file transfer  #scp will create another connection
      When SSH in, add -L 31000:127.0.0.1:31000
      On the remote: nc -lvnp 31000 127.0.0.1 > file
      On the local: nc -v -w 2 127.0.0.1 31000 < file
   ```
   [logtrap.sh](https://github.com/mndarren/Code-Lib/blob/master/Data_Security_lib/resource/bash_code/logtrap.sh)
4. gdb (Attacker) --Find memory address from register and dump data  
   ```
   1) $./add # run program and keep it running
      $ps -al | grep add # find pid (say 11334)
   2) $gdb --pid 11334
   	  $info registers                #rdx, rdi
   	  $x rsi   OR $x/i $rsi
   	  $x/64ca <r8 address>
   3) $cat /proc/pid#/maps | more    # check useful memory addresses, using another putty session
   4) $dump memory ~/register583 0x777777777 0x777777877  #dump data into a file
   5) $detach pid#       #disconnect the process, **otherwise we cannot kill the process**, attach pid#
   6) $quit
   7) $xxd ~/register583 # check the primary data
   ```
5. dd (Data Dump)
   ```
   1) clone hard disk 
      $dd if=/dev/sda of=/dev/sdb
   2) backup a partition to a file
      $dd if=/dev/sda2 of=~/hda2disk.img
      $dd if=~/hda2disk.img of=/dev/sdb3   #restore
   3) compress a big file
      $dd if=/dev/sda2 | bzip2 hdadisk.img.bz2
   4) Wipe delete volume
      $dd if=/dev/zero of=/dev/sdb    #all bits are zero
      $for i in {1..10}; do dd if=/dev/random of=/dev/sdb; done   #multiple times
   5) create virtual swap space
      $dd if=/dev/zero of=/swapfile bs=1024 count=200000    #block size, blocks
   6) ddrescue to recover data
      $apt-get install gddrescue    #install
      $ddrescue -f -n /dev/sdx /dev/sdy rescue.log
      $ddrescue -d -f -r3 /dev/sdx /dev/sdy rescue.log
      $fsck -f /dev/sdy     #sdy is a bad disk
   7) convert file format between ASCII and EBCDIC
      $dd if=ascii.txt of=ebcdic.txt conv=ebcdic
      $dd if=ebcdic.txt if=ascii.txt conv=ascii
   8) Logical size to physical block size
      $dd if=ascii.txt of=fsync.txt conv=sync
   9) Create random file
      $dd if=/dev/random of=ranfile bs=1k count=1
   10) Not overwrite the existing file
      $dd if=ascii.txt of=comma.txt conv=excl
   11) Readable random file
      $base64 /dev/urandom | head -c 100 > urand.txt
   ```
6. Ramdisk (for better performace)
   ```
   1) Create ramdisk
      $free    #check memory available
      $sudo mkdir -p /mnt/ramdisk  #create directory
      $sudo mount -t tmpfs -o size=100m tmpfs /mnt/ramdisk  #create ramdisk
      $df   #check it out
   2) sudo cat -n sqllog > /mnt/ramdisk/sqllog2 #load sqllog into ramdisk with line #
   3) time cat /mnt/ramdisk/sqllog2 | grep "%27+or+27%"  #get location
   ```
   [monitorLog.sh](https://github.com/mndarren/Code-Lib/blob/master/Data_Security_lib/resource/bash_code/monitorLog.sh)
7. GPG (security encryption)  **3 times for more secure**
   ```
   $gpg -c zzz483
   $ls -l zzz483*
   $file zzz483.gpg
   $cat zzz483.gpg
   $xxd zzz483.gpg
   $gpg -d zzz483.gpg
   ```
8. Pipe (named:user level && unnamed:kernel level) [unnamed pipe example](https://github.com/mndarren/Code-Lib/blob/master/cpp_lib/unnamed_pipe/Interface.c)
   ```
   $mkfifo pipexz      #create a named pipe
   $ls -l > pipexz     #write into pipe
   $cat < pipexz       #read from pipe
   #named pipe can be used in network communication, unnamed pipe cannot;
   #named pipe can do multiprocess communication, unnamed pipe cannot;
   #named pipe exists in FS, independently from process, unnamed pipe vanished as soon as it's closed or complete execution
   $mkfifo backpipe
   $nc -l 12345 0<backpipe | nc www.google.com 80 1>backpipe  #redirect input and output
   #the following is a big one:
   mkfifo tmp
   mkfifo tmp2
   nc -l 8080 -k > tmp < tmp2 &
   while true; do
      openssl s_client -connet www.google.com:443 -quiet < tmp > tmp2
   done;    #the traffic cannot be viewed in wire sniffer, like wireshark
   ```
   [encServ.sh](https://github.com/mndarren/Code-Lib/blob/master/Data_Security_lib/resource/bash_code/encServ.sh)  
   [encCli.sh](https://github.com/mndarren/Code-Lib/blob/master/Data_Security_lib/resource/bash_code/encCli.sh)
9. StromBringer (famous with nmap and ncat) focus on layer 2 (MAC = 48 bits = 12 Hex)
   ```
   $./e-ping lo 00 00 00 00 00 00   00 00 00 00 00 00  #only way to catch packet by tcpdump with e-ping utility
   $./e-stat virnet0                                   #using utility e-stat to see the traffic
   $./e-listen virnet0                                 #using utility e-listen to see a trace of traffic
   $./psend2 venet0:0 adr 00 00 00 00 00 00 addr 7E EE 93 EB B1 C2 #(P10 trace)  #broadcast packets to real mac
   $./scripts/Strombringer.sh venet0:0                 #lock up the virtual terminal, using strombringer shell
   $./nfork                                            #1048 means thresh hold before denial service
   ```
10. How to remote Kali
   ```
   apt-get update && apt-get upgrade
   apt-get dist-upgrade
   apt-get install xrdp
   service xrdp start
   service xrdp-sesman start
   # if got error
   apt-get remove gnome-core
   apt-get install lxde-core lxde kali-defaults kali-root-login desktop-base
   update-alternatives --config x-session-manager
   		# choose /usr/bin/startlxde
   reboot
   ```
11. FTK Imager
   ```
   link: https://accessdata.com/products-services/forensic-toolkit-ftk
   
   ```