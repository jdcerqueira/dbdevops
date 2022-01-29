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
            //Verifica a base versionadora
            if (!existeBaseControladora(_configuracao))
            {
                criaBaseControladora(_configuracao);
                criaEstruturaBaseControladora(_configuracao);
            }

            //Caso a base versionadora esteja vazia, serão aplicados todos os scripts existentes na pasta ´ScriptsAplicados´
            if(versaoAtualAmbiente(_configuracao) == 0)
            {
                foreach(Scripts aplicado in Scripts.listaScriptsAplicados(_configuracao).scripts)
                    aplicaScript(_configuracao, aplicado, new Scripts.Info(aplicado));
            }
        }

        private Boolean existeBaseControladora(Configuracao _configuracao)
        {
            SqlDataReader result = DAO.executaComandoResultSet(DAO.connection(_configuracao), Util.Constantes.QueriesEstaticas.verificaBaseControladora(_configuracao));
            if (result.Read())
                if (result[0].ToString() != "")
                    return true;

            return false;
        }

        private void criaBaseControladora(Configuracao _configuracao)
        {
            DAO.executaComando(DAO.connection(_configuracao), Util.Constantes.QueriesEstaticas.criaBaseControladora(_configuracao));
        }

        private void criaEstruturaBaseControladora(Configuracao _configuracao)
        {
            DAO.executaComando(DAO.connection(_configuracao), Util.Constantes.QueriesEstaticas.criaEstruturaBaseControladora());
        }

        private int versaoAtualAmbiente(Configuracao _configuracao)
        {
            SqlDataReader result = DAO.executaComandoResultSet(DAO.connection(_configuracao,_configuracao.baseControladora), Util.Constantes.QueriesEstaticas.versaoAtualAmbiente());
            if (result == null)
                return 0;

            result.Read();
            return Int32.Parse(result[0].ToString());
        }

        private void aplicaScript(Configuracao _configuracao, Scripts _script, Scripts.Info _info)
        {
            StringBuilder conteudoArquivo = new StringBuilder();
            foreach (String linha in Util.Arquivos.carregaConteudoArquivo(_script.caminhoArquivo))
                conteudoArquivo.AppendLine(linha);

            DAO.executaComando(DAO.connection(_configuracao),conteudoArquivo.ToString());
            DAO.executaComando(DAO.connection(_configuracao, _configuracao.baseControladora), Util.Constantes.QueriesEstaticas.aplicaVersaoScript(_info, conteudoArquivo.ToString(), _configuracao));
        }
    }
}
