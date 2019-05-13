--Procedure to remove all Pipe in a table
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

--version 2: execute update queries once when query string length gets 1000 characters
DROP PROCEDURE IF EXISTS dbo.removePipeFromTable;
GO
CREATE PROCEDURE dbo.removePipeFromTable (@tbl_name VARCHAR(50)) AS
BEGIN
DECLARE @col_name VARCHAR(50)
DECLARE @temp_q VARCHAR(100)
DECLARE @Q VARCHAR(1000) = 'UPDATE '+@tbl_name+' SET '
--get table column name cursor
DECLARE Table_Col_Cursor CURSOR FOR
SELECT C.name AS column_name 
FROM sys.all_columns C
LEFT JOIN sys.tables T
ON C.object_id = t.object_id
WHERE t.name = @tbl_name
--open cursor
OPEN Table_Col_Cursor
FETCH NEXT FROM Table_Col_Cursor INTO @col_name
--build update query string, execute once when length of string gets 1000 characters
WHILE @@FETCH_STATUS = 0
BEGIN
	SET  @temp_q = '['+@col_name+'] = REPLACE(['+@col_name+'], ''|'', ''''),'
	IF LEN(@Q + @temp_q) >= 1000
		BEGIN
			SET @Q = LEFT(@Q, LEN(@Q)-1)
			EXEC (@Q)
			SET @Q = 'UPDATE '+@tbl_name+' SET '
		END
	SET @Q = @Q + @temp_q
	FETCH NEXT FROM Table_Col_Cursor INTO @col_name
END
--last execute the update query string
IF LEN(@Q) > 0
	BEGIN
		SET @Q = LEFT(@Q, LEN(@Q)-1)
		EXEC (@Q)
	END
--close cursor
CLOSE Table_Col_Cursor
DEALLOCATE Table_Col_Cursor
END
GO

--execute procedure
EXEC dbo.removePipeFromTable loan_tbl;
GO

--Procedure to dedup records by key column, keeping the 1st record of duplicates
DROP PROCEDURE IF EXISTS dbo.dedupRecords
GO
CREATE PROCEDURE dbo.dedupRecords (@tbl_name VARCHAR(50), @order_column VARCHAR(50)) AS
DECLARE @Q VARCHAR(512)
BEGIN
    SET @Q = 'WITH CTE AS(
       SELECT *, RN = ROW_NUMBER()OVER(PARTITION BY '+@order_column+' ORDER BY '+@order_column+')
       FROM '+@tbl_name+')
       DELETE FROM CTE WHERE RN > 1'
    EXEC (@Q)
END
GO

--execute procedure
EXEC dbo.dedupRecords @tbl_name='any_tbl', @order_column='KeyCol'
GO

--Procedure to create a view by adding a new column
DROP PROCEDURE IF EXISTS dbo.createViewByAddNewColumn
GO
CREATE PROCEDURE dbo.createViewByAddNewColumn (@tbl_name VARCHAR(50), @base_col VARCHAR(50), @prepend VARCHAR(20)) AS
DECLARE @Q VARCHAR(512)
DECLARE @view_name VARCHAR(60) = @tbl_name+'_v'
DECLARE @new_col_name VARCHAR(60) = @base_col+'_custom'
BEGIN
   SET @Q = 'CREATE OR ALTER VIEW '+@view_name+' AS
             SELECT '''+@prepend+'''+'+@base_col+' AS '+@new_col_name+', * FROM '+@tbl_name
   EXEC (@Q)
END
GO

--execute create view procedure
exec dbo.createViewByAddNewColumn @tbl_name='loan_tbl', @base_col='NoteNb', @prepend='LN'

--Data format change pieces
--datetime
CASE LEN(SomeDate) WHEN 0 THEN '' ELSE CAST(FORMAT(CAST(SomeDate AS datetime), 'M/d/yyyy H:mm') AS VARCHAR) END AS SomeDate
--Delete leading zeros
CASE WHEN LEN(SomeCode) = 0 THEN CAST('' AS Varchar(20)) ELSE CAST(CAST(SomeCode AS INTEGER) AS VARCHAR(50)) END AS SomeCode
--Add double quote at the 2 ends if " or , in the value, double double quotes, empty -> 40 spaces
CASE LEN(SomeTitle) WHEN 0 THEN '                                        ' ELSE 
     CASE WHEN CHARINDEX('"', SomeTitle) > 0 OR CHARINDEX(',', SomeTitle) > 0 THEN '"' + REPLACE(SomeTitle, '"', '""') + '"' ELSE SomeTitle END 
     END AS SomeTitle
--Float to Varchar type
CASE LEN(SomeValue) WHEN 0 THEN '' ELSE CAST(CAST(SomeValue AS FLOAT) AS VARCHAR) END AS SomeValue
--add 2 new line feed
CASE LEN([SomeThing]) WHEN 0 THEN '' ELSE [SomeThing]+CHAR(10)+CHAR(10) END
