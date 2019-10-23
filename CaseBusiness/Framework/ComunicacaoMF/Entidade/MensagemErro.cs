using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.ComunicacaoMF.Entidade
{
    public class MensagemErro
    {
        private Int32 _codigo = 0;
        private String _mensagemEnvio = "";
        private List<String> _mensagemRetorno = null;
        private String _erro = "";
        private DateTime _DataHoraErro;
        private String _stackTrace = "";
        private String _usuario = "";

        public Int32 Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public String MensagemEnvio
        {
            get { return _mensagemEnvio; }
            set { _mensagemEnvio = value; }
        }

        public List<String> MensagemRetorno
        {
            get { return _mensagemRetorno; }
            set { _mensagemRetorno = value; }
        }

        public String Erro
        {
            get { return _erro; }
            set { _erro = value; }
        }

        public DateTime DataHoraErro
        {
            get { return _DataHoraErro; }
            set { _DataHoraErro = value; }
        }

        public String StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        public String Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }
    }
}