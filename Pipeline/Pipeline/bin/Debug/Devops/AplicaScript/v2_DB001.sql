USE DB001;
IF EXISTS(SELECT TOP 1 1 FROM sys.sysusers WHERE name = 'dbuser')
	DROP USER dbuser;

USE master;
IF EXISTS(SELECT TOP 1 1 FROM sys.syslogins WHERE name = 'dbuser')
	DROP LOGIN dbuser;

CREATE LOGIN dbuser WITH PASSWORD = 'dbuser', CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF;

USE DB001;
CREATE USER dbuser FOR LOGIN dbuser;