using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pipeline_config
{
    public class Configuracao
    {
        public String branch;
        public String connection;
        public String aplicaScript;
        public String scriptCompleto;
        public String scriptAplicado;
        public String log;
        public String baseControladora;
        public String usuarioBase;
        public String senhaBase;
        public Dictionary<String, int> versaoBaseAplicada = new Dictionary<String, int>();

        public Configuracao()
        {
            //carrega arquivo configuração
            foreach (String linhaArquivo in Util.Arquivos.carregaConteudoArquivo(Util.Constantes.arquivoConfiguracao))
            {
                String[] infoLinhaArquivo = linhaArquivo.Split('=');

                this.branch = infoLinhaArquivo[0] == "branch" ? infoLinhaArquivo[1] : this.branch;
                this.connection = infoLinhaArquivo[0] == "connection" ? infoLinhaArquivo[1] : this.connection;
                this.aplicaScript = infoLinhaArquivo[0] == "aplicaScript" ? infoLinhaArquivo[1] : this.aplicaScript;
                this.scriptCompleto = infoLinhaArquivo[0] == "scriptCompleto" ? infoLinhaArquivo[1] : this.scriptCompleto;
                this.scriptAplicado = infoLinhaArquivo[0] == "scriptAplicado" ? infoLinhaArquivo[1] : this.scriptAplicado;
                this.log = infoLinhaArquivo[0] == "log" ? infoLinhaArquivo[1] : this.log;
                this.baseControladora = infoLinhaArquivo[0] == "baseControladora" ? infoLinhaArquivo[1] : this.baseControladora;
                this.usuarioBase = infoLinhaArquivo[0] == "usuarioBase" ? infoLinhaArquivo[1] : this.usuarioBase;
                this.senhaBase = infoLinhaArquivo[0] == "senhaBase" ? Util.Criptografia.Decrypt(infoLinhaArquivo[1].Replace("(***Igual***)","="),Util.Constantes.keyCripto,true) : this.senhaBase;
            }

            //preenche dicionário de bases e versionamentos na encontrados na pasta de aplicados
            foreach(FileInfo arquivo in Util.Arquivos.listaArquivosPasta(this.scriptAplicado))
            {
                int versao = 0;
                Scripts.Info info = new Scripts.Info(new Scripts { nomeArquivo = arquivo.Name, caminhoArquivo = arquivo.FullName });

                if(!versaoBaseAplicada.ContainsKey(info.baseDados))
                    versaoBaseAplicada.Add(info.baseDados, info.versao);
                else
                {
                    versaoBaseAplicada.TryGetValue(info.baseDados, out versao);
                    if (versao < info.versao)
                        versaoBaseAplicada[info.baseDados] = info.versao;
                }
            }
        }

        override
        public String ToString()
        {
            return $"Branch: {this.branch}\n" +
                $"Connection: {this.connection}\n" +
                $"AplicaScript: {this.aplicaScript}\n" +
                $"ScriptCompleto: {this.scriptCompleto}\n" +
                $"ScriptAplicado: {this.scriptAplicado}\n" +
                $"Log: {this.log}\n" +
                $"BaseControladora: {this.baseControladora}";
        }

        public static void salvarConfiguracao
            (String _branch,
            String _connection,
            String _aplicaScript,
            String _scriptCompleto,
            String _scriptAplicado,
            String _log,
            String _baseControladora,
            String _usuarioBase,
            String _senhaBase)
        {
            StringBuilder configuracao = new StringBuilder();
            configuracao.AppendLine($"branch={_branch}");
            configuracao.AppendLine($"connection={_connection}");
            configuracao.AppendLine($"aplicaScript={_aplicaScript}");
            configuracao.AppendLine($"scriptCompleto={_scriptCompleto}");
            configuracao.AppendLine($"scriptAplicado={_scriptAplicado}");
            configuracao.AppendLine($"log={_log}");
            configuracao.AppendLine($"baseControladora={_baseControladora}");
            configuracao.AppendLine($"usuarioBase={_usuarioBase}");
            configuracao.AppendLine($"senhaBase={Util.Criptografia.Encrypt(_senhaBase,Util.Constantes.keyCripto,true).Replace("=","(***Igual***)")}");

            Util.Arquivos.gravaArquivo(Util.Constantes.arquivoConfiguracao,configuracao.ToString());
        }
    }
}
