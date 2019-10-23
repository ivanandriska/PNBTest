using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout
{
    internal class Mensagem
    {
        private String _nome = "";
        private List<Acao> _acao = new List<Acao>();

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public List<Acao> Acao
        {
            get { return _acao; }
            set { _acao = value; }
        }
    }
}
