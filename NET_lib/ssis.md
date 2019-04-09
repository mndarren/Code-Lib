# SSIS Lib
=================<br>
1. SSIS -- SQL Server Integration Service
2. Easily move data from one DB to another DB, cleaning, aggregating, merging
3. Source types: SQL Server DB, Oracle DB, DB2 DB, Excel files
4. Components of SSIS architecture (each component has tasks and containers):
```
   - Control Flow (Stores containers and Tasks) (Brain of SSIS)
   - Data Flow (Source, Destination, Transformations)
   - Event Handler (Sending of messages, Emails)
   - Package Explorer (Offers a single view for all in package)
   - Parameters (User Interaction)
```
5. Components details
```
   1) Control Flow: The components contain containers and tasks which are managed by precedence constraints.
   2) Precedence Constraints: these constraints direct tasks to execute in a predefined order. 
                              also defines the workflow of the entire SSIS package (luigi work).
   3) Task: individual unit of work. same to a method/function. drag and drop to configure them.
   4) Containers: group of tasks together into a unit of work.
                  types: Sequence Container, For loop container (condition expression), Foreach loop container (whole set).
   5) Data Flow: the heart of SSIS, if Control Flow is the brain.
   6) Packages: the notion of a package. Collection of tasks. Control Flow + Data Flow = Package
                can save files onto SQL Server in .msdb or package catalog database.
                .msdb is a structured file very similar to .rdl files to Reporting Services.
   7) Parameters: variable with a few main exceptions. outside package to be passed in for package starting.
```
6. SSIS Tasks Types
```
   Execute SQL Task -- run SQL statement
   Data Flow Task -- read data from source, transform data, write out destinations
   Execute Package Task -- execute packages in same project
   Send Email Task -- notification email
   Script Task -- run .NET or C# coding
```
7. Best Practices
```
   - in-memory pipeline. Everything occurs in memory
   - Try to minimize logged operations
   - Plan for capacity by understanding resource utilization
   - Optimize the SQL lookup transformation, data source, and destination
   - Schedule and distribute it correctly
```
8. How to install SSIS in Ubuntu: https://social.technet.microsoft.com/wiki/contents/articles/52294.sql-server-and-ssis-installation-on-ubuntu.aspx
9. How to install openssl < 1.10: https://askubuntu.com/questions/1000629/how-to-install-openssl-1-0-2-with-default-openssl-1-1-1-on-ubuntu-16-04
10. Windows 10 VM template: https://developer.microsoft.com/en-us/microsoft-edge/tools/vms/

