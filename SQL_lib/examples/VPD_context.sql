--@Author: Darren Xie
--@Date 10/29/2017
/**@issue-fixed:
1.	In the policy function, the return value of SYS_CONTEXT('USERENV', 'SESSION_USER') are all upper case, but (SYSTEM_USERNAME) will return different case value. So I changed it to this:
WHERE upper(SYSTEM_USERNAME) = upper(SYS_CONTEXT('USERENV', 'SESSION_USER'));
2.	After inserting new record as PAdmin, we cannot see it as DBA643. So we must use commit; like last time.
3.	If you want to reset up everything, we should run 3 script file (create_tables.sql -> pre_solution.sql -> context_var_setting.sql) otherwise the triggers on p4 will be gone since those dropped and created again.
4.	In the p5 solution script, we should change to DBA643 account before run policy function. It will not work if let sysadmin_ctx user to do that.
*/

--Step 1, prepare for context var
-- 1.1 create ctx user
conn SYS/Jiling#7@localhost/IA643 as sysdba;
Drop User sysadmin_ctx CASCADE;
CREATE USER sysadmin_ctx IDENTIFIED BY playboy;
GRANT CREATE SESSION, CREATE ANY CONTEXT, CREATE PROCEDURE, CREATE TRIGGER, 
      ADMINISTER DATABASE TRIGGER TO sysadmin_ctx IDENTIFIED BY playboy;

GRANT EXECUTE ON DBMS_SESSION TO sysadmin_ctx;
GRANT EXECUTE ON DBMS_RLS TO sysadmin_ctx;
GRANT RESOURCE TO sysadmin_ctx;

conn DBA643/Dubai9$@localhost/IA643;
GRANT SELECT ON DBA643.Branch_Managers TO sysadmin_ctx;

--1.2 create context
conn sysadmin_ctx/playboy@localhost/IA643;
CREATE OR REPLACE CONTEXT MANAGER_CTX USING set_manager_ctx_pkg;

--1.3 create related procedure
CREATE OR REPLACE PACKAGE set_manager_ctx_pkg IS 
   PROCEDURE get_branch_id; 
   PROCEDURE get_manager;
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
  
  PROCEDURE get_manager
  AS 
      CURSOR names_cur IS
          SELECT SYSTEM_USERNAME
          FROM   DBA643.Branch_Managers
          WHERE  BRANCH_ID = ( SELECT BRANCH_ID 
                               FROM DBA643.Branch_Managers 
                               WHERE upper(SYSTEM_USERNAME) = upper(SYS_CONTEXT('USERENV', 'SESSION_USER')));
      
      manager_cur  DBA643.Branch_Managers.SYSTEM_USERNAME%TYPE;
   BEGIN 
      OPEN names_cur;
      LOOP
        FETCH names_cur INTO manager_cur;
        EXIT WHEN names_cur%NOTFOUND;
        
        IF upper(manager_cur) <> upper(USER) THEN
           manager_cur := upper(manager_cur);  -- always use upper case
           DBMS_SESSION.SET_CONTEXT('MANAGER_CTX', 'MANAGER_NAME', manager_cur);
        END IF;
      END LOOP;
      CLOSE names_cur;
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
  sysadmin_ctx.set_manager_ctx_pkg.get_manager;
 END;
/
show error;
-- Step 2, Create policy
--conn DBA643/Dubai9$@localhost/IA643;
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
    where_v := 'CTL_SEC_USER = USER OR CTL_SEC_USER = SYS_CONTEXT(''MANAGER_CTX'',''MANAGER_NAME'')';
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
conn DBA643/Dubai9$@localhost/IA643;
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