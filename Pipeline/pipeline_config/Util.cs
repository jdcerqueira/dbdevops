﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace pipeline_config
{
    public class Util
    {

        public class Constantes
        {
            public const String arquivoConfiguracao = "config";
            public const String keyCripto = "abc";

            public class QueriesEstaticas
            {
                public static String verificaBaseControladora(Configuracao _configuracao)
                {
                    return $"SELECT DB_ID('{_configuracao.baseControladora}')";
                }

                public static String criaBaseControladora(Configuracao _configuracao)
                {
                    return $"CREATE DATABASE {_configuracao.baseControladora}";
                }

                public static String criaEstruturaBaseControladora()
                {
                    return $"CREATE TABLE dbo.Versionamento" +
                        $"(" +
                        $"Versao_Script INT," +
                        $"Script VARCHAR(MAX)," +
                        $"Base_Dados SYSNAME," +
                        $"Data_Execucao DATETIME DEFAULT GETDATE()," +
                        $"Usuario SYSNAME," +
                        $"Servidor SYSNAME," +
                        $"CONSTRAINT PK_Versionamento PRIMARY KEY(Versao_Script,Base_Dados))";
                }

                public static String versaoAtualAmbiente()
                {
                    return $"SELECT MAX(Versao_Script) UltimaVersao, Base_Dados FROM dbo.Versionamento GROUP BY Base_Dados";
                }

                public static String aplicaVersaoScript(Scripts.Info _info, String conteudoScript, Configuracao _configuracao)
                {
                    return "INSERT INTO dbo.Versionamento" +
                        "(Versao_Script, " +
                        "Script, " +
                        "Base_Dados, " +
                        "Usuario, " +
                        "Servidor)" +
                        "VALUES(" +
                        $"{_info.versao}," +
                        $"'{Util.Criptografia.Encrypt(conteudoScript,Util.Constantes.keyCripto,true)}'," +
                        $"'{_info.baseDados}'," +
                        $"'{_configuracao.usuarioBase}'," +
                        $"'{_configuracao.connection}'" +
                        ")";
                }
            }
        }

        public class Arquivos
        {
            public static String[] carregaConteudoArquivo(String arquivo)
            {
                validaArquivo(arquivo);
                return File.ReadAllLines(arquivo);
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
                if(validaDiretorio(caminhoPasta))
                    return new DirectoryInfo(caminhoPasta).GetFiles("*.sql").OrderBy(file=>file.Name).ToArray();

                return new FileInfo[1];
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
                File.WriteAllText(caminho,conteudo);
            }
        }

        public class Criptografia
        {
            public static String Encrypt(String toEncrypt, String key, Boolean useHashing)
            {
                Byte[] keyArray;
                Byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }

            public static String Decrypt(String toDecrypt, String key, Boolean useHashing)
            {
                Byte[] keyArray;
                Byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }
    }
}
