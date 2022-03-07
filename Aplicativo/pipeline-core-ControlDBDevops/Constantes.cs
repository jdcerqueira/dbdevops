using System;
using System.Text;

namespace pipeline_core_ControlDBDevops
{
    public static class Constantes
    {
        //Dados da base versionadora
        public static String loginControladora = "lgn_dbcontrol";
        public static String usuarioControladora = "usr_dbcontrol";
        public static String senhaControladora = "lgn_dbcontrol_99";
        public static String baseControladora = "ControlDBDevops";

        public static String selectIdDatabase = $"SELECT ISNULL(DB_ID('{baseControladora}'),0) Retorno";
        public static String arquivoBaseVersionadora = @"\01_DatabaseControlVersion_Create.sql";

        public static String scriptBaseVersionadora()
        {
            StringBuilder retorno = new StringBuilder();

            // Cria a base de dados
            retorno.AppendLine("USE master");
            retorno.AppendLine("GO");
            retorno.AppendLine($"IF DB_ID('{baseControladora}') IS NOT NULL");
            retorno.AppendLine("BEGIN");
            retorno.AppendLine($"   ALTER DATABASE[{baseControladora}] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE");
            retorno.AppendLine($"   DROP DATABASE[{baseControladora}]");
            retorno.AppendLine("END");
            retorno.AppendLine("GO");
            retorno.AppendLine($"CREATE DATABASE[{baseControladora}]");
            retorno.AppendLine("GO");
            retorno.AppendLine($"ALTER DATABASE[{baseControladora}] SET ALLOW_SNAPSHOT_ISOLATION ON");
            retorno.AppendLine("GO");

            // Cria o usuário aplicador dos scripts
            retorno.AppendLine("USE[master]");
            retorno.AppendLine("GO");

            retorno.AppendLine($"IF EXISTS(SELECT TOP 1 1 FROM sys.syslogins WHERE name = '{loginControladora}')");
            retorno.AppendLine("BEGIN");
            retorno.AppendLine($"    DROP LOGIN {loginControladora}");
            retorno.AppendLine("END");
            retorno.AppendLine("GO");

            retorno.AppendLine($"CREATE LOGIN {loginControladora} WITH PASSWORD = '{senhaControladora}', CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF");
            retorno.AppendLine("GO");

            retorno.AppendLine($"ALTER SERVER ROLE[dbcreator] ADD MEMBER[{loginControladora}]");
            retorno.AppendLine($"ALTER SERVER ROLE[securityadmin] ADD MEMBER[{loginControladora}]");
            retorno.AppendLine("GO");

            retorno.AppendLine($"USE[{baseControladora}]");
            retorno.AppendLine("GO");

            retorno.AppendLine($"CREATE USER {usuarioControladora} FOR LOGIN {loginControladora}");
            retorno.AppendLine("GO");
            retorno.AppendLine("CREATE ROLE AplicaScripts AUTHORIZATION [dbo]");
            retorno.AppendLine("GO");
            retorno.AppendLine($"ALTER ROLE AplicaScripts ADD MEMBER {usuarioControladora}");
            retorno.AppendLine("GO");
            retorno.AppendLine("GRANT EXECUTE TO AplicaScripts");
            retorno.AppendLine("GO");

            // Cria os objetos
            // Tabela de versionamento
            retorno.AppendLine($"USE[{baseControladora}]");
            retorno.AppendLine("GO");
            retorno.AppendLine("IF OBJECT_ID('dbo.VersionsControl', 'U') IS NOT NULL");
            retorno.AppendLine("BEGIN");
            retorno.AppendLine("    DROP TABLE dbo.VersionsControl");
            retorno.AppendLine("END");
            retorno.AppendLine("GO");

            retorno.AppendLine("CREATE TABLE dbo.VersionsControl");
            retorno.AppendLine("(");
            retorno.AppendLine("    nuVersion       INT             NOT NULL,");
            retorno.AppendLine("    nmDatabase      SYSNAME         NOT NULL,");
            retorno.AppendLine("    user_execute    SYSNAME         NOT NULL,");
            retorno.AppendLine("    dateTimeExecute DATETIME        NOT NULL,");
            retorno.AppendLine("    nmFile          VARCHAR(100)    NOT NULL,");
            retorno.AppendLine("    scriptContent   VARCHAR(MAX)    NOT NULL,");
            retorno.AppendLine("    CONSTRAINT PKVersionsControl01 PRIMARY KEY(nuVersion, nmDatabase)");
            retorno.AppendLine(")");
            retorno.AppendLine("GO");

            // Procedures
            // Retorna a última versão aplicada agrupada por base de dados.
            retorno.AppendLine("IF OBJECT_ID('dbo.pReturnMaxVersionByDb', 'P') IS NOT NULL");
            retorno.AppendLine("    DROP PROCEDURE dbo.pReturnMaxVersionByDb");
            retorno.AppendLine("GO");
            retorno.AppendLine("CREATE PROCEDURE dbo.pReturnMaxVersionByDb");
            retorno.AppendLine("AS");
            retorno.AppendLine("BEGIN");
            retorno.AppendLine("    SET TRANSACTION ISOLATION LEVEL SNAPSHOT");
            retorno.AppendLine("SET NOCOUNT ON");
            retorno.AppendLine("SELECT");
            retorno.AppendLine("    nmDatabase, MAX(NuVersion) MaxVersion");
            retorno.AppendLine("FROM dbo.VersionsControl");
            retorno.AppendLine("GROUP BY");
            retorno.AppendLine("    nmDatabase");
            retorno.AppendLine("SET NOCOUNT OFF");
            retorno.AppendLine("END");
            retorno.AppendLine("GO");

            // Retorna todas as versões aplicadas em uma base de dados
            retorno.AppendLine("IF OBJECT_ID('dbo.pReturnAllVersionsForDb', 'P') IS NOT NULL");
            retorno.AppendLine("    DROP PROCEDURE dbo.pReturnAllVersionsForDb");
            retorno.AppendLine("GO");
            retorno.AppendLine("CREATE PROCEDURE dbo.pReturnAllVersionsForDb");
            retorno.AppendLine("    @nmDatabase SYSNAME");
            retorno.AppendLine("AS");
            retorno.AppendLine("BEGIN");
            retorno.AppendLine("    SET TRANSACTION ISOLATION LEVEL SNAPSHOT");
            retorno.AppendLine("    SET NOCOUNT ON");
            retorno.AppendLine("    SELECT");
            retorno.AppendLine("        nuVersion,nmDatabase,user_execute,dateTimeExecute,nmFile,scriptContent");
            retorno.AppendLine("    FROM dbo.VersionsControl");
            retorno.AppendLine("    WHERE nmDatabase = @nmDatabase");
            retorno.AppendLine("    ORDER BY nuVersion");
            retorno.AppendLine("    SET NOCOUNT OFF");
            retorno.AppendLine("END");
            retorno.AppendLine("GO");

            // Registra uma nova versão aplicada na base de dados
            retorno.AppendLine("IF OBJECT_ID('dbo.pRegisterNewVersion','P') IS NOT NULL");
            retorno.AppendLine("    DROP PROCEDURE dbo.pRegisterNewVersion");
            retorno.AppendLine("GO");
            retorno.AppendLine("CREATE PROCEDURE dbo.pRegisterNewVersion");
            retorno.AppendLine("    @nuVersion          INT,");
            retorno.AppendLine("    @nmDatabase         SYSNAME,");
            retorno.AppendLine("    @user_execute       SYSNAME,");
            retorno.AppendLine("    @dateTimeExecute    DATETIME,");
            retorno.AppendLine("    @nmFile             VARCHAR(100),");
            retorno.AppendLine("    @scriptContent      VARCHAR(MAX)");
            retorno.AppendLine("AS");
            retorno.AppendLine("BEGIN");
            retorno.AppendLine("    SET TRANSACTION ISOLATION LEVEL SNAPSHOT");
            retorno.AppendLine("    SET NOCOUNT ON");
            retorno.AppendLine("    INSERT INTO dbo.VersionsControl");
            retorno.AppendLine("        (nuVersion, nmDatabase, user_execute, dateTimeExecute, nmFile, scriptContent)");
            retorno.AppendLine("    VALUES");
            retorno.AppendLine("        (@nuVersion, @nmDatabase, @user_execute, ISNULL(@dateTimeExecute, GETDATE()), @nmFile, @scriptContent)");
            retorno.AppendLine("    SET NOCOUNT OFF");
            retorno.AppendLine("END");
            retorno.AppendLine("GO");

            // Retorna a última versão aplicada de uma base de dados específica
            retorno.AppendLine("IF OBJECT_ID('dbo.pReturnMaxVersionForDb', 'P') IS NOT NULL");
            retorno.AppendLine("    DROP PROCEDURE dbo.pReturnMaxVersionForDb");
            retorno.AppendLine("GO");
            retorno.AppendLine("CREATE PROCEDURE dbo.pReturnMaxVersionForDb");
            retorno.AppendLine("    @nmDatabase SYSNAME");
            retorno.AppendLine("AS");
            retorno.AppendLine("BEGIN");
            retorno.AppendLine("    SET TRANSACTION ISOLATION LEVEL SNAPSHOT");
            retorno.AppendLine("    SET NOCOUNT ON");
            retorno.AppendLine("    SELECT");
            retorno.AppendLine("        MAX(NuVersion) MaxVersion");
            retorno.AppendLine("    FROM dbo.VersionsControl");
            retorno.AppendLine("    WHERE");
            retorno.AppendLine("        nmDatabase = @nmDatabase");
            retorno.AppendLine("    SET NOCOUNT OFF");
            retorno.AppendLine("END");
            retorno.AppendLine("GO");

            return retorno.ToString();
        }
    }
}
