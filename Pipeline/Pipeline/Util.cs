using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pipeline
{
    public class Util
    {

        public class Constantes
        {
            public const String arquivoConfiguracao = "config";

            public class QueriesEstaticas
            {
                public static String verificaBaseControladora(Configuracao _configuracao)
                {
                    return $"SELECT DB_ID('{_configuracao.baseControladora}')";
                }

                public static String criaBaseControladora(Configuracao _configuracao)
                {
                    return $"CREATE DATABASE {_configuracao.baseControladora}";
                }

                public static String criaEstruturaBaseControladora()
                {
                    return $"CREATE TABLE dbo.Versionamento" +
                        $"(" +
                        $"Versao_Script INT PRIMARY KEY," +
                        $"Script VARCHAR(MAX)," +
                        $"Base_Dados SYSNAME," +
                        $"Data_Execucao DATETIME DEFAULT GETDATE()," +
                        $"Usuario SYSNAME," +
                        $"Servidor SYSNAME)";
                }

                public static String versaoAtualAmbiente()
                {
                    return $"SELECT Versao_Script FROM dbo.Versionamento";
                }

                public static String aplicaVersaoScript(Scripts.Info _info, String conteudoScript, Configuracao _configuracao)
                {
                    return "INSERT INTO dbo.Versionamento" +
                        "(Versao_Script, " +
                        "Script, " +
                        "Base_Dados, " +
                        "Usuario, " +
                        "Servidor)" +
                        "VALUES(" +
                        $"{_info.versao}," +
                        $"'{conteudoScript.Replace("'","")}'," +
                        $"'{_info.baseDados}'," +
                        $"'sa'," +
                        $"'{_configuracao.connection}'" +
                        ")";
                }
            }
        }

        public class Arquivos
        {
            public static String[] carregaConteudoArquivo(String arquivo)
            {
                return File.ReadAllLines(arquivo);
            }

            public static FileInfo[] listaArquivosPasta(String caminhoPasta)
            {
                validaDiretorio(caminhoPasta);
                return new DirectoryInfo(caminhoPasta).GetFiles("*.sql").OrderBy(file=>file.Name).ToArray();
            }

            public static void validaDiretorio(String caminhoPasta)
            {
                if (!Directory.Exists(caminhoPasta))
                    Directory.CreateDirectory(caminhoPasta);
            }
        }
    }
}
