using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicaScript
{
    public class Infos
    {

        public class BaseDados
        {
            public static SqlConnection connection(String connString)
            {
                try
                {
                    SqlConnection retorno = new SqlConnection(connString);
                    new Infos.Log("Infos.BaseDados.connection(?)", "Conexão com a base de dados gerada com sucesso.", new Infos.Configuracao());
                    return retorno;
                }
                catch (SqlException sq)
                {
                    new Infos.Log("Infos.BaseDados.connection(?)", $"Erro ao gerar o reader - SQL Exception ({sq.StackTrace} >> {sq.Message}).", new Infos.Configuracao());
                }
                catch (Exception ex)
                {
                    new Infos.Log("Infos.BaseDados.connection(?)", $"Erro ao gerar o reader - Exception ({ex.StackTrace} >> {ex.Message}).", new Infos.Configuracao());
                }

                return new SqlConnection();
            }

            public static Boolean executaQuery(SqlConnection conn, String stmt)
            {
                try
                {
                    SqlCommand command = new SqlCommand(stmt, conn);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    new Infos.Log("Infos.BaseDados.executaQueryResultSet(?,?)", "Script executado com sucesso.", new Infos.Configuracao());
                    return true;
                }
                catch (SqlException sq)
                {
                    new Infos.Log("Infos.BaseDados.executaQuery(?,?)", $"Erro ao gerar o reader - SQL Exception ({sq.StackTrace} >> {sq.Message}).", new Infos.Configuracao());
                }
                catch (Exception ex)
                {
                    new Infos.Log("Infos.BaseDados.executaQuery(?,?)", $"Erro ao gerar o reader - Exception ({ex.StackTrace} >> {ex.Message}).", new Infos.Configuracao());
                }

                return false;
            }

            public static SqlDataReader executaQueryResultSet(SqlConnection conn, String stmt)
            {
                try
                {
                    SqlCommand command = new SqlCommand(stmt, conn);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    new Infos.Log("Infos.BaseDados.executaQueryResultSet(?,?)","Gerado reader com sucesso.",new Infos.Configuracao());
                    return reader;
                }
                catch (SqlException sq)
                {
                    new Infos.Log("Infos.BaseDados.executaQueryResultSet(?,?)", $"Erro ao gerar o reader - SQL Exception ({sq.StackTrace} >> {sq.Message}).", new Infos.Configuracao());
                }
                catch(Exception ex)
                {
                    new Infos.Log("Infos.BaseDados.executaQueryResultSet(?,?)", $"Erro ao gerar o reader - Exception ({ex.StackTrace} >> {ex.Message}).", new Infos.Configuracao());
                }

                return null;
            }
        }
        public class Constantes
        {
            public static String connectionString(String servidor)
            {
                return $@"Server={servidor};Database=master;Trusted_Connection=True;";
            }
            public static String connectionString(String baseControladora, String servidor)
            {
                return $@"Server={servidor};Database={baseControladora};Trusted_Connection=True;";
            }
        }
        public class Scripts
        {
            public String fullpath { get; set; }
            public String filename { get; set; }

            public class ParaAplicar
            {
                public List<Scripts> arquivos { get; set; }
            }
            public class ScriptCompleto
            {
                public Scripts arquivo { get; set; }
            }
            public class Aplicados
            {
                public List<Scripts> arquivos { get; set; }
            }
        }
        public class Arquivo
        {
            public static StringBuilder conteudoArquivo(String arquivo)
            {
                StringBuilder retorno = new StringBuilder();

                if (!System.IO.File.Exists(arquivo))
                {
                    System.IO.File.Create(arquivo);
                    return new StringBuilder().Append("");
                }
                    

                foreach (String linha in System.IO.File.ReadAllLines(arquivo))
                    retorno.AppendLine(linha);

                return retorno;
            }

            public static void moveArquivo(String origem, String destino)
            {
                StringBuilder conteudoArquivoOrigem = Infos.Arquivo.conteudoArquivo(origem);
                System.IO.File.WriteAllText(destino,conteudoArquivoOrigem.ToString());
                System.IO.File.Delete(origem);
            }

            public static void escreveArquivo(String arquivo, String conteudo)
            {

                if (!System.IO.File.Exists(arquivo))
                    System.IO.File.Create(arquivo);

                System.IO.File.WriteAllText(arquivo, conteudo);
            }

            public static String caminhoCompleto(String arquivo, String caminhoLogico = "")
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory() + caminhoLogico);
                return directoryInfo.FullName + "\\" + arquivo;
            }

            public static System.IO.DirectoryInfo caminhoCompleto(String caminhoLogico = "")
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory() + caminhoLogico);
                return directoryInfo;
            }
        }
        public class Log
        {
            public DateTime dateTime = DateTime.Now;
            public String metodo = "";
            public String mensagem = "";
            public String arquivoLog = "";

            public Log(String _metodo, String _mensagem, Infos.Configuracao configuracao)
            {
                this.metodo = _metodo;
                this.mensagem = _mensagem;
                this.arquivoLog = configuracao.retornaArquivoLog();

                String linhaLog = $"{dateTime.ToString()} - {metodo} - {mensagem}";
                Console.WriteLine(linhaLog);
            }
        }
        public class Configuracao
        {
            public String branch = "";
            public String connection = "";
            public String aplicaScript = "";
            public String scriptCompleto = "";
            public String log = "";
            public String scriptAplicado = "";
            public const String arquivoScriptCompleto = "script_completo.sql";
            public const String arquivoLog = "log.txt";
            public String baseControladora = "";

            public Configuracao()
            {
                inicializaVariaveisConfiguracao();
            }

            public void inicializaVariaveisConfiguracao()
            {
                foreach (String line in System.IO.File.ReadAllText($@"config").Split('\n'))
                {
                    String[] lineKeyValue = line.Split('=');

                    this.branch = lineKeyValue[0] == "branch" ? lineKeyValue[1] : this.branch;
                    this.connection = lineKeyValue[0] == "connection" ? lineKeyValue[1] : this.connection;
                    this.aplicaScript = lineKeyValue[0] == "aplicaScript" ? lineKeyValue[1] : this.aplicaScript;
                    this.scriptCompleto = lineKeyValue[0] == "scriptCompleto" ? lineKeyValue[1] : this.scriptCompleto;
                    this.scriptAplicado = lineKeyValue[0] == "scriptAplicado" ? lineKeyValue[1] : this.scriptAplicado;
                    this.log = lineKeyValue[0] == "log" ? lineKeyValue[1] : this.log;
                    this.baseControladora = lineKeyValue[0] == "baseControladora" ? lineKeyValue[1] : this.baseControladora;
                }
            }

            public String retornaArquivoLog()
            {
                return Infos.Arquivo.caminhoCompleto(arquivoLog, "\\" + this.log);
            }

            public Infos.Scripts.ParaAplicar scriptsParaAplicar()
            {
                Infos.Scripts.ParaAplicar retorno = new Infos.Scripts.ParaAplicar();
                retorno.arquivos = new List<Infos.Scripts>();

                System.IO.DirectoryInfo directoryInfoAplicaScript = Infos.Arquivo.caminhoCompleto("\\" + this.aplicaScript);
                foreach (var item in directoryInfoAplicaScript.GetFiles("*.sql").OrderBy(file => file.Name))
                    retorno.arquivos.Add(new Infos.Scripts() { fullpath = item.FullName, filename = item.Name });

                return retorno;
            }

            public Infos.Scripts.Aplicados scriptsAplicados()
            {
                return new Infos.Scripts.Aplicados() { arquivos = Infos.Configuracao.listaScriptsNaPasta(this.scriptAplicado) };
            }

            public static List<Infos.Scripts> listaScriptsNaPasta(String caminho)
            {
                List<Infos.Scripts> retorno = new List<Scripts>();

                System.IO.DirectoryInfo directoryInfoAplicados = Infos.Arquivo.caminhoCompleto("\\" + caminho);
                foreach (var item in directoryInfoAplicados.GetFiles("*.sql").OrderBy(file => file.Name))
                    retorno.Add(new Infos.Scripts() { fullpath = item.FullName, filename = item.Name });

                return retorno;
            }
        }
    }
}
