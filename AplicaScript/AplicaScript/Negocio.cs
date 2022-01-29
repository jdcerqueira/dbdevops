using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicaScript
{
    public class Negocio
    {
        // Configuracao
        



        //public void iniciaProcessoDevops()
        //{
        //    if (!this.verificaBaseControladoraNoAmbiente())
        //    {
        //        this.criaBaseControladora();
        //        this.criaEstruturaTabelaVersionamento();
        //    }
        //}

        

        //public Infos.Scripts.ScriptCompleto scriptCompletoArquivo()
        //{
        //    System.IO.DirectoryInfo directoryInfoScriptFull = Infos.Arquivo.caminhoCompleto("\\" + this.scriptCompleto);

        //    if (directoryInfoScriptFull.GetFiles(arquivoScriptCompleto).Length == 0 ||
        //        !System.IO.File.Exists(directoryInfoScriptFull.GetFiles(arquivoScriptCompleto)[0].FullName))
        //        return new Infos.Scripts.ScriptCompleto()
        //        {
        //            arquivo = new Infos.Scripts()
        //            {
        //                filename ="",
        //                fullpath =""
        //            }
        //        };

        //    return new Infos.Scripts.ScriptCompleto()
        //    {
        //        arquivo = new Infos.Scripts()
        //        {
        //            filename = directoryInfoScriptFull.GetFiles(arquivoScriptCompleto)[0].Name,
        //            fullpath = directoryInfoScriptFull.GetFiles(arquivoScriptCompleto)[0].FullName
        //        }
        //    };
        //}

        

        //public void incrementaScriptCompleto()
        //{
        //    String str_arquivoScriptCompleto = this.scriptCompletoArquivo().arquivo.fullpath != "" ? this.scriptCompletoArquivo().arquivo.fullpath : Infos.Arquivo.caminhoCompleto(arquivoScriptCompleto,"\\" + this.scriptCompleto) ;
        //    StringBuilder sb_arquivoScriptCompleto = Infos.Arquivo.conteudoArquivo(str_arquivoScriptCompleto);
        //    StringBuilder sb_scriptsAplicados = new StringBuilder();

        //    // Percorre os arquivos para aplicar
        //    foreach (Infos.Scripts script in this.scriptsParaAplicar().arquivos)
        //    {
        //        StringBuilder aplicar = Infos.Arquivo.conteudoArquivo(script.fullpath);
        //        sb_scriptsAplicados.AppendLine("/*" + script.filename + "*/");
        //        sb_scriptsAplicados.AppendLine(aplicar.ToString());
        //        Infos.Arquivo.moveArquivo(script.fullpath, Infos.Arquivo.caminhoCompleto(script.filename, "\\" + this.scriptAplicado));
        //    }

        //    sb_arquivoScriptCompleto.AppendLine(sb_scriptsAplicados.ToString());
        //    Infos.Arquivo.escreveArquivo(str_arquivoScriptCompleto, sb_arquivoScriptCompleto.ToString());
        //}

        
    }
}
