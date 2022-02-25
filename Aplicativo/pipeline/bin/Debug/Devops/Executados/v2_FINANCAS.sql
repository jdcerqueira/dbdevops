USE [FINANCAS]
GO

IF EXISTS(SELECT TOP 1 1 FROM sys.sysusers WHERE name = 'usr_financas')
BEGIN
	DROP USER [usr_financas]
END
GO

IF DATABASE_PRINCIPAL_ID('Client') IS NOT NULL
BEGIN
	DROP ROLE [Client]
END

IF SCHEMA_ID('lancamentos') IS NOT NULL
BEGIN
	DROP SCHEMA [lancamentos]
END
GO

USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM sys.syslogins WHERE name = 'lgn_financas')
BEGIN
	DROP LOGIN [lgn_financas]
END
GO

CREATE LOGIN [lgn_financas] WITH 
	PASSWORD = 'lgn_financas', 
	CHECK_POLICY = OFF, 
	CHECK_EXPIRATION = OFF, 
	DEFAULT_DATABASE = [FINANCAS]
GO

USE [FINANCAS]
GO

CREATE USER [usr_financas] FOR LOGIN [lgn_financas]
GO

CREATE SCHEMA [lancamentos] AUTHORIZATION [dbo]
GO

CREATE ROLE [Client] AUTHORIZATION [dbo]
GO

ALTER ROLE [Client] ADD MEMBER [usr_financas]
GO

GRANT EXECUTE ON SCHEMA :: [lancamentos] TO [Client]
GO