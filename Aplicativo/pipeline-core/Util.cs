using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace pipeline_core
{
    public class Util
    {
        public class Constantes
        {
            public const String arquivoConfiguracao = "config";
            public const String keyCripto = "abc";
            public const String arquivoScriptBaseVersionadora = "01_DatabaseControlVersion_Create.sql";
            public const String arquivoScriptCompleto = "script_completo.sql";

            public static String scriptBaseVersionadora()
            {
                StringBuilder retorno = new StringBuilder();

                // Cria a base de dados
                retorno.AppendLine("USE master");
                retorno.AppendLine("GO");
                retorno.AppendLine($"IF DB_ID('{Configuracao.baseControladora}') IS NOT NULL");
                retorno.AppendLine("BEGIN");
                retorno.AppendLine($"   ALTER DATABASE[{Configuracao.baseControladora}] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE");
                retorno.AppendLine($"   DROP DATABASE[{Configuracao.baseControladora}]");
                retorno.AppendLine("END");
                retorno.AppendLine("GO");
                retorno.AppendLine($"CREATE DATABASE[{Configuracao.baseControladora}]");
                retorno.AppendLine("GO");
                retorno.AppendLine($"ALTER DATABASE[{Configuracao.baseControladora}] SET ALLOW_SNAPSHOT_ISOLATION ON");
                retorno.AppendLine("GO");

                // Cria o usuário aplicador dos scripts
                retorno.AppendLine("USE[master]");
                retorno.AppendLine("GO");

                retorno.AppendLine($"IF EXISTS(SELECT TOP 1 1 FROM sys.syslogins WHERE name = '{Configuracao.loginControladora}')");
                retorno.AppendLine("BEGIN");
                retorno.AppendLine($"    DROP LOGIN {Configuracao.loginControladora}");
                retorno.AppendLine("END");
                retorno.AppendLine("GO");

                retorno.AppendLine($"CREATE LOGIN {Configuracao.loginControladora} WITH PASSWORD = '{Configuracao.senhaControladora}', CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF");
                retorno.AppendLine("GO");

                retorno.AppendLine($"ALTER SERVER ROLE[dbcreator] ADD MEMBER[{Configuracao.loginControladora}]");
                retorno.AppendLine($"ALTER SERVER ROLE[securityadmin] ADD MEMBER[{Configuracao.loginControladora}]");
                retorno.AppendLine("GO");

                retorno.AppendLine($"USE[{Configuracao.baseControladora}]");
                retorno.AppendLine("GO");

                retorno.AppendLine($"CREATE USER {Configuracao.usuarioControladora} FOR LOGIN {Configuracao.loginControladora}");
                retorno.AppendLine("GO");
                retorno.AppendLine("CREATE ROLE AplicaScripts AUTHORIZATION [dbo]");
                retorno.AppendLine("GO");
                retorno.AppendLine($"ALTER ROLE AplicaScripts ADD MEMBER {Configuracao.usuarioControladora}");
                retorno.AppendLine("GO");
                retorno.AppendLine("GRANT EXECUTE TO AplicaScripts");
                retorno.AppendLine("GO");

                // Cria os objetos
                // Tabela de versionamento
                retorno.AppendLine($"USE[{Configuracao.baseControladora}]");
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

            public class QueriesEstaticas
            {
                public static String verificaBaseControladora()
                {
                    return $"SELECT DB_ID('{Configuracao.baseControladora}')";
                }

                public static String versaoAtualAmbiente()
                {
                    return $"SELECT MAX(Versao_Script) UltimaVersao, Base_Dados FROM dbo.Versionamento GROUP BY Base_Dados";
                }
            }
        }

        public class Arquivos
        {
            public static void complementaArquivo(String arquivoOrigem, String arquivoDestino)
            {
                try
                {
                    Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "carregaConteudoArquivo", $"{arquivoOrigem} -> {arquivoDestino}" });
                    String contentArquivoOrigem = carregaConteudoArquivo(arquivoOrigem);
                    File.AppendAllText(arquivoOrigem, contentArquivoOrigem);
                }
                catch (Exception ex)
                {
                    Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "ERRO", ex.Message });
                }
            }

            public static void moveArquivo(String arquivoOrigem, String arquivoDestino)
            {
                File.Move(arquivoOrigem, arquivoDestino);
            }


            public static String carregaConteudoArquivo(String arquivo)
            {
                validaArquivo(arquivo);
                return File.ReadAllText(arquivo);
            }

            public static void validaArquivo(String arquivo)
            {
                if (!File.Exists(arquivo))
                {
                    var criado = File.Create(arquivo);
                    criado.Close();
                }
            }

            public static FileInfo[] listaArquivosPasta(String caminhoPasta)
            {
                Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "Util.Arquivos.validaDiretorio", $"Valida a existência da pasta - {caminhoPasta}" });

                if (validaDiretorio(caminhoPasta))
                    return new DirectoryInfo(caminhoPasta).GetFiles("*.sql").OrderBy(file => file.Name).ToArray();

                Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "Retorno", $"Não foram encontrados arquivos na pasta - {caminhoPasta}" });
                return new FileInfo[] { null };
            }

            public static Boolean validaDiretorio(String caminhoPasta)
            {
                if (caminhoPasta == null || caminhoPasta == "")
                    return false;

                if (!Directory.Exists(caminhoPasta))
                    Directory.CreateDirectory(caminhoPasta);

                return true;
            }

            public static void gravaArquivo(String caminho, String conteudo)
            {
                try
                {
                    Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "validaArquivo", $"Valida a existência do arquivo - {caminho}" });
                    validaArquivo(caminho);
                    File.WriteAllText(caminho, conteudo);
                }
                catch (Exception ex)
                {
                    Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "ERRO", ex.Message });
                    throw new Exception(ex.StackTrace);
                }
            }
        }

        public class Criptografia
        {
            public static String Encrypt(String toEncrypt, String key, Boolean useHashing)
            {
                Byte[] keyArray;
                Byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }

            public static String Decrypt(String toDecrypt, String key, Boolean useHashing)
            {
                Byte[] keyArray;
                Byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }
    }
}
