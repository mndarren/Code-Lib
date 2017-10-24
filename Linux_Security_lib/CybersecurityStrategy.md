# Cybersecurity Strategies


1. Strategy: close all permission for sensitive files, modify it using script file
   ```
   $sudo chmod 000 myPass.txt
   $./changeMyPass
   ```
   Script file: changeMyPass
   ```
   ?
   ```
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
5. 