-- IA 643 Database Security and Auditing
-- Virtual Private Database
-- Fall 2017
-- Dr. Jim Q. Chen
--run 3 script file (create_tables.sql -> pre_solution.sql -> context_var_setting.sql)

Drop table Transactions;
Drop Table Financial_Products;
Drop Table Branch_Managers;
Drop Table Customers;
Drop Table Branches;

CREATE TABLE Branches (
       Branch_ID           NUMBER CONSTRAINT Company_Pk PRIMARY KEY,
       Branch_Name         VARCHAR2(30) NOT NULL,
       Street_Address       VARCHAR2(50) NOT NULL,
       CITY                 VARCHAR2(30) NOT NULL,
       STATE                VARCHAR2(2) NOT NULL,
       ZIPCODE              VARCHAR2(9) NOT NULL,
       PHONE                VARCHAR2(15) NOT NULL,
       FAX                  VARCHAR2(15) NOT NULL,
       EMAIL                VARCHAR2(50) NOT NULL,
       URL                  VARCHAR2(50) NOT NULL,
       STATUS               VARCHAR2(10) NOT NULL,
       CTL_SEC_USER         VARCHAR2(30) NULL,
       CTL_SEC_LEVEL        NUMBER NULL);


INSERT INTO Branches VALUES(
20141, 'Cal Campus', '740 South 4th Ave.', 'Berkeley','CA', '94701','5103084992', 
'5103084896','Help@StarBank.com', 'www.StarBank.com','A','BADMIN', 3);

INSERT INTO Branches VALUES(
20142, 'Bay EasyBanking', '278 W. Division Street', 'San Francisco','CA', '56301','4153184997', 
'4153185897','Help@StarBank.com', 'www.StarBank.com','A','CADMIN', 3);

CREATE TABLE Customers (
       Customer_ID          NUMBER CONSTRAINT Customer_PK PRIMARY KEY,
       Branch_ID           NUMBER CONSTRAINT Customer_Bran_ID_FK REFERENCES Branches (Branch_ID),
       TAX_ID               NUMBER NOT NULL,
       FIRST_NAME           VARCHAR2(20) NOT NULL,
       LAST_NAME            VARCHAR2(20) NOT NULL,
       DOB		    Date NOT NULL,
       Sex		    CHAR(1),
       Marital_Status	    CHAR(1),
       CTL_SEC_USER         VARCHAR2(30) NULL,
       CTL_SEC_LEVEL        NUMBER NULL
);

INSERT INTO Customers VALUES(
1001, 20141, '15281','John','Killmer', '12-Mar-1996','M', 'M', 'BADMIN', 4);

INSERT INTO Customers VALUES(
1002, 20142, '19271','Stephanie','Porwoll','23-Jun-1992', 'F', 'S' , 'CADMIN', 4);


CREATE TABLE Branch_Managers (
       Manager_ID                NUMBER CONSTRAINT BM_PK PRIMARY KEY,
       Branch_ID  	         NUMBER NULL,
       FIRST_NAME           VARCHAR2(20) NOT NULL,
       LAST_NAME            VARCHAR2(20) NOT NULL,
       SYSTEM_USERNAME      VARCHAR2(30) NOT NULL,
       CTL_SEC_USER         VARCHAR2(20) NULL,
       CTL_SEC_LEVEL        NUMBER NULL
);
INSERT INTO Branch_Managers VALUES(
200, 20141, 'Bob','Wright','BAdmin', 'DBA643', 5);
INSERT INTO Branch_Managers VALUES(
201, 20142, 'Clark','Murray','CAdmin', 'DBA643', 5);
INSERT INTO Branch_Managers VALUES(
202, 20141, 'Patty','Wilson','PAdmin', 'DBA643', 5);
INSERT INTO Branch_Managers VALUES(
203, 20142, 'Kyle','Weber','KAdmin', 'DBA643', 5);



CREATE TABLE Financial_Products (
       FP_ID                NUMBER CONSTRAINT FP_Pk PRIMARY KEY,
       Customer_ID          NUMBER CONSTRAINT FP_FK REFERENCES Customers,
       FP_Type		    CHAR(3) NOT NULL,
       Start_Date	    Date NOT NULL,
       End_Date             DATE,
       CTL_SEC_USER         VARCHAR2(30) NULL,
       CTL_SEC_LEVEL        NUMBER NULL
);

INSERT INTO Financial_Products VALUES(
110001, 1001, 'CHK','1-Jul-12', NULL, 'BADMIN', 4);
INSERT INTO Financial_Products VALUES(
220002, 1002,'SAV', '8-Apr-13', NULL,  'CADMIN', 4);


CREATE TABLE Transactions (
       Tran_ID               NUMBER CONSTRAINT DailyHour_PK PRIMARY KEY,
       FP_ID                NUMBER CONSTRAINT DAILYHOURS_FK REFERENCES Financial_Products,
       Trans_Date	    Date NOT NULL,
       Trans_Type	    CHAR(1) NOT NULL,
       Trans_Amount         NUMBER(12,2),
       Trans_Location       VARCHAR2(10),
       CTL_SEC_USER         VARCHAR2(30) NULL,
       CTL_SEC_LEVEL        NUMBER NULL
);

INSERT INTO Transactions VALUES(
800,110001, '2-Oct-17','W',25,'CSU','BADMIN', 4);
INSERT INTO Transactions VALUES(
811,110001, '23-Sep-17', 'D', 5200, 'Bay','BADMIN', 4);

INSERT INTO Transactions VALUES(
820,220002, '2-Aug-17','D', 4000, 'Oakland','CADMIN', 4);
INSERT INTO Transactions VALUES(
830,220002, '4-Nov-17','D', 3350,'Bay', 'CADMIN', 4);

