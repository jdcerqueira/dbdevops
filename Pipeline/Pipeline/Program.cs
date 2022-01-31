using pipeline_config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipeline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Carregar arquivo de configuracao
            //Inicia o pipeline
            new Pipeline(new Configuracao());
        }
    }
}
