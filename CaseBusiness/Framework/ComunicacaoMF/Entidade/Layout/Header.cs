using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout
{
    internal class Header
    {
        private List<Campo> _campos = new List<Campo>();

        public List<Campo> Campos
        {
            get { return _campos; }
            set { _campos = value; }
        }
    }
}
