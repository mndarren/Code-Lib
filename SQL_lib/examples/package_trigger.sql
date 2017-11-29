-- @Author: Darren (Zhao) Xie
-- @Date: 09/25/2017

-- Q1. Create a package
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

-- Q2 Trigger
-- trigger to update credit if overpayment
CREATE OR REPLACE TRIGGER Over_Payment_Trig
   BEFORE UPDATE OF PAYMENT_TOTAL ON INVOICES
   FOR EACH ROW
WHEN (new.payment_total > old.payment_total)
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
