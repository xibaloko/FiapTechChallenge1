#!/bin/bash
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "SqlServer2019!" -d master -i /tmp/CriarBanco.sql
