using pipeline_config;
using System;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Pipeline
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
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaBaseVersionadora", "" });
            validaBaseVersionadora(_configuracao);

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "GetDctBasesVersoes()", "" });
            this.versoesMaxAplicadasBaseVersionadora = _configuracao.GetDctBasesVersoes();

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
            
            foreach(Scripts script in Scripts.GetScripts(_configuracao.aplicaScript))
                Scripts.aplicaScript(script, _configuracao, true);
        }

        private void aplicaScriptsPastaAplicados(Configuracao _configuracao)
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "aplicaScript", $"Aplicará os scripts na pasta {_configuracao.scriptAplicado}." });

            foreach (var item in this.versoesMaxAplicadasScriptsExecutados)
            {
                if (!this.versoesMaxAplicadasBaseVersionadora.ContainsKey(item.Key))
                    foreach (Scripts script in Scripts.GetScripts(_configuracao.scriptAplicado, item.Key))
                        Scripts.aplicaScript(script, _configuracao);
                else
                    if(this.versoesMaxAplicadasBaseVersionadora[item.Key] < item.Value)
                        foreach (Scripts script in Scripts.GetScripts(_configuracao.scriptAplicado, item.Key))
                        {
                            Scripts.Info info = new Scripts.Info(script);
                            if(info.versao > this.versoesMaxAplicadasBaseVersionadora[item.Key])
                                Scripts.aplicaScript(script, _configuracao);
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
            catch(Exception ex)
            {
                Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Exception", ex.Message });
            }
            
        }

        private void atualizaConfiguracoesBaseVersionadora(Configuracao configuracao)
        {
        }


        private Boolean existeBaseVersionadora(Configuracao _configuracao)
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "executaResultSet", Util.Constantes.QueriesEstaticas.verificaBaseControladora()});
            SqlDataReader result = new DAO(_configuracao).executaResultSet(Util.Constantes.QueriesEstaticas.verificaBaseControladora(), false);
            if (result.Read())
                if (result[0].ToString() != "")
                    return true;

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "Não foi encontrada a base de dados versionadora."});
            return false;
        }

        private void criaBaseControladora(Configuracao _configuracao)
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "executaArquivoScript", $"Criação da base versionadora - {_configuracao.scriptBaseVersionadora.caminhoArquivo}" });
            new DAO(_configuracao).executaArquivoScript(_configuracao.scriptBaseVersionadora.caminhoArquivo,false);
        }

        //private Dictionary<String, int> versaoAtualAmbiente(Configuracao _configuracao)
        //{
        //    // Precisa aplicar o log aqui
        //    Console.WriteLine($"Pesquisa de versão dos scripts já aplicados no servidor {_configuracao.connection}.");
        //    Dictionary<String, int> retorno = new Dictionary<String, int>();

        //    Microsoft.Data.SqlClient.SqlDataReader result = DAO.executaComandoResultSet(DAO.connection_BaseEspecifica(_configuracao,Configuracao.baseControladora), Util.Constantes.QueriesEstaticas.versaoAtualAmbiente());
        //    if (result == null)
        //    {
        //        // Precisa aplicar o log aqui
        //        Console.WriteLine($"Retornou null na pesquisa.");
        //        return retorno;
        //    }

        //    while (result.Read())
        //    {
        //        retorno.Add(result[1].ToString(), Int32.Parse(result[0].ToString()));
        //        // Precisa aplicar o log aqui
        //        Console.WriteLine($"Retornou Base: {result[1].ToString()} - Versão: {result[0].ToString()} na pesquisa.");
        //    }
                

        //    return retorno;
        //}

        //private void aplicaScript(Configuracao _configuracao, Scripts _script, Scripts.Info _info)
        //{
        //    StringBuilder conteudoArquivo = new StringBuilder();
        //    foreach (String linha in Util.Arquivos.carregaConteudoArquivo(_script.caminhoArquivo))
        //        conteudoArquivo.AppendLine(linha);

        //    //DAO.executaComando(DAO.connection(_configuracao),conteudoArquivo.ToString());
        //    // Precisa aplicar o log aqui
        //    Console.WriteLine($"Aplicado o script {_script.nomeArquivo}.");

        //    //DAO.executaComando(DAO.connection(_configuracao, Configuracao.baseControladora), Util.Constantes.QueriesEstaticas.aplicaVersaoScript(_info, conteudoArquivo.ToString(), _configuracao));
        //    // Precisa aplicar o log aqui
        //    Console.WriteLine($"Registrada a versão do script {_script.nomeArquivo}.");
        //}
    }
}
