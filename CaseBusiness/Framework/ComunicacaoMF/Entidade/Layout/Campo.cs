using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout
{
    internal class Campo
    {
        private String _nome = "";
        private CaseBusiness.Framework.TipoDados _tipoDados;
        private Int32 _tamanho = 0;
        private String _formato = "";
        private String _propriedade = "";

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }
        
        public CaseBusiness.Framework.TipoDados TipoDados
        {
            get { return _tipoDados; }
            set { _tipoDados = value; }
        }
        
        public Int32 Tamanho
        {
            get { return _tamanho; }
            set { _tamanho = value; }
        }

        public String Formato
        {
            get { return _formato; }
            set { _formato = value; }
        }

        public String Propriedade
        {
            get { return _propriedade; }
            set { _propriedade = value; }
        }
    }
}
