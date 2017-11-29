--@Author: Darrren Xie
--@Due: 10/13/2017 noon

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

-- Create password verify function
conn sys/Jiling#7@localhost/IA643 AS sysdba;
Create or Replace Function dd_pwd_fun(username varchar2, new_pass varchar2,
                                      old_pass varchar2) RETURN BOOLEAN IS
num_same         NUMBER := 0;
new_len          NUMBER := length(new_pass);
user_len         NUMBER := length(username);
ch1              VARCHAR2(3);
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
