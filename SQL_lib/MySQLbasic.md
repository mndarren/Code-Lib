#MySQL Basic Commands
==========================
1. Database operations
   ```
   show databases;
   use musicstore;
   ```
2. Tables operations
   ```
   show tables;
   create table table_name (
       );
   Update merchandise set inventory_count = inventory_count-1 where id=1;
   Alter table merchandise engine = innodb;
   show table status where name = 'merchandise';
   ```
3. Query
   ```
   Select * from Sales;
   SELECT DISTINCT ROUTINE_NAME, ROUTINE_TYPE
        FROM INFORMATION_SCHEMA.ROUTINES
        WHERE ROUTINE_DEFINITION LIKE '%guitar%';
   show create procedure search_for_guitars;
   Select count( * ) from merchandise where merchandise.description like '%guitar%';
   ```
4. Run functions
   ```
   call search_for_guitar();
   ```
5. MetaData
   ```
   source mysqlsampledatabase.sql;   # run script file
   show processlist;                 # show ID and user name
   show privileges;                  # show privileges
   show status;                      # show DB engine number of threads created and running
   show columns from customers;      # show table stucture
   ```
