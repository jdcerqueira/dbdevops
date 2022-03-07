using System;
using System.Collections.Generic;
using pipeline_core;
using Microsoft.Data.SqlClient;
using pipeline_core_log;
using pipeline_core_dao;

namespace pipeline
{
    public class Pipeline
    {
        Dictionary<String, int> versoesMaxAplicadasBaseVersionadora;
        Dictionary<String, int> versoesMaxAplicadasScriptsExecutados;

        public Pipeline(Configuracao _configuracao)
        {
            /*
             * Neste trecho inicial, o pipeline irá organizar a base de dados controladora de versões.
             * Serão aplicadas as versões que deveriam já existir no servidor e na base de dados informada no script.
             * Após o ajuste no ambiente, tornando o ambiente confiável as versões já existentes na base de dados, iremos para o fluxo de aplicar os scripts pendentes.
             * 
             */
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "ControlDBDevops.Info.versoesPorBase()", "" });
            this.versoesMaxAplicadasBaseVersionadora = pipeline_core_ControlDBDevops.ControlDBDevops.Info.versoesPorBase();

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "GetDctScriptsExecutados()", "" });
            this.versoesMaxAplicadasScriptsExecutados = _configuracao.GetDctScriptsExecutados();

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "aplicaScriptsPastaAplicados", "" });
            aplicaScriptsPastaAplicados(_configuracao);

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "aplicaScriptsPastaPendentes", "" });
            aplicaScriptsPastaPendentes(_configuracao);
        }

        private void aplicaScriptsPastaPendentes(Configuracao _configuracao)
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "?", $"Aplicará os scripts na pasta {_configuracao.aplicaScript}." });

            foreach (Scripts script in Scripts.GetScripts(new String[] { _configuracao.aplicaScript }))
                Scripts.aplicaScript(script, _configuracao);
        }

        private void aplicaScriptsPastaAplicados(Configuracao _configuracao)
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "aplicaScript", $"Aplicará os scripts na pasta {_configuracao.scriptAplicado}." });

            foreach (var item in this.versoesMaxAplicadasScriptsExecutados)
            {
                if (!this.versoesMaxAplicadasBaseVersionadora.ContainsKey(item.Key))
                    foreach (Scripts script in Scripts.GetScripts(new String[] { _configuracao.scriptAplicado }, item.Key))
                        Scripts.aplicaScript(script);
                else
                    if (this.versoesMaxAplicadasBaseVersionadora[item.Key] < item.Value)
                    foreach (Scripts script in Scripts.GetScripts(new String[] { _configuracao.scriptAplicado }, item.Key))
                    {
                        if (script.info.versao > this.versoesMaxAplicadasBaseVersionadora[item.Key])
                            Scripts.aplicaScript(script);
                    }
            }
        }

        private void validaBaseVersionadora(Configuracao _configuracao)
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "existeBaseVersionadora", "" });

            try
            {
                if (!existeBaseVersionadora(_configuracao))
                    criaBaseControladora(_configuracao);
            }
            catch (Exception ex)
            {
                Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Exception", ex.Message });
            }

        }

        private Boolean existeBaseVersionadora(Configuracao _configuracao)
        {
            //Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "executaResultSet", pipeline_core_util.Constantes.QueriesEstaticas.verificaBaseControladora() });
            // SqlDataReader result = new DAO(_configuracao).executaResultSet(Util.Constantes.QueriesEstaticas.verificaBaseControladora(), false);

            //if (result.Read())
            //    if (result[0].ToString() != "")
            //        return true;

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "Não foi encontrada a base de dados versionadora." });
            return false;
        }

        private void criaBaseControladora(Configuracao _configuracao)
        {
            //Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "executaArquivoScript", $"Criação da base versionadora - {_configuracao.pastaBaseVersionadora}\\{Util.Constantes.arquivoScriptBaseVersionadora}" });
            //new DAO(_configuracao).executaArquivoScript($"{ _configuracao.pastaBaseVersionadora}\\{ Util.Constantes.arquivoScriptBaseVersionadora}", false);
        }
    }
}
