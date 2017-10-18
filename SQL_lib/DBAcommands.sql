-- 1. Setup PDB
-- create PDB
conn sys/xxxx as sysdba;
CREATE PLUGGABLE DATABASE IA643
ADMIN USER DBA643 IDENTIFIED BY xxxx
--DEFAULT TABLESPACE USERS
ROLES = (CONNECT, DBA);

-- need to open a pluggable database
ALTER PLUGGABLE DATABASE IA643 open;

-- need to save open state so that no need to open it again when database is restarted
ALTER pluggable database IA643 SAVE STATE; 

-- set container to IA643
alter session set container = IA643;

--- 2. Check commands
-- current container current user
show con_name;
show user;
-- show userful stuff
select object_name from obj where object_name='DD_wpd_fun';
select profile, resource_name, LIMIT from DBA_profiles where profile='default';

select username, password from DBA_users;

select * from DBA_tablespaces;

--determine if it's a CDB
SELECT CDB FROM V$DATABASE;
--show containers in a CDB
SELECT NAME, CON_ID, DBID, CON_UID, GUID FROM V$CONTAINERS ORDER BY CON_ID;
--show info about PDB
SELECT PDB_ID, PDB_NAME, STATUS FROM DBA_PDBS ORDER BY PDB_ID;
--show PDB open mode
SELECT NAME, OPEN_MODE, RESTRICTED, OPEN_TIME FROM V$PDBS;
--show services
SELECT name FROM V$SERVICES;
