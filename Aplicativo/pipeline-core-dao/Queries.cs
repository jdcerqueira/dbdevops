using System;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace pipeline_core_dao
{
    public static class Queries
    {

        /*
         * Dada uma query e uma conexão retorna o resultSet
         */
        public static SqlDataReader ExecuteResultSet(Connection connection, String query)
        {
            SqlCommand sqlCommand = new SqlCommand(query, connection.sqlConnection);
            if (sqlCommand.Connection.State == System.Data.ConnectionState.Open)
                sqlCommand.Connection.Close();

            sqlCommand.Connection.Open();
            return sqlCommand.ExecuteReader();
        }

        /*
         * Dado uma query, conexão executa a query e apenas retorna se ocorreu sucesso
         */
        public static Boolean ExecuteNonResultSet(Connection connection, String query)
        {

            SqlCommand sqlCommand = new SqlCommand(query, connection.sqlConnection);
            if (sqlCommand.Connection.State == System.Data.ConnectionState.Open)
                sqlCommand.Connection.Close();

            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();

            return true;
        }

        /*
         * Dado uma conexão e o conteúdo de um Script.SQL, é executada a instrução.
         */
        public static void ExecuteScriptFile(Connection connection, String contentFile)
        {
            if (connection.sqlConnection.State == System.Data.ConnectionState.Open)
                connection.sqlConnection.Close();
            
            connection.sqlConnection.Open();
            Server server = new Server(new ServerConnection(connection.sqlConnection));
            server.ConnectionContext.ExecuteNonQuery(contentFile);
        }
    }
}
