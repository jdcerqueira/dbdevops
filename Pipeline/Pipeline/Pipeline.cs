using pipeline_config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipeline
{
    public class Pipeline
    {
        public Pipeline(Configuracao _configuracao)
        {
            /*
             * Neste trecho inicial, o pipeline irá organizar a base de dados controladora de versões.
             * Serão aplicadas as versões que deveriam já existir no servidor e na base de dados informada no script.
             * Após o ajuste no ambiente, tornando o ambiente confiável as versões já existentes na base de dados, iremos para o fluxo de aplicar os scripts pendentes.
             */

            //Verifica a base versionadora
            if (!existeBaseControladora(_configuracao))
            {
                criaBaseControladora(_configuracao);
                criaEstruturaBaseControladora(_configuracao);
            }

            // Aplica os scripts já controlados pelo repositório como "Aplicados" na base de dados em questão
            foreach (Scripts aplicado in Scripts.listaScriptsAplicados(_configuracao, versaoAtualAmbiente(_configuracao)).scripts)
                aplicaScript(_configuracao, aplicado, new Scripts.Info(aplicado));

            /**********************************
             * Termina aqui o primeiro trecho *
             **********************************/

            /*
             * Trecho de execução dos scripts pendentes.
             */

            // Verifica se os scripts para aplicar estão na versão correta
            if (verificaVersaoScriptParaAplicar(_configuracao))
            {
                foreach (var item in _configuracao.arquivosScriptsParaAplicar.scripts)
                    Console.WriteLine($"Script para aplicar: {item.nomeArquivo} - {item.caminhoArquivo}");
            }
        }

        private Boolean verificaVersaoScriptParaAplicar(Configuracao _configuracao)
        {
            return _configuracao.arquivosScriptsParaAplicar.scripts.Count > 0;
        }

        private Boolean existeBaseControladora(Configuracao _configuracao)
        {
            // Precisa aplicar o log aqui
            Console.WriteLine($"Verificando se a base {_configuracao.baseControladora} existe...");

            SqlDataReader result = DAO.executaComandoResultSet(DAO.connection(_configuracao), Util.Constantes.QueriesEstaticas.verificaBaseControladora(_configuracao));
            if (result.Read())
                if (result[0].ToString() != "")
                {
                    // Precisa aplicar o log aqui
                    Console.WriteLine($"A base {_configuracao.baseControladora} existe.");
                    return true;
                }

            // Precisa aplicar o log aqui
            Console.WriteLine($"A base {_configuracao.baseControladora} não existe.");
            return false;
        }

        private void criaBaseControladora(Configuracao _configuracao)
        {
            // Precisa aplicar o log aqui
            Console.WriteLine($"Criando a base {_configuracao.baseControladora}...");
            
            DAO.executaComando(DAO.connection(_configuracao), Util.Constantes.QueriesEstaticas.criaBaseControladora(_configuracao));

            // Precisa aplicar o log aqui
            Console.WriteLine($"Base {_configuracao.baseControladora} criada com sucesso.");
        }

        private void criaEstruturaBaseControladora(Configuracao _configuracao)
        {
            // Precisa aplicar o log aqui
            Console.WriteLine($"Criando estrutura de versionamento na base {_configuracao.baseControladora}...");

            DAO.executaComando(DAO.connection(_configuracao, _configuracao.baseControladora), Util.Constantes.QueriesEstaticas.criaEstruturaBaseControladora());

            // Precisa aplicar o log aqui
            Console.WriteLine($"Estrutura de versionamento na base {_configuracao.baseControladora} criada com sucesso.");
        }

        private Dictionary<String, int> versaoAtualAmbiente(Configuracao _configuracao)
        {
            // Precisa aplicar o log aqui
            Console.WriteLine($"Pesquisa de versão dos scripts já aplicados no servidor {_configuracao.connection}.");
            Dictionary<String, int> retorno = new Dictionary<String, int>();

            SqlDataReader result = DAO.executaComandoResultSet(DAO.connection(_configuracao,_configuracao.baseControladora), Util.Constantes.QueriesEstaticas.versaoAtualAmbiente());
            if (result == null)
            {
                // Precisa aplicar o log aqui
                Console.WriteLine($"Retornou null na pesquisa.");
                return retorno;
            }

            while (result.Read())
            {
                retorno.Add(result[1].ToString(), Int32.Parse(result[0].ToString()));
                // Precisa aplicar o log aqui
                Console.WriteLine($"Retornou Base: {result[1].ToString()} - Versão: {result[0].ToString()} na pesquisa.");
            }
                

            return retorno;
        }

        private void aplicaScript(Configuracao _configuracao, Scripts _script, Scripts.Info _info)
        {
            StringBuilder conteudoArquivo = new StringBuilder();
            foreach (String linha in Util.Arquivos.carregaConteudoArquivo(_script.caminhoArquivo))
                conteudoArquivo.AppendLine(linha);

            DAO.executaComando(DAO.connection(_configuracao),conteudoArquivo.ToString());
            // Precisa aplicar o log aqui
            Console.WriteLine($"Aplicado o script {_script.nomeArquivo}.");

            DAO.executaComando(DAO.connection(_configuracao, _configuracao.baseControladora), Util.Constantes.QueriesEstaticas.aplicaVersaoScript(_info, conteudoArquivo.ToString(), _configuracao));
            // Precisa aplicar o log aqui
            Console.WriteLine($"Registrada a versão do script {_script.nomeArquivo}.");
        }
    }
}
