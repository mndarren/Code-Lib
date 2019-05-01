DECLARE @col_name varchar(50)
DECLARE @Q varchar(255)
DECLARE Loan_Col_Cursor CURSOR FOR
select C.name as column_name 
from sys.all_columns C
left join sys.tables T
on C.object_id = t.object_id
where t.name = 'loan_tbl'
OPEN Loan_Col_Cursor
FETCH NEXT FROM Loan_Col_Cursor INTO @col_name
WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @Q = 'UPDATE loan_tbl SET '+@col_name+' = REPLACE('+@col_name+', ''|'', '''')'
		exec (@Q)
		FETCH NEXT FROM Loan_Col_Cursor INTO @col_name
	END
CLOSE Loan_Col_Cursor
DEALLOCATE Loan_Col_Cursor
GO

--Function to remove all Pipe in a table
DROP PROCEDURE IF EXISTS dbo.removePipeFromTable;
GO
CREATE PROCEDURE dbo.removePipeFromTable (@tbl_name VARCHAR(50)) AS
BEGIN
DECLARE @col_name VARCHAR(50)
DECLARE @Q VARCHAR(255)
DECLARE Loan_Col_Cursor CURSOR FOR
SELECT C.name AS column_name 
FROM sys.all_columns C
LEFT JOIN sys.tables T
ON C.object_id = t.object_id
WHERE t.name = @tbl_name
OPEN Loan_Col_Cursor
FETCH NEXT FROM Loan_Col_Cursor INTO @col_name
WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @Q = 'UPDATE '+@tbl_name+' SET '+@col_name+' = REPLACE('+@col_name+', ''|'', '''')'
		EXEC (@Q)
		FETCH NEXT FROM Loan_Col_Cursor INTO @col_name
	END
CLOSE Loan_Col_Cursor
DEALLOCATE Loan_Col_Cursor
END
GO