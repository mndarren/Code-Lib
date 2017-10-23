# Attacker Tools and Security Tools
1. Tcpdump (Attacker and Security)  
   ```
   1) $sudo tcpdump port 23 -e -n -vvv -X -i lo -c 33 -w filename
   2) $sudo tcpdump arp -n -vvv -X -c6     #icmp as well
   3) tcpdump filter --<protocol header> [offset:length] <relation> <value>  
      (1) $tcpdump 'ip[9] = 1'  # find ICMP records
      (2) $tcpdump 'ip[0] & 0x0f > 5' or `$tcpdump 'ip[0] & 15 > 5'` #TOS
      (3) $tcpdump 'tcp'    #collect TCP records
      (4) $tcpdump 'ip[19] = 0xff' OR $tcpdump 'ip[19] = 255'
      (5) $?
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
      -sn (ping scan, no port scan)
      -Pn (no ping, treat all host online)
      -b (FTP bounce scan)
      -oN/-oX/-oS/-oG <file>(output in nomal,XML,script,Grepable format)
      -oA <basename>(output in 3 major formats)
      -T (paranoid0|sneaky1|polite2|normal3|aggressive4|insane5) (1,2 for evation of IDS)
   3) $time nmap -sX -p 0-100 199.17.59.234/24 -T 5  #scan more IP
   ```
3. Netcat (Attacker) (Swiss Army Knife) (netcat,ncat,nc)
   ```
   1) $nc -v -o xxx2 127.0.0.1 22    #output a file
   2) $nc -v -c 'echo this is a test' -l -p 1235 -t   #char string, listen TCP, port 1235, answer telnet
      $telnet 127.0.0.1 1235
   3) $nc -vv -l -c './logtrap' localhost -p 1236 -t -o xxzz  #run a script file
      $sudo tcpdump port 1236 -n -vvv -X -i lo -c 66 -s 200
      $netstat -apeen | grep nc    #grab pid
      $ps -aux | grep <pid>
      $telnet localhost 1236
      $sudo netstat -apeen | grep 1236
   4) $nc -l 1237 | nc 127.0.0.1 111&  #redirect port ?
   ```
   Script file: logtrap
   ```
   cat /etc/motd
   echo welcome to Hermes
   echo login:
   read number
   echo
   echo $number > ~/passtrap
   echo passwd:
   read number
   echo
   echo $number >> ~/passtrap
   echo
   echo The system is resyncing try again later.
   ```

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

6. strace (Security debug tool)  http://www.thegeekstuff.com/2011/11/strace-examples
   1) 

7. Ramdisk (for better performace)

8. GPG (security encryption)
   ```
   $gpg -c zzz483     #**3 times for more secure**
   $ls -l zzz483*
   $file zzz483.gpg
   $cat zzz483.gpg
   $xxd zzz483.gpg
   $gpg -d zzz483.gpg
   ```
9. 