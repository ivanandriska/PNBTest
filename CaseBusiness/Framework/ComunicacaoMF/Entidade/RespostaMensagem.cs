using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.ComunicacaoMF.Entidade
{
    public class RespostaMensagem 
    {
        private Int32 _codigo = 0;
        private String _mensagem = "";
        private TipoMensagem _tipoMensagem;

        public Int32 Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public String Mensagem
        {
            get { return _mensagem; }
            set { _mensagem = value; }
        }

        public TipoMensagem TipoMensagem
        {
            get { return _tipoMensagem; }
            set { _tipoMensagem = value; }
        }
    }
}
