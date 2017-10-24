# Attacker Tools and Security Tools
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
      -T (paranoid0|sneaky1|polite2|normal3|aggressive4|insane5) (1,2 for evasion of IDS)
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
   4) $nc -l 1237 | nc 127.0.0.1 111  #redirect port, user connectting 1237 will go to 111
   ```
   [logtrap.sh](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/bash_code/logtrap.sh)
4. gdb (Attacker and Security) --dump data from register  
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
   [monitorLog.sh](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/bash_code/monitorLog.sh)
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
   ```
   [encServ.sh](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/bash_code/encServ.sh)
   [encCli.sh](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/bash_code/encCli.sh)
9. 