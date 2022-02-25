using System;
using System.Collections.Generic;
using System.IO;

namespace pipeline_core
{
    public class Scripts
    {
        public String nomeArquivo { get; set; }
        public String caminhoArquivo { get; set; }

        public class Info
        {
            public String baseDados;
            public int versao;

            public Info(Scripts _script)
            {
                this.baseDados = _script.nomeArquivo.Split('_')[1].Replace(".sql", "");
                this.versao = Int32.Parse(_script.nomeArquivo.Split('_')[0].Replace("v", ""));
            }
        }

        public static void aplicaScript(Scripts script, Configuracao configuracao)
        {
            Scripts.Info info = new Scripts.Info(script);
            DAO executaScript = new DAO(configuracao);

            Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "executaArquivoScript", $"Aplicando o script {script.caminhoArquivo}" });
            executaScript.executaArquivoScript(script.caminhoArquivo, false);

            Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "executaQuery", $"Aplicando o script exec dbo.pRegisterNewVersion" });
            executaScript.executaQuery($"exec dbo.pRegisterNewVersion {info.versao},'{info.baseDados}','{Configuracao.loginControladora}',null,'{script.caminhoArquivo}','{Util.Criptografia.Encrypt(File.ReadAllText(script.caminhoArquivo), Util.Constantes.keyCripto, true)}'", true);
        }

        public static void aplicaScript(Scripts script, Configuracao configuracao, Boolean isPendente)
        {
            try
            {
                Scripts.Info info = new Scripts.Info(script);
                DAO executaScript = new DAO(configuracao);

                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "executaArquivoScript", $"Aplicando o script {script.caminhoArquivo}" });
                executaScript.executaArquivoScript(script.caminhoArquivo, false);

                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "executaQuery", $"Aplicando o script exec dbo.pRegisterNewVersion" });
                executaScript.executaQuery($"exec dbo.pRegisterNewVersion {info.versao},'{info.baseDados}','{Configuracao.loginControladora}',null,'{script.caminhoArquivo}','{Util.Criptografia.Encrypt(File.ReadAllText(script.caminhoArquivo), Util.Constantes.keyCripto, true)}'", true);

                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "complementaArquivo", $"Irá complementar o arquivo de script completo" });
                Util.Arquivos.complementaArquivo(script.caminhoArquivo, $@"{configuracao.scriptCompleto}\{Util.Constantes.arquivoScriptCompleto}");

                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "moveArquivo", $"{script.nomeArquivo} -> {configuracao.scriptAplicado}" });
                Util.Arquivos.moveArquivo(script.caminhoArquivo, $"{configuracao.scriptAplicado}\\{script.nomeArquivo}");
            }
            catch (Exception ex)
            {
                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "ERRO", ex.Message });
            }

        }


        public static List<Scripts> GetScripts(String Caminho)
        {
            List<Scripts> scripts = new List<Scripts>();

            foreach (FileInfo arquivo in Util.Arquivos.listaArquivosPasta(Caminho))
            {
                Scripts script = new Scripts() { nomeArquivo = arquivo.Name, caminhoArquivo = arquivo.FullName };
                Scripts.Info info = new Scripts.Info(script);
                scripts.Add(script);
            }

            return scripts;
        }

        public static List<Scripts> GetScripts(String Caminho, String BaseDados)
        {
            List<Scripts> scripts = new List<Scripts>();

            foreach (FileInfo arquivo in Util.Arquivos.listaArquivosPasta(Caminho))
            {
                Scripts script = new Scripts() { nomeArquivo = arquivo.Name, caminhoArquivo = arquivo.FullName };
                Scripts.Info info = new Scripts.Info(script);
                if (info.baseDados == BaseDados)
                    scripts.Add(script);
            }

            return scripts;
        }
    }
}
