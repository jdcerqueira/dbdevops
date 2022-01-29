using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pipeline
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


        public static Scripts.ScriptsAplicados listaScriptsAplicados(Configuracao _configuracao)
        {
            Scripts.ScriptsAplicados aplicados = new ScriptsAplicados();
            aplicados.scripts = new List<Scripts>();
            foreach (FileInfo arquivos in Util.Arquivos.listaArquivosPasta(_configuracao.scriptAplicado))
            {
                aplicados.scripts.Add(new Scripts { nomeArquivo = arquivos.Name, caminhoArquivo = arquivos.FullName});
            }
            return aplicados;
        }
    }
}
