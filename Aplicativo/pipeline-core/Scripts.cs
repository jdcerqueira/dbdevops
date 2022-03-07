using System;
using System.Collections.Generic;
using System.IO;
using pipeline_core_log;
using pipeline_core_dao;
using pipeline_core_ControlDBDevops;

namespace pipeline_core
{
    public class Scripts
    {
        public String nomeArquivo;
        public String caminhoArquivo;
        public Info info;

        public static String MsgErro = "";
        public static Boolean Ok = true;

        public Scripts(String _nomeArquivo, String _caminhoArquivo)
        {
            this.nomeArquivo = _nomeArquivo;
            this.caminhoArquivo = _caminhoArquivo;
            this.info = new Scripts.Info(this);
        }

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

        public static void aplicaScript(Scripts script)
        {
            //DAO executaScript = new DAO(configuracao);

            Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "Queries.ExecuteScriptFile", $"Aplicando o script {script.caminhoArquivo}" });
            Queries.ExecuteScriptFile(ControlDBDevops.connectionMaster, pipeline_core_util.Arquivos.carregaConteudoArquivo(script.caminhoArquivo));

            Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "ControlDBDevops.registraVersaoBaseDados", ""});
            ControlDBDevops.registraVersaoBaseDados(script.info.versao, script.info.baseDados, script.caminhoArquivo, pipeline_core_util.Criptografia.Encrypt(pipeline_core_util.Arquivos.carregaConteudoArquivo(script.caminhoArquivo), pipeline_core_util.Constantes.keyCripto,true));
        }

        public static void aplicaScript(Scripts script, Configuracao configuracao)
        {
            try
            {
                //DAO executaScript = new DAO(configuracao);

                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "Queries.ExecuteScriptFile", $"Aplicando o script {script.caminhoArquivo}" });
                Queries.ExecuteScriptFile(ControlDBDevops.connectionMaster, pipeline_core_util.Arquivos.carregaConteudoArquivo(script.caminhoArquivo));

                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "ControlDBDevops.registraVersaoBaseDados", $"" });
                ControlDBDevops.registraVersaoBaseDados(script.info.versao, script.info.baseDados, script.caminhoArquivo, pipeline_core_util.Criptografia.Encrypt(pipeline_core_util.Arquivos.carregaConteudoArquivo(script.caminhoArquivo), pipeline_core_util.Constantes.keyCripto, true));

                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "pipeline_core_util.Arquivos.complementaArquivo", $"Irá complementar o arquivo de script completo" });
                pipeline_core_util.Arquivos.complementaArquivo(script.caminhoArquivo, $@"{configuracao.scriptCompleto}\{pipeline_core_util.Constantes.arquivoScriptCompleto}");

                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "pipeline_core_util.Arquivos.moveArquivo", $"{script.nomeArquivo} -> {configuracao.scriptAplicado}" });
                pipeline_core_util.Arquivos.moveArquivo(script.caminhoArquivo, $"{configuracao.scriptAplicado}\\{script.nomeArquivo}");
            }
            catch (Exception ex)
            {
                Log.registraLog(new String[] { "Scripts", System.Reflection.MethodBase.GetCurrentMethod().Name, "ERRO", ex.Message });
            }

        }


        public static List<Scripts> GetScripts(String[] Caminho)
        {
            List<Scripts> scripts = new List<Scripts>();

            for (int i = 0; i < Caminho.Length; i++)
            {
                foreach (FileInfo arquivo in pipeline_core_util.Arquivos.listaArquivosPasta(Caminho[i]))
                {
                    Scripts script = new Scripts(arquivo.Name, arquivo.FullName);
                    if(!scripts.Exists(x=>x.nomeArquivo == script.nomeArquivo))
                        scripts.Add(script);
                }
            }

            return scripts;
        }

        public static List<Scripts> GetScripts(String[] Caminho, String BaseDados)
        {
            List<Scripts> scripts = new List<Scripts>();

            for (int i = 0; i < Caminho.Length; i++)
            {
                foreach (FileInfo arquivo in pipeline_core_util.Arquivos.listaArquivosPasta(Caminho[i]))
                {
                    Scripts script = new Scripts(arquivo.Name, arquivo.FullName);
                    if (script.info.baseDados == BaseDados && !scripts.Exists(x=>x.nomeArquivo == script.nomeArquivo))
                        scripts.Add(script);
                }
            }

            return scripts;
        }

        public static List<String> GetBases(String[] Caminho)
        {

            List<String> retorno = new List<String>();

            for (int i = 0; i < Caminho.Length; i++)
            {
                foreach (FileInfo arquivo in pipeline_core_util.Arquivos.listaArquivosPasta(Caminho[i]))
                {
                    Scripts script = new Scripts(arquivo.Name, arquivo.FullName);
                    if (!retorno.Contains(script.info.baseDados))
                        retorno.Add(script.info.baseDados);
                }
            }

            return retorno;
        }

        public static int[] GetVersoesBases(List<Scripts> scripts)
        {
            int[] retorno = new int[scripts.Count];
            for (int i = 0; i < scripts.Count; i++)
                retorno[i] = scripts[i].info.versao;

            return retorno;
        }
    }
}
