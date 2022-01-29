using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipeline
{
    public class DAO
    {
        public static SqlConnection connection(Configuracao configuracao)
        {
            try
            {
                String strConnection = $@"Server = {configuracao.connection}; Database = master; Trusted_Connection = True;";
                return new SqlConnection(strConnection);
            }
            catch (SqlException sq)
            {
                // aqui precisa escrever no arquivo de log
                Console.WriteLine($"ERR(DAO.connection(?,?)):{sq.Message}");
            }
            catch (Exception ex)
            {
                // aqui precisa escrever no arquivo de log
                Console.WriteLine($"ERR(DAO.connection(?,?)):{ex.Message}");
            }

            return null;
        }

        public static SqlConnection connection(Configuracao configuracao, String baseDados)
        {
            try
            {
                String strConnection = $"Server = {configuracao.connection}; Database = {baseDados}; Trusted_Connection = True;";
                return new SqlConnection(strConnection);
            }
            catch (SqlException sq)
            {
                // aqui precisa escrever no arquivo de log
                Console.WriteLine($"ERR(DAO.connection(?)):{sq.Message}");
            }
            catch (Exception ex)
            {
                // aqui precisa escrever no arquivo de log
                Console.WriteLine($"ERR(DAO.connection(?)):{ex.Message}");
            }

            return null;
        }

        public static void executaComando(SqlConnection connection, String statement)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(statement, connection))
                {
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException sq)
            {
                // aqui precisa escrever no arquivo de log
                Console.WriteLine($"ERR(DAO.executaComando(?,?)):{sq.Message}");
            }
            catch (Exception ex)
            {
                // aqui precisa escrever no arquivo de log
                Console.WriteLine($"ERR(DAO.executaComando(?,?)):{ex.Message}");
            }
        }

        public static SqlDataReader executaComandoResultSet(SqlConnection connection, String statement)
        {
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(statement, connection))
                {
                    sqlCommand.Connection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (!sqlDataReader.HasRows)
                        return null;

                    return sqlDataReader;
                }
            }
            catch (SqlException sq)
            {
                // aqui precisa escrever no arquivo de log
                Console.WriteLine($"ERR(DAO.executaComandoResultSet(?,?)):{sq.Message}");
            }
            catch (Exception ex)
            {
                // aqui precisa escrever no arquivo de log
                Console.WriteLine($"ERR(DAO.executaComandoResultSet(?,?)):{ex.Message}");
            }

            return null;
        }
    }
}
