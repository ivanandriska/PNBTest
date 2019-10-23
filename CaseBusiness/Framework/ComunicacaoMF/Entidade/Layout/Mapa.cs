using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout
{
    internal static class Mapa
    {
        private static CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Header _header = new CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Header();
        private static List<CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Mensagem> _mensagens = new List<CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Mensagem>();

        public static CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Header Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public static List<CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout.Mensagem> Mensagens
        {
            get { return _mensagens; }
            set { _mensagens = value; }
        }
    }
}
