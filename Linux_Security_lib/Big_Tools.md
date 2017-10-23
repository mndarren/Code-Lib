# Attacker Tools and Security Tools
1. Tcpdump (Attacker and Security)  
   1) $sudo tcpdump port 23 -e -n -vvv -X -i lo -c 33 -w filename
   2) tcpdump filter --<protocol header> [offset:length] <relation> <value>  
      (1) $tcpdump 'ip[9] = 1'  # find ICMP records
      (2) $tcpdump 'ip[0] & 0x0f > 5' or `$tcpdump 'ip[0] & 15 > 5'` #TOS
      (3) $tcpdump 'tcp'    #collect TCP records
      (4) $tcpdump 'ip[19] = 0xff' OR $tcpdump 'ip[19] = 255'
      (5) $ 

2. Nmap (Attacker)

3. Netcat (Attacker)

4. gdb (Attacker and Security) --dump data from register  
   ```
   1) $./add # run program and keep it running
      $ps -al | grep add # find pid (say 11334)
   2) `$gdb --pid 11334`  
   	  `$info registers`                #rdx, rdi  
   	  `$x rsi`   OR `$x/i $rsi`  
   	  `$x/64ca <r8 address>`  
   3) `$cat /proc/pid#/maps | more`    # check useful memory addresses, using another putty session
   4) `$dump memory ~/register583 0x777777777 0x777777877`  #dump data into a file
   5) `$detach pid#`       #disconnect the process, **otherwise we cannot kill the process**, attach pid#
   6) `$quit`
   7) `$xxd ~/register583` # check the primary data
   ```
5. dd (Data Dump)

6. strace (Security debug tool)  
   1) 