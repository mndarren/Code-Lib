# SSIS
================================
1. What? SQL Server Integration Services
```
	Data Integration: combining data from different sources, provide users a unified view
	Workflow: automate maintenance of SQL Server DB and updates ot multidimensional analytical data
	SSIS Package = Control Flow Elements + Data Flow Elements
```
2. Why?
```
Data can be loaded in parallel to many varied destinations;
SSIS removes the need of hard core programmers;
Tight integration with other products of Microsoft;
SSIS is cheaper than most other ETL tools.
```
3. How to create a package in SSIS
```
	Open SQL Server Data Tools -> Create a new project (IS type) -> Create a new SSIS package in Solution Explorer
	-> Connection Manager to create a flat file connection(from bottom pane or solution explorer window) 
	-> Data Flow task first (rename) -> in Data flow choose other source
	-> create SQL Server destination from SSIS connection manager (type OLEDB)
	-> mapping -> right click package to execute package (or run from tool bar after 'Set as StartUp Object')
	-> debug (truncation to ignore)
	# Control Flow
	Command - Delete dat from DB -> Load data from flat files to DB
	# Data review when running package
	Right click the relationship link -> Enable Data Viewer (can edit data viewer columns)
```
4. How to do transform data in SSIS
```
	Sort data from Common Sort (Replacing the relationship link with sort commond)
	Aggregate Common (first choose 'Group by', second choose COUNT) -> Union All common
	# Sampling Common
	Row Sampling (how many rows) -> Union All (selected, unselected)
	Percentage Sampling
	# Combining data
	choose 2 sources -> Union All (if column names are different, map them to common column name)
	# Sync & Async
```
5. How to use variables in SSIS
```
	Data Flow task as source -> add Row Count of Common (Create a variable first before this step)
	Script Task -> ReadOnlyVar/ReadWriteVar, C# -> Edit script
	MessageBox.Show("There are" + Dts.Variables["NumberRows"].Value + " rows");
```
6. Data Types and Data Transformations
```
	# unicode
	varchar -> nvarchar in the table OR
	Add Data Conversion between them -> choose the column Data type to string(DT_STR) -> change mapping
	# SSIS Data types
	SQL Server (SSIS): char varchar (DT_STR), nchar nvarchar (DT_WSTR), ntext (DT_NTEXT), text (DT_TEXT)
	bit(DT_BOOL), tinyint(DT_UI1), smallint(DT_I2), int(DT_I4), bigint(DT_I8)
	real(DT_R4), float(DT_R8), decimal numeric (DT_NUMERIC), money smallmoney (DT_CY)
	datetime smalldatetime (DT_DBTIMESTAMP), date(DT_DBTIME), time(DT_DBTIME2), datetime2(DT_DBTIMESTAMP2), datetimeoffset(DT_DBTIMESTAMPOFFSET)
	uniqueidentifier(DT_GUID), varbinary timestamp binary(DT_BYTES), xml sql_variant(DT_WSTR), image(DT_IMAGE)
```
7. Expressions
```
	 Create variables (CurrentDate) -> Date/Time Function: GETDATE() YEAR(@[User::CurrentDate])
	 Type Casts -> DT_WSTR
	 DATEDIFF("d", @[User::CurrentDate], @[User::XmaxDate])  # d means days
	 # Terary operator
	 String, DATEPART("dw", @[User::CurrentDate]) == 6 || String, DATEPART("dw", @[User::CurrentDate]) == 7
	 ? "It's Weekend" : "It's weekday" # dw means weekday
	 # Expression tasks
	 create variable -> Expression Task (Calculate the message) -> double click 
	 @[User::Message] = @[User::WeekendStatus] + "\n\n There are" + (DT_WSTR, 3)@[User::DaysTillChrismas] + "Days till Chrimas."
	 # show the message in windows
	 script task -> MessageBox.Show(Dts.Variables["Message"].Value.ToString());
```
8. Conditional Splits and Derived Columns
```
	# requirement: split a specific name to another
	Delete record task -> Add Conditional Splits task -> FINDSTRING(upper([Mentor]), 'CHERYL', 1)>0
	LEFT(lower([Mentor]), 6) == "tulisa", Default: Normal People
	# Union All, enable viewer (for test)
	# Convert size to 255, right click the relationship link and edit to see the size
	-> Add Derived Column task with a new column (DT_WSTR, 255)REPLACE([Metor], "Cole", "Wakefield")
	# reunited normal people and Cheryl
	-> Add Union All task (choose the created new column)
```
9. Debugging
```
	# find run time errors, setting breakpoints, watching variable values
	# Error found from output window
	Error: 64-bit driver is not installed run the package in 32-bit mode
	Solution: right click package (properties) -> Debug Options, Run64BitRuntime to False
	# check the variable values
	right click pipe, enable data viewer, edit the viewer, copy data
	# breakpoint
	right click Data Flow task, edit breakpoints
	# watching variables
	View -> windows -> Locals 
	                   OR Watch (watch1) -> variables drag var to watch window 
	                   OR QuickWatch
``` 
10. Lookup Transforms
```
	# create lookup transforms, dealing with the output, Caching data
	Add Lookup task (rename specific action) -> Redirect rows to no match output
	-> to 2 devived columns tasks -> create a var to contain not know mentor number
	For the no match output, add 2 columns: MentorId and Notes (type converter)
	# Caching for better performance: Full cache, partial cache, no cache
	add Data Flow task (load mentor to cache): Metor table -> add Cache Transform task (create new connection manager, index)
	modify the Lookup Transformation task from OLE DB connection manager to Cache connection manager
	# the result will run faster
```
