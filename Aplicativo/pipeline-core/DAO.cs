using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using pipeline_core_dao;


namespace pipeline_core
{
    public class DAO
    {
        //public String strConnectionBaseVersionadora = $@"";
        //public String strConnectionBaseMaster = $@"";

        //public DAO(Configuracao _configuracao)
        //{
        //    strConnectionBaseVersionadora = $@"Data Source = {_configuracao.connection}; Initial Catalog = {Configuracao.baseControladora}; User Id = {Configuracao.loginControladora}; Password = {Configuracao.senhaControladora};Encrypt=True;TrustServerCertificate=True;";
        //    strConnectionBaseMaster = $@"Data Source = {_configuracao.connection}; Initial Catalog = master; User Id={_configuracao.usuarioBase}; Password = {_configuracao.senhaBase}; Encrypt=True; TrustServerCertificate=True;";
        //}

        //public SqlConnection GetConnection(String ConnectionString)
        //{
        //    return new Connection(ConnectionString).sqlConnection;
        //}

        //public void executaArquivoScript(String caminhoScript, Boolean isVersionadora)
        //{
        //    String script = File.ReadAllText(caminhoScript);
        //    try
        //    {
        //        using (SqlConnection sqlconnection = this.GetConnection(isVersionadora ? this.strConnectionBaseVersionadora : this.strConnectionBaseMaster))
        //        {
        //            sqlconnection.Open();
        //            Server server = new Server(new ServerConnection(sqlconnection));
        //            server.ConnectionContext.ExecuteNonQuery(script);
        //        }
        //    }
        //    catch (System.Data.SqlClient.SqlException sqlex)
        //    {
        //        Console.WriteLine("ERR (SQL Exception):" + sqlex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ERR: (Exception)" + ex.Message);
        //    }
        //}
    }
}
