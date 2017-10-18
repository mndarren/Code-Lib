-- Example 1:
-- Create TABLESPACE, PROFILE, user, and password verify function

-- Create Table Space
DROP TABLESPACE IA643_TBS INCLUDING CONTENTS AND DATAFILES;
CREATE TABLESPACE IA643_TBS
DATAFILE 'IA643_dat' SIZE 500K
REUSE AUTOEXTEND ON NEXT 300K MAXSIZE 100M;
/
show error;

-- Create profile
Drop Profile Develop_prof CASCADE;
Create Profile Develop_prof Limit
    Sessions_per_user   unlimited
    Cpu_per_session     unlimited
    Cpu_per_call        6000    --60 seconds
    Connect_time        240     --4 hours
    Private_SGA         20k
    Failed_login_attempts 6
    Password_life_time    180   --6 months
    Password_reuse_time   15    --15 days
    Password_grace_time   4;    --4 days
/    
show error;

-- Create User
DROP USER JSmith CASCADE;
CREATE USER JSmith IDENTIFIED BY JSmith
    DEFAULT TABLESPACE IA643_TBS
    TEMPORARY TABLESPACE TEMP
    ACCOUNT UNLOCK
    PROFILE Develop_Prof;
/ 
DROP USER SHouston CASCADE;
CREATE USER SHouston IDENTIFIED BY SHouston
    DEFAULT TABLESPACE IA643_TBS
    TEMPORARY TABLESPACE TEMP
    ACCOUNT UNLOCK
    PROFILE Develop_Prof;
/
DROP USER SClark CASCADE;
CREATE USER SClark IDENTIFIED BY SClark
    DEFAULT TABLESPACE IA643_TBS
    TEMPORARY TABLESPACE TEMP
    ACCOUNT UNLOCK
    PROFILE Develop_Prof;
/
DROP USER BJohnson CASCADE;
CREATE USER BJohnson IDENTIFIED BY BJohnson
    DEFAULT TABLESPACE IA643_TBS
    TEMPORARY TABLESPACE TEMP
    ACCOUNT UNLOCK
    PROFILE Develop_Prof;
/
show error;

GRANT CONNECT, RESOURCE TO JSmith, SHouston, SClark, BJohnson;

-- Create password verify function (should use SYS account)
conn sys/Jiling#7@localhost/IA643 AS sysdba;
Create or Replace Function dd_pwd_fun(username varchar2, new_pass varchar2,
                                      old_pass varchar2) RETURN BOOLEAN IS
new_len          NUMBER := length(new_pass);
user_len         NUMBER := length(username);
ch1              VARCHAR2(3);     -- cannot compare with different type
cnt_digit        NUMBER := 0;
cnt_lower        NUMBER := 0;
cnt_upper        NUMBER := 0;
digit_array varchar2(10) := '0123456789';
lower_array varchar2(26) := 'abcdefghijklmnopqrstuvwxyz';
upper_array varchar2(26) := 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';

BEGIN
    -- at least 8 chars
    IF new_len < 8 THEN
       raise_application_error(-20020, 'Password length less than 8');
       RETURN FALSE;
    END IF;
    -- at least 1 lower, 1 upper, 1 digit
    FOR i IN 1..new_len LOOP
       ch1 := SUBSTR(new_pass, i, 1);
       IF INSTR(digit_array, ch1,1,1) > 0 THEN
          cnt_digit := cnt_digit + 1;
       ELSIF INSTR(lower_array, ch1,1,1) > 0 THEN
          cnt_lower := cnt_lower + 1;
       ELSIF INSTR(upper_array, ch1,1,1) > 0 THEN
          cnt_upper := cnt_upper + 1;
       END IF;
    END LOOP;
    IF cnt_digit < 1 or cnt_lower < 1 or cnt_upper < 1 THEN
       raise_application_error(-20044,'at least 1 upper letter, at least 1 lower letter '
                        || 'and at least 1 digit!');
       RETURN FALSE;
    END IF;
    /* not contain 3 or more same consecutive chars. case-insensitive */
    IF user_len > 2 THEN
       FOR i IN 1..(new_len - 2) LOOP
          ch1 := SUBSTR(new_pass, i,3);
          IF REGEXP_INSTR(UPPER(username), UPPER(ch1)) > 0 THEN
             raise_application_error(-20043,'New password CANNOT '
                        || 'contain 3 or more consecutive '
                        || 'characters of the user name!');
             RETURN FALSE;
          END IF;
       END LOOP;     
    END IF;

    RETURN TRUE;
END dd_pwd_fun;
/
show error;

GRANT EXECUTE ON dd_pwd_fun TO DBA643;

-- alter profile
conn DBA643/Dubai9$@localhost/IA643;
ALTER PROFILE Develop_Prof LIMIT
      PASSWORD_VERIFY_FUNCTION dd_pwd_fun;
/

set serveroutput on;
--testing

--Example 2:
-- Create Package, procedure, function and trigger
-- Make sure correct System date format
BEGIN
  EXECUTE IMMEDIATE 'ALTER SESSION SET NLS_DATE_FORMAT = "DD-MON-YY"';
EXCEPTION
  WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('');
END;
/

-- Package Specification
CREATE OR REPLACE PACKAGE Acme_Accounts AS
    PROCEDURE Update_Payment (invoice_ID_num NUMBER, payment_amount NUMBER);
    PROCEDURE Late_Payment_Days (invoice_ID_num NUMBER);
    FUNCTION Return_Balance_Due (invoice_ID_num NUMBER) RETURN NUMBER;
END Acme_Accounts;
/
show error;
-- Package Body
CREATE OR REPLACE PACKAGE BODY Acme_Accounts AS
   -- update invoice with total payment and payment date
   PROCEDURE Update_Payment (invoice_ID_num NUMBER, payment_amount NUMBER) IS
   BEGIN
      UPDATE INVOICES
         SET PAYMENT_TOTAL = payment_amount + PAYMENT_TOTAL,
             PAYMENT_DATE = (SELECT SYSDATE FROM DUAL)
         WHERE INVOICE_ID = invoice_ID_num;
   END Update_Payment;
   
   -- display late payment days
   PROCEDURE Late_Payment_Days (invoice_ID_num NUMBER) IS
      due_day DATE;
      pay_day DATE;
      late_days NUMBER(5);
   BEGIN
      SELECT INVOICE_DUE_DATE, PAYMENT_DATE
      INTO due_day, pay_day
      FROM INVOICES
      WHERE INVOICE_ID = invoice_ID_num;
      
      IF pay_day is null THEN
         DBMS_OUTPUT.PUT_LINE('NO PAYMENT!');
		     pay_day := SYSDATE;
	    END IF;
      late_days := trunc((((86400*(pay_day - due_day))/60)/60)/24);
      IF late_days > 0 THEN
            DBMS_OUTPUT.PUT_LINE('LATE PAYMENT days = '||late_days||
                                 ', INVOICE ID = ' || invoice_ID_num);
      END IF;
   END Late_Payment_Days;
   
   -- return balance due
   FUNCTION Return_Balance_Due (invoice_ID_num NUMBER) RETURN NUMBER IS
      balance_due NUMBER(10,2);
      should_pay NUMBER(10,2);
      paid_total NUMBER(10,2);
      credited_total NUMBER(10,2);
   BEGIN
      SELECT INVOICE_TOTAL, CREDIT_TOTAL, PAYMENT_TOTAL
      INTO should_pay, credited_total, paid_total
      FROM INVOICES
      WHERE INVOICE_ID = invoice_ID_num;
         
      balance_due := should_pay - paid_total - credited_total;
      RETURN balance_due;
   END Return_Balance_Due;

END Acme_Accounts;
/
show error;

-- trigger to update credit if overpayment (Event, Restriction, Action)
-- Timing: before; Type: Row trigger; Event: update
CREATE OR REPLACE TRIGGER Over_Payment_Trig
   BEFORE UPDATE OF PAYMENT_TOTAL ON INVOICES
   FOR EACH ROW
WHEN (new.payment_total > old.payment_total) --restriction
DECLARE
   balance   NUMBER(10,2);
BEGIN
   balance := :new.CREDIT_TOTAL + :new.PAYMENT_TOTAL - :new.INVOICE_TOTAL;
   IF balance > 0 THEN
      dbms_output.put_line('OVERPAYMENT WARNING: INVOICE_ID = '
                           || :new.INVOICE_ID);
      :new.PAYMENT_TOTAL := :old.INVOICE_TOTAL;
      :new.CREDIT_TOTAL := balance;
   END IF;
END Over_Payment_Trig;
/
show errors;
SET SERVEROUTPUT ON;

