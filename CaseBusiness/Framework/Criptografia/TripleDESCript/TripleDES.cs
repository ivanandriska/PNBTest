using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;

namespace CaseBusiness.Framework.Criptografia.TripleDESCript
{
    internal class TripleDES
    {
        #region Construtores
        public TripleDES()
        {
            try
            {
                String pathPci = "";

                pathPci = CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig + @"/configuracao_pci.xml";

                //if (CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao == TipoAplicacao.Web)
                //    pathPci = AppDomain.CurrentDomain.BaseDirectory + @"/configuracao_pci.xml";
                //else
                //    pathPci = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location) + @"/configuracao_pci.xml";

				if (!Configuracao.Configuracao.pciCarregado)
					CarregarConfiguracao(pathPci, "");
            }
            catch (System.Exception ex)
            {
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}
        }

        public TripleDES(String pathPci, String tipoCriptografia)
        {
            try
            {
				if (!Configuracao.Configuracao.pciCarregado)
					CarregarConfiguracao(pathPci, tipoCriptografia);
            }
            catch (System.Exception ex)
            {
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}
        }

        public TripleDES(String chaveCriptografia)
        {
            try
            {
                _chaveAUtilizar = chaveCriptografia;
                _utilizarChaveTemporaria = true;
            }
            catch (System.Exception ex)
            {
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}
        }
        #endregion Construtores
        
        #region Atributos
        private String _chaveArmazenada = "";
        private String _chaveAUtilizar = "";
        private String _chaveArmazenadaSecundaria = "";
        private String _dataExpiracaoPCI = "";
        private String _tipoCriptografia = "";
        private Byte[] _IV = new Byte[] {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
        private Byte[] _key = Encoding.UTF8.GetBytes("10201123");
		private String akeyAEs = "~8|,S^'K$D}yh$C!E4;v5M'^lYb87Mdi";
        private Boolean _utilizarChaveTemporaria = false;
		#endregion Atributos

		#region Propriedades

		public String TipoCriptografia
        {
          get { return _tipoCriptografia; }
        }

        public String DataExpiracaoPCI
        {
          get { return _dataExpiracaoPCI; }
        }

        #endregion Propriedades

        /// <summary>
        /// Codificação para uso geral (exceto decodificação de arquivos internos ao CaseBusiness)
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public String Codificar(String valor)
        {
            String Triple_DES_Codificar = "";

            try
            {
                if (!_utilizarChaveTemporaria)
                {
                    if (_tipoCriptografia == "F")
                        _chaveAUtilizar = Configuracao.Configuracao.aNova_Chave;
                    else
                        _chaveAUtilizar = Configuracao.Configuracao.aChave_Atual;
                }

                Triple_DES_Codificar = TripleDESCodificar(valor);
            }
            catch (System.Exception ex)
            {
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return Triple_DES_Codificar;
        }

        /// <summary>
        /// Decodificação para uso geral (exceto decodificação de arquivos internos ao CaseBusiness)
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public String Decodificar(String valor)
        {
            String mTriple_DES_DeCodificar = "";

            try
            {
                if (!_utilizarChaveTemporaria)
                {
                    if (_tipoCriptografia == "F")
                        _chaveAUtilizar = Configuracao.Configuracao.aNova_Chave;
                    else
                        _chaveAUtilizar = Configuracao.Configuracao.aChave_Atual;
                }
                mTriple_DES_DeCodificar = TripleDESDecodificar(valor);
            }
            catch (System.Exception ex)
            {
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}
            
            return mTriple_DES_DeCodificar;
        }
        
        private String CarregarConfiguracao(String pathConfiguracao, String tipoCriptografia)
        {
            String novaChave = "";
            DataSet cnConsulta = new DataSet();

            try
            {
                cnConsulta.ReadXml(pathConfiguracao);

                foreach (DataRow cnLinha in cnConsulta.Tables[0].Rows)
                {
					Configuracao.Configuracao.aChave_Atual = Descriptografar(cnLinha[0].ToString());
					Configuracao.Configuracao.aNova_Chave = Descriptografar(cnLinha[1].ToString());
					Configuracao.Configuracao.aSituacao = Convert.ToByte(Descriptografar(cnLinha[2].ToString()));
					Configuracao.Configuracao.aData_Validade = Descriptografar(cnLinha[3].ToString());
					Configuracao.Configuracao.aData_Registro = Descriptografar(cnLinha[4].ToString());

                    if (cnConsulta.Tables[0].Columns.Count > 5)
                        Configuracao.Configuracao.ComplementaryCryptType = (CaseBusiness.Framework.ComplementaryCryptType)Convert.ToInt32(Descriptografar(cnLinha[5].ToString()));
                    else
                        Configuracao.Configuracao.ComplementaryCryptType = CaseBusiness.Framework.ComplementaryCryptType.TripleDES;
                }

                if (tipoCriptografia == "F")
                {
                    novaChave = Configuracao.Configuracao.aNova_Chave;
                    return novaChave;
                }

                if (tipoCriptografia == "P")
                {
                    novaChave = Configuracao.Configuracao.aChave_Atual;
                    return novaChave;
                }

                if (new DateTime(Convert.ToInt32(Configuracao.Configuracao.aData_Validade.Substring(0, 4)), Convert.ToInt32(Configuracao.Configuracao.aData_Validade.Substring(4, 2)), Convert.ToInt32(Configuracao.Configuracao.aData_Validade.Substring(6, 2))) > DateTime.Now)
                {
                    if (Configuracao.Configuracao.aSituacao == 0)
                        _tipoCriptografia = "N";
                    else
                        _tipoCriptografia = "P";
                }
                else
                    _tipoCriptografia = "F";

                _chaveArmazenadaSecundaria = Configuracao.Configuracao.aChave_Atual;
                _chaveArmazenada = Configuracao.Configuracao.aNova_Chave;
				Configuracao.Configuracao.aData_Expiracao_PCI = Configuracao.Configuracao.aData_Registro;
				Configuracao.Configuracao.pciCarregado = true;

			}
            catch (System.Exception ex)
            {
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return novaChave;
        }

        /// <summary>
        /// Usado apenas para descriptografia de arquivos de configuração interno ao CaseBusiness
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        internal String Descriptografar(String valor)
        {
			String mDescriptografar = "";
			String cripto = "DES";

			if (valor.Split(new String[] { "==" }, StringSplitOptions.None).Length > 1)
			{
				if (valor.Split(new String[] { "==" }, StringSplitOptions.None)[1].Length > 0)
					cripto = "AES";
			}

			if (cripto == "AES")
			{
				System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
                System.Security.Cryptography.SHA256 SHA256 = SHA256.Create();

				String iv = "";

				try
				{
					String[] ivct = valor.Split(new String[] { "==" }, StringSplitOptions.None);
					iv = ivct[0] + "==";

					valor = (ivct.Length == 3 ? ivct[1] + "==" : ivct[1]);
					AES.Key = SHA256.ComputeHash(ASCIIEncoding.ASCII.GetBytes(akeyAEs));
					AES.IV = Convert.FromBase64String(iv);
					AES.Mode = CipherMode.CBC;
					System.Security.Cryptography.ICryptoTransform DESDecrypter = AES.CreateDecryptor();
					Byte[] Buffer = Convert.FromBase64String(valor);

					mDescriptografar = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
				}
				catch (System.Exception ex)
				{
					Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
				}
			}
			else
			{
				try
				{
					Byte[] inputByteArray = new Byte[valor.Length];
					DESCryptoServiceProvider Obj_Des = new DESCryptoServiceProvider();
					MemoryStream Obj_ms = new MemoryStream();
					CryptoStream Obj_cs = new CryptoStream(Obj_ms, Obj_Des.CreateDecryptor(_key, _IV), CryptoStreamMode.Write);

					try
					{
						inputByteArray = Convert.FromBase64String(valor);
						Obj_cs.Write(inputByteArray, 0, inputByteArray.Length);
						Obj_cs.FlushFinalBlock();
						Encoding encoding = System.Text.Encoding.UTF8;
						mDescriptografar = encoding.GetString(Obj_ms.ToArray());
					}
					catch (System.Exception exc)
					{
						Log.Log.LogarArquivo("Erro: " + exc.Message + " " + exc.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
					}
				}
				catch (System.Exception ex)
				{
					Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
				}
			}

			return mDescriptografar;
		}

        /// <summary>
        /// Função de codificação Triple DES
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private String TripleDESCodificar_Depreciado(String valor)
        {
            Byte[] encryptedBytes = null;

            try
            {
                MemoryStream ms = null;
                CryptoStream encStream = null;

                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(_chaveAUtilizar, new Byte[0]); //---original
                des.IV = new Byte[8]; //---original
                des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new Byte[8]); //---original

                ms = new MemoryStream((valor.Length * 2) - 1);
                encStream = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write); //---original

                Byte[] plainBytes = Encoding.UTF8.GetBytes(valor);

                encStream.Write(plainBytes, 0, plainBytes.Length);
                encStream.FlushFinalBlock();

                encryptedBytes = new Byte[ms.Length];
                ms.Position = 0;
                ms.Read(encryptedBytes, 0, Convert.ToInt32(ms.Length));

                encStream.Close();
                encStream.Dispose();
                encStream = null;
            }
            catch (System.Exception ex)
            {
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Função de codificação Triple DES
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public string TripleDESCodificar(string TextToEncrypt)
        {
            byte[] MyresultArray = null;

            try
            {
                byte[] MyEncryptedArray = UTF8Encoding.UTF8.GetBytes(TextToEncrypt);

                MD5CryptoServiceProvider MyMD5CryptoService = new MD5CryptoServiceProvider();

                byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_chaveAUtilizar));

                MyMD5CryptoService.Clear();

                var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();

                MyTripleDESCryptoService.IV = new Byte[8];
                MyTripleDESCryptoService.Key = MysecurityKeyArray;

                var MyCrytpoTransform = MyTripleDESCryptoService.CreateEncryptor();

                MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyEncryptedArray, 0, MyEncryptedArray.Length);

                MyTripleDESCryptoService.Clear();
            }
            catch (Exception ex)
            {
                Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
            }

            return Convert.ToBase64String(MyresultArray, 0,MyresultArray.Length);
        }


        /// <summary>
        /// Função de decodificação Triple DES
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public string TripleDESDecodificar(string TextToDecrypt)
        {
            byte[] MyresultArray = null;

            try
            {
                byte[] MyDecryptArray = Convert.FromBase64String(TextToDecrypt);

                MD5CryptoServiceProvider MyMD5CryptoService = new MD5CryptoServiceProvider();

                byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_chaveAUtilizar));

                MyMD5CryptoService.Clear();

                var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();

                MyTripleDESCryptoService.IV = new Byte[8];
                MyTripleDESCryptoService.Key = MysecurityKeyArray;

                var MyCrytpoTransform = MyTripleDESCryptoService.CreateDecryptor();

                MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyDecryptArray, 0, MyDecryptArray.Length);

                MyTripleDESCryptoService.Clear();
            }
            catch (Exception ex)
            {
                Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
            }

            return UTF8Encoding.UTF8.GetString(MyresultArray);
        }

        /// <summary>
        /// Função de decodificação Triple DES
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        private String TripleDESDecodificar_Depreciado(String valor)
        {
            Byte[] plainBytes = null;

            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(_chaveAUtilizar, new Byte[0]);

                des.IV = new Byte[8];

                des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new Byte[8]);

                Byte[] encryptedBytes = Convert.FromBase64String(valor);

                MemoryStream ms = new MemoryStream(valor.Length);
                CryptoStream decStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

                decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                decStream.FlushFinalBlock();

                plainBytes = new Byte[ms.Length];
                ms.Position = 0;
                ms.Read(plainBytes, 0, Convert.ToInt32(ms.Length));

                decStream.Close();
                decStream.Dispose();
                decStream = null;
            }
            catch (System.Exception ex)
            {
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
