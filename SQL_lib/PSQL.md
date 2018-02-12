#PSQL Basic Commands
==========================
1. Install pgSQL: sudo apt-get install postgresql-9.6 <br>
		sudo apt-get install pgadmin3
2. Setting password<br>
	`sudo -u postgres psql postgres`
	`\password postgres`
	`\q`
3. Allowing local connection
	```
	/etc/postgresql/9.6/main/pg_hba.conf
	local   all   all     md5
	host    all   all   ::1/128 md5
	sudo service postgresql restart
	```
4. Allowing remote connection
	```
	/etc/postgresql/9.6/main/pg_hba.conf
	local   all   all     md5 (changed to the following)
	host    all   all   0.0.0.0/0   trust
	/etc/postgresql/9.6/main/postgresql.conf
	listen_addresses = '*'
	sudo service postgresql restart
	```