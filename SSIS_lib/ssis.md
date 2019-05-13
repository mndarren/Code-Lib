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
11. Sequence containers anf For loop
```
	# if some tasks don't depend on each other, they can be moved to a sequence container
	For loop container is a for loop scope in any language code, in which there'll be a script task
```
12. ForEACH loop: R: find files and copy files to another place
```
	Execute SQL task(delete rows from the table which contains file pathnames);
	File System Task(Empty folder)
	Foreach Loop Container, 
		link the previous two task to this, 
		create a var contain FileName,
		Collection: Foreach file enumerator, folder specified and all files, Traverse subfolders
		Variable mappings: FileName index 0
		add execute SQL task(Add file names to SQL table) in container: 
			connection, SQL statement(INSERT INTO tblFile(FileName) VALUES(?)), 
			Parameter mapping: FileName, input nvarchar, 0
		add File system task(copy TXT files):
			destination folder choosing,
			Error: FileName is empty => solved by assigning var FileName to 'test'
		Edit the pipe between the previous task: only TXT: Constaint/Expression
			choose Expression for easier: UPPER(RIGHT(@[User::FileName], 4)) == ".TXT"
		add varialbe: FileCount
		add Expression Task (Increment file): @[User::FileCount] = @[User::FileCount] + 1
	Data Flow task:
		Source from SQL Server table
		Row Count task
	add variable: RowCount
	add script task:
		int FileCount = Convert.ToInt32(Dts.Variables["FileCount"].Value);
		int RowCount = Convert.ToInt32(Dts.Variables["RowCount"].Value);

		MessageBox.Show("File Count " + FileCount + "Row Count " + RowCount);
```
13. ADO Enumerators: R: find the largest Proteges Mentor
```
	Create a variable: Mentors
	Execute SQL Task (load mentors to a var): Result Set(Full set): Result Name = 0
	new vars: FirstName, LastName, Proteges, HighestNumber, HighestName
	add Foreach loop container: Foreach ADO Enumerator, var=Mentors, Mapping
		add script task:Check highest number(config read vars) update highest vars
		add another script task to report the highest number and name
	# Foreach Item Enumerator less useful
	# Loop SQL schemas
	Foreach loop container(loop over tables): Foreach ADO.NET Schema Rowset Enumberator
		New connection: provider = .Net Providers\SqlClient Data Provider\SQL Native Client
		ServerName, database name
		Schema = Tables
		Var mappings: TableName to index 2
		add script task(write out table name)
			string tName = Convert.ToString(Dts.Variables["TableName"].Value);
			System.IO.StreamWriter sw = new System.IO.StreamWriter("c:\\ajb files\\tables.txt", true);
			sw.WriterLine(tName);
			sw.Close();
	# NodeList Enumerators -- Reading XML files
	Foreach loop container (loop over nodes)
		Foreach NodeList Enumerator, DocumentSourceType=FileConnection, source to the xml file
		OuterXpathString=/urlset/url/loc, var mapping: new var 'NodeName' index to 0
		add script task(write out text file)
```
14. Script in SSIS
```
	# 2 types: script task and script component
	new script task: change Main to MakeChoice for EntryPoint
		DialogResult answer = MessageBox.Show("Choose Excel?", "Choice", 
												MessageBoxButtons.YesNo,
												MessageBoxIcon.Question,
												MessageBoxDefaultButton.Button2);
		switch (answer){
			case DialogResult.Yes: 
				Dts.TaskResult = (int)ScriptResults.Success;
				break;
			case DialogResult.No: 
				Dts.TaskResult = (int)ScriptResults.Failure;
				break;
		}
	add 2 sequence containers: one for excel file, the other for SQL
	add another script task:
		// empty var collection
		Variables Wolvariables = null;
		// access 2 var readonly
		Dts.VariableDispenser.LockForRead("User::NumberMentors");
		Dts.VariableDispenser.LockForRead("User::NumberFinalists");
		// access var read-write
		Dts.VariableDispenser.LockForWrite("User::NumberTotal");
		//put vars in var collection
		Dts.VariableDispenser.GetVariables(ref WolVariables);

		int NumberMentors = Convert.ToInt32(WolVariables["NumberMentors"].Value);
		int NumberFinalists = Convert.ToInt32(WolVariables["NumberFinalists"].Value);
		WolVariables["NumberTotal"].Value = NumberMentors + NumberFinalists

		WolVariables.Unlock();
	How to link them?
		right click the pipe for failure side to choose Failure
		right click the pipe to edit to Constraint and True
```
15. Script Component, R: validate the data
```
	create 2 required tables (prerequisite)
	# Note: type can be varchar(MAX) in SQL Server
	add SQL task(delete old data): 
		TRUNCATE TABLE tblGoodContestant
		TRUNCATE TABLE tblBadContestant
	add Data Flow task (import dodgy data)
		new connection in package level, not project level
			flat file connection, column names in the first data row (check or not)
			columns: row delimiter, column delimiter
			Advanced: rename the column name and type
		add Source task(connect the new connection)
		add Script Component task (validate data)
			Import Columns: usage type can be ReadOnly/ReadWrite
			Inputs and Outputs: output column name and type
			Edit script: PreExecute(), PostExecute(), 
				Input0_ProcessInputRow(Input0Buffer Row){
					if (Row.ContextantName_IsNull || Row.Position_IsNull ||
						Row.SeriesNumber_IsNull || Row.MentorName_IsNull){
							Row.Problem = "At least one column is not filled in";
							Row.IsGood = False;
							return;
						}
					// if all are not Null, try to convert number to integer
					uint s = 0;
					uint p = 0;
					try{
						s = Convert.ToUInt32(Row.SeriesNumber);
					} catch {
						s = 0;
					}
					try {
						p = Convert.ToUInt32(Row.Position);
					} catch {
						p = 0;
					}
					// if either still 0, we couldn't convert
					if (s == 0 || p == 0){
						Row.Problem = "The series and position numbers aren't both integers"
						Row.IfGood = false;
						return;
					}

					// after the 2 filters
					Row.intSeries = s;
					Row.intPosition = p;

					Row.MentorName = Row.MentorName.Trim();
					Row.ContestantName = Row.ContestantName.Trim();

					Row.Problem = "";
					Row.IfGood = true;
				}
			add Conditional split task: 
				Output Name = Good data, Condition = [IfGood]==True,
				default name = Bad data
			add destination task to load data to the 2 new tables
```
16. Expression and other constraints
```
	# we can create template version (the pipe/link can be Success/Failure/Completior)
	Completior will always run whether success or failure.
	# setup 3 expresions when union 3 pipe, we can change it from AND to OR (Edit pipe)
```
17. Event-handling and logging
```
	# we can put report task to Event Handlers, this will be the same result to putting it in control flow.
	# review Package explorer
	OnPostExecution, OnVariableValueChanged
	Varibles -> Grid Options -> check "Raise event when variable value changes"
	         -> move varialbe into the task level -> remove any ReadVar in script task
	SSIS -> logging -> text loc -> config 
	# Audit transform, similar with pipe Viewer
	add Audit transform replacing a pipe with viewer
```
18. Error handling
```
	# in Data Conversion task configuration
	Configure Error Output: Fail component to Redirect row
```
19. Parameters and deployment
```
	Right click project in Solution Explorer -> Convert to Package Deployment Model
	In SQL execution task: Expresion Builder "SELECT FinalistName FROM tblFinalist WHERE FinishingPosition<=" +
											  (DT_WSTR_2)@[User::MaxPosition] + "and FinalistName like '%" + @[User::NameContains] +"%'"
	Create Catalog -> Enable automatic execution of IS stored procedure at SQL Server startup
	Right click project -> deploy -> execute package multiple ways (SQL script, directly run)
	# Parameters means external variables for project
	Environments (similar to Catalog in SQL Server)
```
