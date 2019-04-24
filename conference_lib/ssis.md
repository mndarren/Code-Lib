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
```