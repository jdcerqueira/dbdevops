using System;
using System.Linq;
using System.IO;
using pipeline_core_log;

namespace pipeline_core_util
{
    public class Arquivos
    {
        public static void complementaArquivo(String arquivoOrigem, String arquivoDestino)
        {
            try
            {
                Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "validaArquivo", $"{arquivoOrigem} -> {arquivoDestino}" });
                validaArquivo(arquivoDestino);

                Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "carregaConteudoArquivo", $"{arquivoOrigem} -> {arquivoDestino}" });
                String contentArquivoOrigem = $"-- {DateTime.Now.ToString()} **** Script aplicado - {arquivoOrigem} ****\n" + carregaConteudoArquivo(arquivoOrigem) + "\n\n";

                Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "File.AppendAllText", $"{arquivoOrigem} -> {arquivoDestino}" });
                File.AppendAllText(arquivoDestino, contentArquivoOrigem);
            }
            catch (Exception ex)
            {
                Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "ERRO", ex.Message });
            }
        }

        public static void moveArquivo(String arquivoOrigem, String arquivoDestino)
        {
            File.Move(arquivoOrigem, arquivoDestino);
        }


        public static String carregaConteudoArquivo(String arquivo)
        {
            validaArquivo(arquivo);
            return File.ReadAllText(arquivo);
        }

        public static void validaArquivo(String arquivo)
        {
            if (!File.Exists(arquivo))
            {
                var criado = File.Create(arquivo);
                criado.Close();
            }
        }

        public static FileInfo[] listaArquivosPasta(String caminhoPasta)
        {
            Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "Util.Arquivos.validaDiretorio", $"Valida a existência da pasta - {caminhoPasta}" });

            if (validaDiretorio(caminhoPasta))
                return new DirectoryInfo(caminhoPasta).GetFiles("*.sql").OrderBy(file => file.Name).ToArray();

            Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "Retorno", $"Não foram encontrados arquivos na pasta - {caminhoPasta}" });
            return new FileInfo[] { null };
        }

        public static Boolean validaDiretorio(String caminhoPasta)
        {
            if (caminhoPasta == null || caminhoPasta == "")
                return false;

            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);

            return true;
        }

        public static void gravaArquivo(String caminho, String conteudo)
        {
            try
            {
                Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "validaArquivo", $"Valida a existência do arquivo - {caminho}" });
                validaArquivo(caminho);
                File.WriteAllText(caminho, conteudo);
            }
            catch (Exception ex)
            {
                Log.registraLog(new String[] { "Util.Arquivos", System.Reflection.MethodBase.GetCurrentMethod().Name, "ERRO", ex.Message });
                throw new Exception(ex.StackTrace);
            }
        }
    }
}
