using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.ComunicacaoMF.Entidade.Layout
{
    internal class Acao
    {
        private String _nome = "";
        private String _programa = "";
        private List<Campo> _campos = new List<Campo>();
        private Int32 _registrosPorPagina = 0;
        private Tipo _tipo;

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public String Programa
        {
            get { return _programa; }
            set { _programa = value; }
        }

        public List<Campo> Campos
        {
            get { return _campos; }
            set { _campos = value; }
        }

        public Int32 RegistrosPorPagina
        {
            get { return _registrosPorPagina; }
            set { _registrosPorPagina = value; }
        }

        public Tipo Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
    }
}
