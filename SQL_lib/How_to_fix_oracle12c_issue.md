# status failure test failed IO error the network adapter could not establish the connection
============================================================================================

-- Once we encounter some connection error with listener stuff,
-- we can check the following things

1. Check port and server IP is OK to communicate
   ```
   $netstat -an   # check if 1521 port is listening (powershell if windows)
   $tnsping db_server  # check it

   listener.ora (path: C:\app\Admin\product\12.1.0\dbhome_1\NETWORK\ADMIN)
   tnslistener.ora (path:C:\app\Admin\product\12.1.0\dbhome_1\NETWORK\ADMIN)
   hosts (path: C:\Windows\System32\drivers\etc)
   # if we only use localhost, so we can comment out other server ip lines
   ```
2. In cmd window, we can check:
   ```
   #lsnrctl stop
   #lsnrctl start
   #lsnrctl service
   #lsnrctl status
   ```
3. From Start -> services:
   ```
   # we should restart Oracle listener service if any change made in the listener.ora
   ```
4. others
   ```
   Once couldn't connect to SYS account, go to check SQL developer version
   It will not work on the too old version. Use the latest version 17.2 right now

   PASSWORD EXPIRE only work in cmd line window, not work on SQL Developer
   #sqlplus
   Don't run .sql build-in files because they might change the default settings.
   Ortherwise we have to recreate a CDB.
   ```