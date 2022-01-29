USE XPTO;

IF EXISTS(SELECT TOP 1 1 FROM sys.sysusers WHERE name = 'xptoacesso')
	DROP USER xptoacesso;

IF EXISTS(SELECT TOP 1 1 FROM sys.syslogins WHERE name = 'xptoacesso')
	DROP LOGIN xptoacesso;

CREATE LOGIN xptoacesso WITH PASSWORD = 'xptoacesso', CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF;
CREATE USER xptoacesso FOR LOGIN xptoacesso;

PRINT 'Usuario criado com sucesso na base e no servidor.';
