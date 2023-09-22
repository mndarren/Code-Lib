Console CMD
==========================
1. SQL Server
```
Add-Migration createdatabase -o Data/Migrations
Update-Database
#Connection String
Data Source=DESKTOP-MPJ6TH1\\SQLEXPRESS;Initial Catalog=IssueDB;Integrated Security=True
```
2. SQL
```
SET IDENTITY_INSERT dbo.Categories ON;

insert into dbo.Categories  values (4, 'Hello', 123, SYSDATETIME());
```
3. Resources
```
https://bootswatch.com/
https://getbootstrap.com/
```