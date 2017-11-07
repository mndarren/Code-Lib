# Cybersecurity Strategies
====================================  

0. Things related to managing data
   ```
   size
   format
   access method
   who can access
   location
   retrieval speed
   versions
   redundancy
   information hiding
   ability to tamper with the file
   un-intended copies of the data
   ```
   Layered Security
   ```
   Layer 1, network layer: VPN, honeynet, IDS/IPS, SSL, cloud zones
   Layer 2, Memory layer: pipe
   Layer 3, OS layer: Credentials, 2-factor anthentication, periodly change password, ldap and Kerberos
   Layer 4, FS layer: soft link to data, change location, change folder name
   Layer 5, Data layer: Digital signature, Compress data, chmod data and use script to modify data, Diff format data, encrypted data
   ```
1. Strategy: close all permission for sensitive files, modify it using script file
   ```
   $chmod -rwx myPass.txt
   $./changeMyPass
   ```
   [changeMyPass.sh](https://github.com/mndarren/Code-Lib/blob/master/Data_Security_lib/resource/bash_code/readPass.sh)
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
5. Remember:"Default is dangerous!", so change DB default settings.  
   Hacker's likes
   ```
   /etc/passwd
   /etc/shadow
   /var/logs
   root id (0)
   stop touch command, because hacker uses it to hide his foot print. use finger print to check file integration
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
9. Commands for analysis
   ```
   $netstat -s       #show the health of these software components
   $route -C         #Kernel IP routing cache
   ```
10. Two ways to find bad process id
   ```
   $ps -aux | grep java
   $lsof -t ~/javaclass/keyfile.txt
   ```
11. Moving data: Encrypted, duplicated and conpressed data before moving through any pipe.
12. Risk Assessment --Justifying Countermeasure Priority
   ```
   Step1, create severiry table
          Involves super user: 5
          Involves executable code: 4
          Attack on OS resource: 3
          Attack on a service: 2
          Attack on documentation/versioning/protocol: 1
   Step2, grab data from log file
          $cat secure | grep root | wc -l    #3652
          $cat secure | grep root | grep password | wc -l   #2433
          $uptime    #19days 2:24
   Step3, Calculate attack coefficient
          Attack coefficient on incidence level: 2433 * 5 = 12165
          Attack coefficient on temporal average: 2433 / (19 * 24 + 2) = 2433/458 =~ 5.3 IPH * 5 = 26.5
   ```
13. Rist Analysis by Tyhpe/source or destination
   ```
   Critical, Severe, and Moderate attacks *bar graph*;
   Common Vulnerabilities *pie chart*.
   Event analysis of auth fail log *curve plane*.
   **fail2ban.log and secure.log can be used together to find the attacker**
   ```
14. 