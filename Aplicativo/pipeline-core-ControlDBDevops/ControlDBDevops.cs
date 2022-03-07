using Microsoft.Data.SqlClient;
using pipeline_core_dao;
using pipeline_core_log;
using System;
using System.Collections.Generic;
using System.IO;

namespace pipeline_core_ControlDBDevops
{
    public class ControlDBDevops
    {
        public static Connection connectionMaster;
        public static Connection connectionControlDBDevops;
        String caminhoScriptBaseCompleta = "";
        
        public ControlDBDevops(String Servidor, String usuario, String senha, String pastaBaseVersionadora)
        {
            //Inicializa as variaveis de configuracao
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "connectionMaster", "Definição da conexão com o servidor (base master)" });
            connectionMaster = new Connection($@"Data Source = {Servidor}; Initial Catalog = master; User Id = {usuario}; Password = {senha};Encrypt=True;TrustServerCertificate=True;");

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "connectionControlDBDevops", "Definição da conexão com o servidor (base versionadora)" });
            connectionControlDBDevops = new Connection($@"Data Source = {Servidor}; Initial Catalog = {Constantes.baseControladora}; User Id = {Constantes.loginControladora}; Password = {Constantes.senhaControladora};Encrypt=True;TrustServerCertificate=True;");

            // Cria script da base de dados controladora no repositório indicado
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "caminhoScriptBaseCompleta", "" });
            this.caminhoScriptBaseCompleta = $@"{pastaBaseVersionadora}\{Constantes.arquivoBaseVersionadora}";

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "gravaArquivo", "Gravando o arquivo: " + $@"{this.caminhoScriptBaseCompleta }" });
            pipeline_core_util.Arquivos.gravaArquivo(this.caminhoScriptBaseCompleta, Constantes.scriptBaseVersionadora());

            // Verifica a base aplicada
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "aplicaBaseVersionadoraAmbiente", "" });
            aplicaBaseVersionadoraAmbiente();
        }

        public void aplicaBaseVersionadoraAmbiente()
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteResultSet", "Constantes.selectIdDatabase {?}" });
            SqlDataReader baseExistente = Queries.ExecuteResultSet(connectionMaster, Constantes.selectIdDatabase);

            if (baseExistente.Read())
            {
                if (baseExistente[0].ToString() == "0")
                {
                    Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "ExecuteScriptFile", "Constantes.scriptBaseVersionadora() {?}" });
                    Queries.ExecuteScriptFile(connectionMaster, Constantes.scriptBaseVersionadora());
                }
                else
                    Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "aplicaBaseVersionadoraAmbiente", "Identificada a base de dados versionadora." });
            }
            else
                Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "aplicaBaseVersionadoraAmbiente", "Não retornou dados no resultSet." });
        }

        public static Boolean registraVersaoBaseDados(int versao, String dataBase, String nomeArquivo, String conteudoArquivoCripto)
        {
            try
            {
                String instrucaoSql = $"EXEC dbo.pRegisterNewVersion {versao},'{dataBase}','{Constantes.usuarioControladora}',NULL,'{nomeArquivo}','{conteudoArquivoCripto}'";
                Queries.ExecuteNonResultSet(ControlDBDevops.connectionControlDBDevops, instrucaoSql);
            }
            catch(SqlException ex)
            {
                throw new Exception(ex.Message);

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }

        public class Info
        {
            public int nuVersion { get; set; }
            public String nmDatabase { get; set; }
            public String user_execute { get; set; }
            public DateTime dateTimeExecute { get; set; }
            public String nmFile { get; set; }
            public String scriptContent { get; set; }

            override
            public String ToString()
            {
                return $"nuVersion: {this.nuVersion} - nmDatabase: {this.nmDatabase} - dateTimeExecute: {this.dateTimeExecute.ToString()}";
            }


            public static List<Info> versoesPorBase(String database)
            {
                String instrucaoSql = $"EXEC dbo.pReturnAllVersionsForDb '{database}'";
                SqlDataReader result = Queries.ExecuteResultSet(ControlDBDevops.connectionControlDBDevops, instrucaoSql);

                List<Info> retorno = new List<Info>();
                while (result.Read())
                {
                    retorno.Add(new ControlDBDevops.Info()
                    {
                        nuVersion = Int32.Parse(result["nuVersion"].ToString()),
                        nmDatabase = result["nmDatabase"].ToString(),
                        user_execute = result["user_execute"].ToString(),
                        dateTimeExecute = DateTime.Parse(result["dateTimeExecute"].ToString()),
                        nmFile = result["nmFile"].ToString(),
                        scriptContent = result["scriptContent"].ToString()
                    });
                }
                return retorno;
            }

            public static Dictionary<String, int> versoesPorBase()
            {
                String instrucaoSql = $"EXEC dbo.pReturnMaxVersionByDb";
                SqlDataReader sql = Queries.ExecuteResultSet(connectionControlDBDevops, instrucaoSql);

                Dictionary<String, int> retorno = new Dictionary<String, int>();
                while (sql.Read())
                    retorno.Add(sql["nmDatabase"].ToString(), Int32.Parse(sql["MaxVersion"].ToString()));

                return retorno;
            }
        }
    }
}
