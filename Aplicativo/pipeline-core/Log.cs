using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pipeline_core
{
    public static class Log
    {
        static String classe;
        static String metodo;
        static String acao;
        static String conteudo;
        static DateTime dtLog = DateTime.Now;

        private static String ToString()
        {
            return $"{dtLog.ToString()}" +
                $" - {classe}.{metodo}.{acao}" +
                $" - {conteudo}";
        }

        public static void registraLog(String[] _log)
        {
            classe = _log[0];
            metodo = _log[1];
            acao = _log[2];
            conteudo = _log[3];

            Console.WriteLine(ToString());
            // Persistir em arquivo
        }
    }
}
