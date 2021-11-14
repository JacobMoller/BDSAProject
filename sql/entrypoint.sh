#!/bin/bash
database=ProjectBank
wait_time=15s
password=P@55w0rd


# wait for SQL Server to come up
echo importing data will start in $wait_time...
sleep $wait_time
echo importing data...

# run the init script to create the DB and the tables in /table
/opt/mssql-tools/bin/sqlcmd -S 0.0.0.0 -U sa -P $password -i ./init.sql

/opt/mssql-tools/bin/sqlcmd -S 0.0.0.0 -U sa -P $password -i ./table/[User].sql

/opt/mssql-tools/bin/bcp ProjectBank.dbo.[User] in data/[User].csv -c -t',' -F 2 -S 0.0.0.0 -U sa -P $password

/opt/mssql-tools/bin/bcp ProjectBank.dbo.Project in data/Project.csv -c -t',' -F 2 -S 0.0.0.0 -U sa -P $password
