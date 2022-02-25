USE master
GO
IF DB_ID('ControlDBDevops') IS NOT NULL
BEGIN
   ALTER DATABASE[ControlDBDevops] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE
   DROP DATABASE[ControlDBDevops]
END
GO
CREATE DATABASE[ControlDBDevops]
GO
ALTER DATABASE[ControlDBDevops] SET ALLOW_SNAPSHOT_ISOLATION ON
GO
USE[ControlDBDevops]
GO
IF EXISTS(SELECT TOP 1 1 FROM sys.sysusers WHERE name = 'usr_dbcontrol')
BEGIN
    DROP USER usr_dbcontrol
END
GO
USE[master]
GO
IF EXISTS(SELECT TOP 1 1 FROM sys.syslogins WHERE name = 'lgn_dbcontrol')
BEGIN
    DROP LOGIN lgn_dbcontrol
END
GO
CREATE LOGIN lgn_dbcontrol WITH PASSWORD = 'lgn_dbcontrol_99', CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF
GO
ALTER SERVER ROLE[dbcreator] ADD MEMBER[lgn_dbcontrol]
ALTER SERVER ROLE[securityadmin] ADD MEMBER[lgn_dbcontrol]
GO
USE[ControlDBDevops]
GO
CREATE USER usr_dbcontrol FOR LOGIN lgn_dbcontrol
GO
USE[ControlDBDevops]
GO
IF OBJECT_ID('dbo.VersionsControl', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.VersionsControl
END
GO
CREATE TABLE dbo.VersionsControl
(
    nuVersion       INT             NOT NULL,
    nmDatabase      SYSNAME         NOT NULL,
    user_execute    SYSNAME         NOT NULL,
    dateTimeExecute DATETIME        NOT NULL,
    nmFile          VARCHAR(100)    NOT NULL,
    scriptContent   VARCHAR(MAX)    NOT NULL,
    CONSTRAINT PKVersionsControl01 PRIMARY KEY(nuVersion, nmDatabase)
)
GO
IF OBJECT_ID('dbo.pReturnMaxVersionByDb', 'P') IS NOT NULL
    DROP PROCEDURE dbo.pReturnMaxVersionByDb
GO
CREATE PROCEDURE dbo.pReturnMaxVersionByDb
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT
SET NOCOUNT ON
SELECT
    nmDatabase, MAX(NuVersion) MaxVersion
FROM dbo.VersionsControl
GROUP BY
    nmDatabase
SET NOCOUNT OFF
END
GO
IF OBJECT_ID('dbo.pReturnAllVersionsForDb', 'P') IS NOT NULL
    DROP PROCEDURE dbo.pReturnAllVersionsForDb
GO
CREATE PROCEDURE dbo.pReturnAllVersionsForDb
    @nmDatabase SYSNAME
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT
    SET NOCOUNT ON
    SELECT
        nuVersion,nmDatabase,user_execute,dateTimeExecute,nmFile,scriptContent
    FROM dbo.VersionsControl
    WHERE nmDatabase = @nmDatabase
    ORDER BY nuVersion
    SET NOCOUNT OFF
END
GO
IF OBJECT_ID('dbo.pRegisterNewVersion','P') IS NOT NULL
    DROP PROCEDURE dbo.pRegisterNewVersion
GO
CREATE PROCEDURE dbo.pRegisterNewVersion
    @nuVersion          INT,
    @nmDatabase         SYSNAME,
    @user_execute       SYSNAME,
    @dateTimeExecute    DATETIME,
    @nmFile             VARCHAR(100),
    @scriptContent      VARCHAR(MAX)
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT
    SET NOCOUNT ON
    INSERT INTO dbo.VersionsControl
        (nuVersion, nmDatabase, user_execute, dateTimeExecute, nmFile, scriptContent)
    VALUES
        (@nuVersion, @nmDatabase, @user_execute, ISNULL(@dateTimeExecute, GETDATE()), @nmFile, @scriptContent)
    SET NOCOUNT OFF
END
GO
IF OBJECT_ID('dbo.pReturnMaxVersionForDb', 'P') IS NOT NULL
    DROP PROCEDURE dbo.pReturnMaxVersionForDb
GO
CREATE PROCEDURE dbo.pReturnMaxVersionForDb
    @nmDatabase SYSNAME
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT
    SET NOCOUNT ON
    SELECT
        MAX(NuVersion) MaxVersion
    FROM dbo.VersionsControl
    WHERE
        nmDatabase = @nmDatabase
    SET NOCOUNT OFF
END
GO
