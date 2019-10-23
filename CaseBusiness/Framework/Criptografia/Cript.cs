using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;

namespace CaseBusiness.Framework.Criptografia
{

    public class Cript
    {
        private TripleDESCript.TripleDES tDES;
		private CaseBusiness.Framework.ComplementaryCryptType _complementaryCryptType = ComplementaryCryptType.TripleDES;
        private String _chaveInterna = "2rpnet.";

		#region Construtores
		public Cript()
        {
            tDES = new TripleDESCript.TripleDES();
        }

		public Cript(CaseBusiness.Framework.ComplementaryCryptType complementaryCryptType)
		{
			_complementaryCryptType = complementaryCryptType;

			if (complementaryCryptType == ComplementaryCryptType.TripleDES)
				tDES = new TripleDESCript.TripleDES();
		}

		public Cript(Boolean usarChaveInterna)
        {
            if (usarChaveInterna)
                tDES = new TripleDESCript.TripleDES(_chaveInterna);
        }

        public Cript(String pathPCI, String tipoCriptografia)
        {
            tDES = new TripleDESCript.TripleDES(pathPCI, tipoCriptografia);
        }
        #endregion Construtores

        #region Atributos
        #endregion Atributos

        #region Propriedades



        #endregion Propriedades


		public String Codificar(String valor)
		{
			String r = "";

			try
			{
				if (_complementaryCryptType == CaseBusiness.Framework.ComplementaryCryptType.TripleDES)
					r = tDES.Codificar(valor);
				//else if (_complementaryCryptType == CaseBusiness.Framework.ComplementaryCryptType.Voltage)
				//	r = CaseBusiness.Framework.Criptografia.VoltageCript.Context.Protect(valor);
			}
			catch (System.Exception ex)
			{
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
				throw ex;
			}

			return r;
		}

		public String CodificarNumeroCartao(String valor)
        {
			String r = "";

			try
			{
				r = Codificar(valor);
			}
			catch (System.Exception ex)
			{
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
				throw ex;
			}

            return r;
        }

		public String CodificarCodigoIdentificacaoCartao(String valor)
		{
			SHA256Managed hashstring = new SHA256Managed();
			String hashString = String.Empty;

			Byte[] bytes = Encoding.Unicode.GetBytes(valor);
			Byte[] hash = hashstring.ComputeHash(bytes);


			foreach (Byte x in hash)
				hashString += String.Format("{0:x2}", x);

			return hashString;
		}

		public String Decodificar(String valor)
		{
			String r = "";

			try
			{
				if (_complementaryCryptType == CaseBusiness.Framework.ComplementaryCryptType.TripleDES)
					r = tDES.Decodificar(valor);
				//else if (_complementaryCryptType == CaseBusiness.Framework.ComplementaryCryptType.Voltage)
				//	r = CaseBusiness.Framework.Criptografia.VoltageCript.Context.Access(valor);
			}
			catch (System.Exception ex)
			{
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
				throw ex;
			}


			return r;
		}

		public String DecodificarNumeroCartao(String valor)
        {
			String r = "";

			try
			{
				r = Decodificar(valor);
			}
			catch (System.Exception ex)
			{
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
				throw ex;
			}

			return r;
		}
    }
}
