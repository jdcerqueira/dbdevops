using System;
using Microsoft.Data.SqlClient;

namespace pipeline_core_dao
{
    public class Connection
    {
        public SqlConnection sqlConnection;
        
        public Connection(String stringConnection)
        {
            this.sqlConnection = new SqlConnection(stringConnection);
        }
    }
}
