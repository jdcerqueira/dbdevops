using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pipeline_config
{
    public class Scripts
    {
        public String nomeArquivo { get; set; }
        public String caminhoArquivo { get; set; }

        public class ScriptsAplicados
        {
            public List<Scripts> scripts { get; set; }
        }

        public class Info
        {
            public String baseDados;
            public int versao;

            public Info(Scripts _script)
            {
                this.baseDados = _script.nomeArquivo.Split('_')[1].Replace(".sql","");
                this.versao = Int32.Parse(_script.nomeArquivo.Split('_')[0].Replace("v", ""));
            }
        }

        /*
         * 
         * Lista os scripts que estão disponíveis na pasta aplicados.
         * Para listagem dos itens, será considerada uma versão de aplicados inicial.
         * Esta versão inicial, seria a versão já encontrada no banco de dados, resultado na não execução das versões anteriores
         * 
         */
        public static Scripts.ScriptsAplicados listaScriptsAplicados(Configuracao _configuracao, Dictionary<String,int> _versoesAplicadas)
        {
            Scripts.ScriptsAplicados aplicados = new ScriptsAplicados();
            aplicados.scripts = new List<Scripts>();
            foreach (FileInfo arquivos in Util.Arquivos.listaArquivosPasta(_configuracao.scriptAplicado))
            {
                Scripts script = new Scripts { nomeArquivo = arquivos.Name, caminhoArquivo = arquivos.FullName };
                Scripts.Info info = new Scripts.Info(script);
                int _versaoInicial = 0;
                _versoesAplicadas.TryGetValue(info.baseDados, out _versaoInicial);
                if(info.versao > _versaoInicial)
                    aplicados.scripts.Add(script);
            }
            return aplicados;
        }
    }
}
