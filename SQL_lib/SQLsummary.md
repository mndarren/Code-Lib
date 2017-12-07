# SQL Summary
================
0. Architecture
```
     |-DDL  Definition: Drop, Create, Alter
SQL:-|-DML  Minipulation Insert, Update, DELETE
     |-DCL  CONTROL      GRANT, Revoke
     |-DQ   Query        SELECT from where group by having
```	 
1. ALTER
```
(1) Alter table employee
          Add Constraint Employee_Salary_NN Check (hourly_salary >= 0);
(2) Alter Table Employee
		  Add CONSTRAINT Employee_FK
			  FOREIGN KEY (Comp_ID) REFERENCES Company(Comp_ID);
(3) Alter Session Set NLS_Date_Format = 'Month DD, yyyy';
    SELECT sysdate from dual;  -- default dd-mon-yyyy
(4) Alter System set Audit_Trail = DB Scope = Spfile;
(5) Alter SYSTEM Kill Session ...;
(6) Alter Trigger eval_change_trig DISABLE; -- ENABLE
    ALter Table evaluation DISABLE ALL Triggers;
```
2. Insert update delete
```
(1) Insert into Employee (Emp_ID, First_name, Last_Name)
			Values (201781, 'Joe', 'Smith');
(2) Update Employee
		Set hourly_salary = 26.50
		Where Emp_ID = 201781;
(3) Update Employee
		Set hourly_salary = hourly_salary * 1.1
		Where Comp_ID = (Select Comp_ID
						 from Company
						 where upper(Comp_Name) = 'ABC_Inc');
(4) Update Customer
		Set Cus_FN = replace(Cus_FName, 'Jim', 'James'); -- using functions
(5) Update Invoices
		Set Inv_Due_Date = Decode(Term_ID, 1, Inv_Date+10,
										   2, Inv_Date+20,
										   3, Inv_Date+30, null);
(6) Delete From Employee
		where Comp_ID = 201; -- mark delete, trunc
```
3. Grant priviledges(Object, System)
```
(1) Grant select, update, delete
		ON Employee
		TO Jode, Achen, JSmith;
(2) Grant Update(Comp_Name), INSERT
		ON Company
		TO JChen;
(3) Grant ALL On Employee TO Public; -- all object priviledges
(4) Grant Select ON Company TO JSmith with Grant option; -- with copy right
(5) Revoke Select ON Company From JSmith;   -- Cascading effect JSmith lose grant select to others
(6) Grant Select Any Table to JSmith with admin option;   -- without ON, system level
    -- if revoke any table from JSmith, JSmith still has Admin option
	-- because No cascade for System level 
(7) --Example for System level: Create any table, Insert any table,delete any table,Create session, update any table
(8) -- DBA_TAB_Privs, User_TAB_Privs, Role_Tab_Privs
    -- DBA_Sys_Privs, ..
    -- DBA_Role_Privs, ..
(9) with grant option   --Object privilege
    with admin option   --System privilege
```
4. Function parameters and test
```
(1) IN, OUT, INOUT
(2) Test procedure: CALL/EXEC
(3) Test function: select function() from dual;
```
5. Query using CASE
```
SELECT CompanyName, (CASE WHEN Fax IS NULL
	                      THEN 'No Fax'
	                      ELSE Fax END) AS Fax
FROM Comstomers
WHERE CompanyName LIKE 'A%';
```