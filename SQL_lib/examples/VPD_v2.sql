--@Author: Darren Xie
--@Date: 10/29/2017

--Step 1, prepare for context var
-- 1.1 create ctx user
conn SYS/xxxxxx@localhost/IA643 as sysdba;
Drop User sysadmin_ctx CASCADE;
CREATE USER sysadmin_ctx IDENTIFIED BY playboy;
GRANT CREATE SESSION, CREATE ANY CONTEXT, CREATE PROCEDURE, CREATE TRIGGER, 
      ADMINISTER DATABASE TRIGGER TO sysadmin_ctx IDENTIFIED BY playboy;

GRANT EXECUTE ON DBMS_SESSION TO sysadmin_ctx;
GRANT EXECUTE ON DBMS_RLS TO sysadmin_ctx;
GRANT RESOURCE TO sysadmin_ctx;

conn DBA643/xxxxxx@localhost/IA643;
GRANT SELECT ON DBA643.Branch_Managers TO sysadmin_ctx;


--1.2 create context
conn sysadmin_ctx/playboy@localhost/IA643;
CREATE OR REPLACE CONTEXT MANAGER_CTX USING set_manager_ctx_pkg;

--1.3 create related procedure
CREATE OR REPLACE PACKAGE set_manager_ctx_pkg IS 
   PROCEDURE get_branch_id; 
 END; 
 /
CREATE OR REPLACE PACKAGE BODY set_manager_ctx_pkg IS
   PROCEDURE get_branch_id 
   IS 
    branch_id_v NUMBER;
   BEGIN 
    SELECT BRANCH_ID INTO branch_id_v FROM DBA643.Branch_Managers 
       WHERE upper(SYSTEM_USERNAME) = upper(SYS_CONTEXT('USERENV', 'SESSION_USER'));
    DBMS_SESSION.SET_CONTEXT('MANAGER_CTX', 'BRANCH_ID_CTX', branch_id_v);
   EXCEPTION  
    WHEN NO_DATA_FOUND THEN NULL;
  END;
 END;
/
show error;
--1.4 create related trigger
CREATE or Replace TRIGGER set_manager_ctx_trig AFTER LOGON ON DATABASE
 BEGIN
  sysadmin_ctx.set_manager_ctx_pkg.get_branch_id;
 END;
/
show error;
-- Step 2, Create policy
Create or Replace FUNCTION Sec_Fun_Cust (P_schema_name IN varchar2, P_object_name IN varchar2) Return varchar2 IS
  where_v varchar2(300);
BEGIN
  IF USER <> 'DBA643' then
    where_v := 'BRANCH_ID = ' || NVL(SYS_CONTEXT('MANAGER_CTX','BRANCH_ID_CTX'),0);
  ELSE
    where_v := '';
  END IF; 
  return where_v;
END;
/
show error;

Create or Replace FUNCTION Sec_Fun_Cust1 (P_schema_name IN varchar2, P_object_name IN varchar2) Return varchar2 IS
  where_v varchar2(300);
BEGIN
  IF USER <> 'DBA643' then
     where_v := 'CTL_SEC_USER IN (SELECT UPPER(SYSTEM_USERNAME) FROM DBA643.Branch_Managers WHERE BRANCH_ID = '|| NVL(SYS_CONTEXT('MANAGER_CTX','BRANCH_ID_CTX'),0) || ')';
  ELSE
    where_v := '';
  END IF; 
  return where_v;
END;
/
show error;

exec DBMS_RLS.DROP_Policy('DBA643', 'Branches', 'Row_Owner_Sec');
BEGIN
  DBMS_RLS.ADD_POLICY (object_schema     => 'DBA643',
                       object_name       => 'Branches',
                       policy_name       => 'Row_Owner_Sec',
                       function_schema   => 'sysadmin_ctx',
                       policy_function   => 'Sec_Fun_Cust',
                       statement_types   => 'SELECT, UPDATE, DELETE, INSERT',
                       update_check      => TRUE);
END;
/
exec DBMS_RLS.DROP_Policy('DBA643', 'Customers', 'Row_Owner_Sec');
exec DBMS_RLS.ADD_Policy ('DBA643','Customers','Row_Owner_Sec','sysadmin_ctx','Sec_Fun_Cust','SELECT, UPDATE, DELETE, INSERT',TRUE);
exec DBMS_RLS.DROP_Policy('DBA643', 'Financial_Products', 'Row_Owner_Sec1');
exec DBMS_RLS.ADD_Policy ('DBA643','Financial_Products','Row_Owner_Sec1','sysadmin_ctx','Sec_Fun_Cust1','SELECT, UPDATE, DELETE, INSERT',TRUE);
exec DBMS_RLS.DROP_Policy('DBA643', 'Transactions', 'Row_Owner_Sec1');
exec DBMS_RLS.ADD_Policy ('DBA643','Transactions','Row_Owner_Sec1','sysadmin_ctx','Sec_Fun_Cust1','SELECT, UPDATE, DELETE, INSERT',TRUE);

-- Step 3, Create another 2 users
conn DBA643/xxxxxx@localhost/IA643;
DROP USER PAdmin CASCADE;
Create User PAdmin identified by PAdmin
    DEFAULT TABLESPACE IA643_TBS
    TEMPORARY TABLESPACE TEMP
    ACCOUNT UNLOCK;
    
DROP USER KAdmin CASCADE;
Create User KAdmin identified by KAdmin
    DEFAULT TABLESPACE IA643_TBS
    TEMPORARY TABLESPACE TEMP
    ACCOUNT UNLOCK;
/
show error;
Grant Connect, Resource To PAdmin, KAdmin;

-- Step 4, create role and grant it to managers
Drop Role Manager_R;
Create Role Manager_R;
Grant SELECT, INSERT, UPDATE, DELETE ON Branches To Manager_R;
Grant SELECT, INSERT, UPDATE, DELETE ON Customers To Manager_R;
Grant SELECT, INSERT, UPDATE, DELETE ON Financial_Products To Manager_R;
Grant SELECT, INSERT, UPDATE, DELETE ON Transactions To Manager_R;

Grant Manager_R To BAdmin, CAdmin, PAdmin, KAdmin;
/
set serveroutput on;