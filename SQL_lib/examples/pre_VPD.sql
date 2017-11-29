--@Author: Darren Xie
--run 3 script file (create_tables.sql -> pre_solution.sql -> context_var_setting.sql)

-- set DBA role as an active role
set Role DBA;

-- Step1: set triggers for each table
CREATE OR REPLACE TRIGGER Trg_Insert_Branches
Before INSERT OR UPDATE ON Branches
For each Row
begin
  :new.CTL_SEC_USER := user;
end;
/

CREATE OR REPLACE TRIGGER Trg_Insert_Customers
Before INSERT OR UPDATE ON Customers
For each Row
begin
  :new.CTL_SEC_USER := user;
end;
/

CREATE OR REPLACE TRIGGER Trg_Insert_Branch_Managers
Before INSERT OR UPDATE ON Branch_Managers
For each Row
begin
  :new.CTL_SEC_USER := user;
end;
/

CREATE OR REPLACE TRIGGER Trg_Insert_Financial_Products
Before INSERT OR UPDATE ON Financial_Products
For each Row
begin
  :new.CTL_SEC_USER := user;
end;
/

CREATE OR REPLACE TRIGGER Trg_Insert_Transactions
Before INSERT OR UPDATE ON Transactions
For each Row
begin
  :new.CTL_SEC_USER := user;
end;
/

-- Step 2: Create policy function; when DBA643, s/he should see all records
--Create or Replace FUNCTION Sec_Fun_Cust (P_schema_name IN varchar2, P_object_name IN varchar2) Return varchar2 IS
--BEGIN
--  IF USER <> 'DBA643' then
--    return 'CTL_SEC_USER = USER';
--  ELSE
--    return '';
--  END IF; 
--END;
--/
--exec DBMS_RLS.DROP_Policy('DBA643', 'Branches', 'Row_Owner_Sec');
--BEGIN
--  DBMS_RLS.ADD_POLICY (object_schema     => 'DBA643',
--                       object_name       => 'Branches',
--                       policy_name       => 'Row_Owner_Sec',
--                       function_schema   => 'DBA643',
--                       policy_function   => 'Sec_Fun_Cust',
--                       statement_types   => 'SELECT, UPDATE, DELETE, INSERT',
--                       update_check      => TRUE);
--END;
--exec DBMS_RLS.DROP_Policy('DBA643', 'Customers', 'Row_Owner_Sec');
--exec DBMS_RLS.ADD_Policy ('DBA643','Customers','Row_Owner_Sec','DBA643','Sec_Fun_Cust','SELECT, UPDATE, DELETE, INSERT',TRUE);
--exec DBMS_RLS.DROP_Policy('DBA643', 'Financial_Products', 'Row_Owner_Sec');
--exec DBMS_RLS.ADD_Policy ('DBA643','Financial_Products','Row_Owner_Sec','DBA643','Sec_Fun_Cust','SELECT, UPDATE, DELETE, INSERT',TRUE);
--exec DBMS_RLS.DROP_Policy('DBA643', 'Transactions', 'Row_Owner_Sec');
--exec DBMS_RLS.ADD_Policy ('DBA643','Transactions','Row_Owner_Sec','DBA643','Sec_Fun_Cust','SELECT, UPDATE, DELETE, INSERT',TRUE);

-- Step 3: create users: BAdmin and CAdmin
-- Create Table Space
DROP TABLESPACE IA643_TBS INCLUDING CONTENTS AND DATAFILES;
CREATE TABLESPACE IA643_TBS
DATAFILE 'IA643_dat' SIZE 500K
REUSE AUTOEXTEND ON NEXT 300K MAXSIZE 100M;
/
show error;

DROP USER BAdmin CASCADE;
Create User BAdmin identified by BAdmin
    DEFAULT TABLESPACE IA643_TBS
    TEMPORARY TABLESPACE TEMP
    ACCOUNT UNLOCK;
    
DROP USER CAdmin CASCADE;
Create User CAdmin identified by CAdmin
    DEFAULT TABLESPACE IA643_TBS
    TEMPORARY TABLESPACE TEMP
    ACCOUNT UNLOCK;
/
show error;
Grant Connect, Resource To BAdmin, CAdmin;

-- Step 4: create synonyms
DROP PUBLIC SYNONYM Branches;
CREATE PUBLIC SYNONYM Branches For DBA643.Branches;

DROP PUBLIC SYNONYM Customers;
CREATE PUBLIC SYNONYM Customers For DBA643.Customers;

DROP PUBLIC SYNONYM Financial_Products;
CREATE PUBLIC SYNONYM Financial_Products For DBA643.Financial_Products;

DROP PUBLIC SYNONYM Transactions;
CREATE PUBLIC SYNONYM Transactions For DBA643.Transactions;

-- Step 5: Grant privileges to users
Drop Role Bran_Mgr;
Create Role Bran_Mgr;
Grant SELECT, INSERT, UPDATE, DELETE ON Branches To Bran_Mgr;
Grant SELECT, INSERT, UPDATE, DELETE ON Customers To Bran_Mgr;
Grant SELECT, INSERT, UPDATE, DELETE ON Financial_Products To Bran_Mgr;
Grant SELECT, INSERT, UPDATE, DELETE ON Transactions To Bran_Mgr;

Grant Bran_Mgr To BAdmin, CAdmin;
/
set serveroutput on;