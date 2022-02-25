using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Data.SqlClient;

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
        //public Scripts scriptBaseVersionadora = null;
        public const String baseControladora = "ControlDBDevops";
        public String usuarioBase = "";
        public String senhaBase = "";
        public String pastaBaseVersionadora = "";

        //Dados da base versionadora
        public const String loginControladora = "lgn_dbcontrol";
        public const String usuarioControladora = "usr_dbcontrol";
        public const String senhaControladora = "lgn_dbcontrol_99";

        const String replaceIgual = "%3euue3%";

        public Configuracao()
        {
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "", "Inicia configuração." });

            //carrega arquivo configuração
            foreach (String linhaArquivo in Util.Arquivos.carregaConteudoArquivo(Util.Constantes.arquivoConfiguracao).Split('\n'))
            {
                String[] infoLinhaArquivo = linhaArquivo.Split('=');

                this.branch = infoLinhaArquivo[0] == "branch" ? infoLinhaArquivo[1] : this.branch;
                this.connection = infoLinhaArquivo[0] == "connection" ? infoLinhaArquivo[1] : this.connection;
                this.aplicaScript = infoLinhaArquivo[0] == "aplicaScript" ? infoLinhaArquivo[1] : this.aplicaScript;
                this.scriptCompleto = infoLinhaArquivo[0] == "scriptCompleto" ? infoLinhaArquivo[1] : this.scriptCompleto;
                this.scriptAplicado = infoLinhaArquivo[0] == "scriptAplicado" ? infoLinhaArquivo[1] : this.scriptAplicado;
                this.log = infoLinhaArquivo[0] == "log" ? infoLinhaArquivo[1] : this.log;
                this.pastaBaseVersionadora = infoLinhaArquivo[0] == "pastaBaseVersionadora" ? @infoLinhaArquivo[1].ToString() : this.pastaBaseVersionadora;
                this.usuarioBase = infoLinhaArquivo[0] == "usuarioBase" ? infoLinhaArquivo[1] : this.usuarioBase;
                this.senhaBase = infoLinhaArquivo[0] == "senhaBase" ? Util.Criptografia.Decrypt(infoLinhaArquivo[1].Replace(replaceIgual, "="), Util.Constantes.keyCripto, true) : this.senhaBase;
            }

            // Valida as pastas informadas
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaDiretorio", $"{this.aplicaScript}" });
            Util.Arquivos.validaDiretorio(this.aplicaScript);

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaDiretorio", $"{this.scriptCompleto}" });
            Util.Arquivos.validaDiretorio(this.scriptCompleto);

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaDiretorio", $"{this.scriptAplicado}" });
            Util.Arquivos.validaDiretorio(this.scriptAplicado);

            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "validaDiretorio", $"{this.pastaBaseVersionadora}" });
            Util.Arquivos.validaDiretorio(this.pastaBaseVersionadora);

            // Cria os arquivos de scripts para a base versionadora
            if (!(this.pastaBaseVersionadora == null || this.pastaBaseVersionadora == ""))
            {
                Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "gravaArquivo", $@"{this.pastaBaseVersionadora}\{Util.Constantes.arquivoScriptBaseVersionadora}" });
                Util.Arquivos.gravaArquivo($@"{this.pastaBaseVersionadora}\{Util.Constantes.arquivoScriptBaseVersionadora}", Util.Constantes.scriptBaseVersionadora());
            }

            //this.scriptBaseVersionadora = new Scripts() { caminhoArquivo = $@"{this.pastaBaseVersionadora}\{Util.Constantes.aquivoScriptBaseVersionadora}" };
        }

        public Dictionary<String, int> GetDctBasesVersoes()
        {

            Dictionary<String, int> retorno = null;
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "executaResultSet", "EXEC dbo.pReturnMaxVersionByDb" });

            try
            {
                SqlDataReader versoesBases = new DAO(this).executaResultSet("EXEC dbo.pReturnMaxVersionByDb", true);
                retorno = new Dictionary<String, int>();
                while (versoesBases.Read())
                    retorno.Add(versoesBases["nmDatabase"].ToString(), Int32.Parse(versoesBases["MaxVersion"].ToString()));

                return retorno;
            }
            catch (Exception ex)
            {
                Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "ERRO", ex.Message });
            }

            return null;
        }

        public Dictionary<String, int> GetDctScriptsExecutados()
        {
            Dictionary<String, int> retorno = null;
            Log.registraLog(new String[] { this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Util.Arquivos.listaArquivosPasta", "" });

            try
            {
                retorno = new Dictionary<String, int>();
                foreach (FileInfo arquivo in Util.Arquivos.listaArquivosPasta(this.scriptAplicado))
                {
                    Scripts script = new Scripts() { nomeArquivo = arquivo.Name, caminhoArquivo = arquivo.FullName };
                    Scripts.Info info = new Scripts.Info(script);

                    if (retorno.ContainsKey(info.baseDados))
                        retorno[info.baseDados] = info.versao;
                    else
                        retorno.Add(info.baseDados, info.versao);
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
                $"BaseControladora: {Configuracao.baseControladora}\n" +
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
            configuracao.AppendLine($"branch={_branch}");
            configuracao.AppendLine($"connection={_connection}");
            configuracao.AppendLine($"aplicaScript={_aplicaScript}");
            configuracao.AppendLine($"scriptCompleto={_scriptCompleto}");
            configuracao.AppendLine($"scriptAplicado={_scriptAplicado}");
            configuracao.AppendLine($"log={_log}");
            configuracao.AppendLine($"baseControladora={_baseControladora}");
            configuracao.AppendLine($"usuarioBase={_usuarioBase}");
            configuracao.AppendLine($"senhaBase={Util.Criptografia.Encrypt(_senhaBase, Util.Constantes.keyCripto, true).Replace("=", replaceIgual)}");
            configuracao.AppendLine($"pastaBaseVersionadora={_pastaBaseVersionadora}");

            Util.Arquivos.gravaArquivo(Util.Constantes.arquivoConfiguracao, configuracao.ToString());
        }
    }
}
