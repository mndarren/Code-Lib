# SQL DBA Summary
===========================================

1. PACKAGE (spec and body)
   ```
   Consist of Cursors, variables, types, exceptions and subprograms.
   ```
2. Trigger (3 parts: event, timing&restriction, action)
   ```
   Event: 1) on table: insert, update, delete
          2) on any object: create, alter, drop
   	      3) A DB startup or shutdown
   	      4) A specific error message or any error message
   	      5) A user login or logoff
   Types: Row trigger and statement trigger
   ```
3. Privs
   ```
   DBA_TAB_Privs     #check Object Privs on a role or user
   DBA_Sys_Privs     #check System Privs on a role or user
   DBA_Role_Privs    #check Roles granted to a user
   Ex: **SELECT * FROM DBA_TAB_PRIVS WHERE grantee='JSMITH';**
   User_TAB_Privs    #check Object Privs on current user
   User_Sys_Privs    #check System Privs on current user
   User_Role_Privs   #check Roles on current user
   Role_Tab_Privs    #check Object Privs in a role for current user
   Role_Sys_Privs    #check System Privs in a role for current user
   Role_Role_Privs   #check Roles in role for current user
   ```
4. PL/SQL (Procedure Language)
   ```
    Type: char, varchar, number, Date, binary_Integer
	Large Object Data type: CLOB, BLOB, BFILE
	Row, Long Row, NLOB
	%Type: Assumes data type of data field
	%RowType: Assumes data tyep of database row
	Wage   Employee.Salary%TYPE
	EmployRec  Employee%ROWTYPE
   ```
5. Set Serveroutput on, loop
   ```
	While()
	LOOP
		...
	End LOOP;
	For n in 1...20
	LOOP
		...
	END LOOP;
   ```
6. Cursor (Implicit Cursor and explicit cursor)
   ```
	--4 attributes: SQL%isOpen, NotFound, Found, RowCount
    --3 store procedure units: function, procedure, trigger
		--parameters: input(value, default) and output(reference)
   ```
7. Trigger(new, old, when, if)
   ```
	--new old are global var in memory
	--when -> new, if -> :new
	--:= is assignment operator
   ```
8. SEQUENCE  
   ```
   Create Sequence Vendor_seq
	   Start with 2000 increment by 2 max 10000;
   ```
9. Trigger example
   ```
   Create or Replace trigger U_State_Trig
   Before Insert or Update of State on Vendor_seq
   For each row
   When (new.State != upper(new.State))
   Begin
   	:new.State := upper(:new.State);
   End;
   /
   show error;
   
   Create or replace trigger Vendor_ID_Trig
   Before Insert ON Vendor
   for each row
   if :new.V_Code is null then
   BEGIN
   	select Vendor_Seq.NEXTVAL
   	into :new.V_Code
   	from dual;
   End;
   end if;
   /
   show error;
   ```
10. Difference between sys_context('USERENV', 'CURRENT_USER') and sys_context('USERENV', 'SESSION_USER')
   ```
   CURRENT_SUER might be changed in the same session;
   SESSION_USER will be the same in the same session.
   ```
11. show objects and kill session
   ```
   select * from Obj;    #show all objects including table index, function, procedure, etc
   select s.sid, s.serial#, s.username, s.program From v$session s;
   ALTER SYSTEM KILL SESSION 'sid.serial#' IMMEDIATE;
   ```
12. Auditing
   ```
   1) FGA (Fine-Gained Auditing) -- only select, insert, update and delete types for FGA
      Stored: SYS.FGA_LOG$, View: DBA_FGA_Audit_Trail
   EXEC DBMS_FGA.add_policy(object_Schema   => 'DBA643',
                            object_name     => 'Cust', 
                            policy_name     => 'AUDIT_SELECT_Policy', 
                            audit_column    => 'Cus_no,Cus_CREDIT_LIMIT', 
                            audit_condition => 'CUS_CREDIT_LIMIT >= 1000', 
                            audit_column_opts => DBMS_FGA.ALL_COLUMNS,   -- by default
                            statement_types => 'SELECT');  
   2) Standard Auditing
      Stored: SYS.Aud$, View: DBA_Audit_Trail
      Currently auditing: DBA_stmt_Audit_Opts, DBA_obj_Audit_Opts, DBA_Pri_Audit_Opts
      i) Statement (no object specified, DDL)
         AUDIT Create Table;
         AUDIT Create Trigger;
         AUDIT Create View By Scott By Access; 
         AUDIT Create Session whenever not successful;
         NOAUDIT Create Table;
         NOAUDIT Create View By Scott;
      ii) Object (no user specified, DML & DQ)
         Audit SELECT On Scott.Emp By Access Whenever Not Successful;
         Audit UPDATE On Scott.Emp By Access;
         Audit INSERT On Scott.Emp By Access;
         Audit DELETE On Scott.Emp By Access;
         NoAudit SELECT On Scott.Emp Whenever Not Successful;
         NoAudit UPDATE ON Scott.Emp
      iii) Privilege (DCL & DBA events)
      	 AUDIT Create Any Table by Scott By Access;
         AUDIT GRANT ANY OBJECT PRIVILEGE By Access;
         AUDIT GRANT ANY PRIVILEGE by Allen By Access;
         AUDIT GRANT ANY ROLE By Access;
         noAUDIT Create Any Table by Scott;
         noAUDIT GRANT ANY OBJECT PRIVILEGE;
         noAUDIT GRANT ANY ROLE;
   3) User-defined Auditing
   4) Where to store the audit data (OS, XML, DB, XML_Extended, DB_Extended)
      ALTER SYSTEM set audit_trail = DB scope = SPfile;
   ```
13. 