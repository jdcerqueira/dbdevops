using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using pipeline_core_ControlDBDevops;
using pipeline_core_log;

namespace pipeline_core
{
    public class Configuracao
    {
        //Dados do arquivo config
        public String branch = "";
        public String connection = "";
        public String aplicaScript = "";
        public String scriptCompleto = "";
        public String scriptAplicado = "";
        public String log = "";
        public String usuarioBase = "";
        public String senhaBase = "";
        public String pastaBaseVersionadora = "";

        public Configuracao()
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "Inicia configuração." });

            //carrega arquivo configuração
            String arquivoConfig = pipeline_core_util.Arquivos.carregaConteudoArquivo(pipeline_core_util.Constantes.arquivoConfiguracao);

            if(arquivoConfig != "")
            {
                var jsonConfig = JObject.Parse(arquivoConfig);

                this.branch = jsonConfig["branch"].ToString();
                this.connection = jsonConfig["connection"].ToString().Replace("/","\\");
                this.aplicaScript = jsonConfig["aplicaScript"].ToString().Replace("/", "\\");
                this.scriptCompleto = jsonConfig["scriptCompleto"].ToString().Replace("/", "\\");
                this.scriptAplicado = jsonConfig["scriptAplicado"].ToString().Replace("/", "\\");
                this.log = jsonConfig["log"].ToString().Replace("/", "\\");
                this.pastaBaseVersionadora = jsonConfig["pastaBaseVersionadora"].ToString().Replace("/", "\\");
                this.usuarioBase = jsonConfig["usuarioBase"].ToString();
                this.senhaBase = pipeline_core_util.Criptografia.Decrypt(jsonConfig["senhaBase"].ToString(), pipeline_core_util.Constantes.keyCripto, true);
            }
            
            // Valida as pastas informadas
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaDiretorio", $"{this.aplicaScript}" });
            pipeline_core_util.Arquivos.validaDiretorio(this.aplicaScript);

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaDiretorio", $"{this.scriptCompleto}" });
            pipeline_core_util.Arquivos.validaDiretorio(this.scriptCompleto);

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaDiretorio", $"{this.scriptAplicado}" });
            pipeline_core_util.Arquivos.validaDiretorio(this.scriptAplicado);

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaDiretorio", $"{this.pastaBaseVersionadora}" });
            pipeline_core_util.Arquivos.validaDiretorio(this.pastaBaseVersionadora);

            // Cria os arquivos de scripts para a base versionadora
            new ControlDBDevops(this.connection, this.usuarioBase, this.senhaBase, this.pastaBaseVersionadora);
        }

        public Dictionary<String, int> GetDctScriptsExecutados()
        {
            Dictionary<String, int> retorno = null;
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Util.Arquivos.listaArquivosPasta", "" });

            try
            {
                retorno = new Dictionary<String, int>();
                foreach (FileInfo arquivo in pipeline_core_util.Arquivos.listaArquivosPasta(this.scriptAplicado))
                {
                    Scripts script = new Scripts(arquivo.Name, arquivo.FullName);

                    if (retorno.ContainsKey(script.info.baseDados))
                        retorno[script.info.baseDados] = script.info.versao;
                    else
                        retorno.Add(script.info.baseDados, script.info.versao);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Util.Arquivos.listaArquivosPasta", ex.Message });
            }

            return retorno;
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
                $"Pasta base versionadora: {this.pastaBaseVersionadora}";
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
            String _senhaBase,
            String _pastaBaseVersionadora)
        {
            StringBuilder configuracao = new StringBuilder();

            configuracao.AppendLine("{");
            configuracao.AppendLine($"\"branch\":\"{_branch}\"");
            configuracao.AppendLine($",\"connection\":\"{_connection}\"");
            configuracao.AppendLine($",\"aplicaScript\":\"{_aplicaScript}\"");
            configuracao.AppendLine($",\"scriptCompleto\":\"{_scriptCompleto}\"");
            configuracao.AppendLine($",\"scriptAplicado\":\"{_scriptAplicado}\"");
            configuracao.AppendLine($",\"log\":\"{_log}\"");
            configuracao.AppendLine($",\"baseControladora\":\"{_baseControladora}\"");
            configuracao.AppendLine($",\"usuarioBase\":\"{_usuarioBase}\"");
            configuracao.AppendLine($",\"senhaBase\":\"{pipeline_core_util.Criptografia.Encrypt(_senhaBase, pipeline_core_util.Constantes.keyCripto, true)}\"");
            configuracao.AppendLine($",\"pastaBaseVersionadora\":\"{_pastaBaseVersionadora}\"");
            configuracao.AppendLine("}");

            pipeline_core_util.Arquivos.gravaArquivo(pipeline_core_util.Constantes.arquivoConfiguracao, configuracao.ToString().Replace("\\","/"));
        }
    }
}
