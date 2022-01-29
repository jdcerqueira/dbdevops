using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicaScript
{
    public class Git
    {
        private static String executaComandos(String comando)
        {
            String diretorioAtual = System.IO.Directory.GetCurrentDirectory().ToString();

            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(comando);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            StringBuilder stringBuilder = new StringBuilder(cmd.StandardOutput.ReadToEnd());
            String[] Lines = stringBuilder.ToString().Split('\n');
            StringBuilder strReturn = new StringBuilder();

            for (int i = 4; i < Lines.Length - 2; i++)
            {
                if (Lines[i].Length > 1 && Lines[i] != $@"{diretorioAtual}>" && Lines[i] != $@"{diretorioAtual}>{comando}")
                    strReturn.AppendLine(Lines[i].Trim());
            }

            return strReturn.ToString().Replace("\n","");
        }

        public static String branchAtual()
        {
            return executaComandos("git branch --show-current");
        }

        public static String fetch()
        {
            executaComandos("git fetch");
            return "Ok!";
        }

        public static String status()
        {
            return executaComandos("git status");
        }

        public static String pull()
        {
            return executaComandos("git pull");
        }
    }
}
