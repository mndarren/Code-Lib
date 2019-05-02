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