# Cybersecurity Strategies


1. Strategy: close all permission for sensitive files, modify it using script file
   ```
   $chmod -rwx myPass.txt
   $./changeMyPass
   ```
   [changeMyPass.sh](https://github.com/mndarren/Code-Lib/blob/master/Linux_Security_lib/resource/bash_code/readPass.sh)
2. Log policy
   ```
   1) keep 7 versions
   2) compress 5 older versions
   3) create version once per week
   4) keep them 3 months
   5) location, permission, encryption
   ```
3. Policy: Backup data (Redundency, 3 replicas)
4. Policy: Don't let people download data to laptop, only deal with it online.
5. Remember:"Default is dangerous!", so change DB default settings, root id (0), /etc/shadow etc.  
   Hacker's likes
   ```
   /etc/passwd
   /etc/shadow
   /var/logs
   ```
6. Log Data Analysis example
   ```
   $cat syslog.1 | wc -l    #total events
   $cat syslog.1 | grep "authentication fail" | wc -l   #failed authentication events
   failed events/total events = 4/2223 = .224%
   Policy: set up a risk alert for that!
   ```
7. Policy hot spots:
   ```
   $w -o              #need policy to reduce limit IDLE times
   $top -d 0.0001     #need policy cannot < 1 minute
   $vmstat 1          #1 second
   ```
8. Passwd hack in the worst order:
   ```
   ldap compromise -> system shadow -> Fred password in name server cache -> system passwd
   ```