using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.ComunicacaoMF.Entidade
{
    public abstract class Header
    {
        private String _programa = "";
        private Int32 _resposta = 0;
        private String _mensagem = "";
        private String _usuarioManutencao = "";
        private String _indicadorPagina = "";
        private String _paginaSeguinte = "";
        private String _paginaAnterior = "";
        private Int32 _registrosPorPagina = 0;
        private RespostaMensagem _respostaMensagem;
        private String _tipoManutencao = "";

        public String Programa
        {
            get { return _programa; }
            set { _programa = value; }
        }

        public Int32 Resposta
        {
            get { return _resposta; }
            set { _resposta = value; }
        }

        public String Mensagem
        {
            get { return _mensagem; }
            set { _mensagem = value; }
        }

        public String UsuarioManutencao
        {
            get { return _usuarioManutencao; }
            set { _usuarioManutencao = value; }
        }

        public String IndicadorPagina
        {
            get { return _indicadorPagina; }
            set { _indicadorPagina = value; }
        }

        public String PaginaSeguinte
        {
            get { return _paginaSeguinte; }
            set { _paginaSeguinte = value; }
        }

        public String PaginaAnterior
        {
            get { return _paginaAnterior; }
            set { _paginaAnterior = value; }
        }

        public Int32 RegistrosPorPagina
        {
            get { return _registrosPorPagina; }
            set { _registrosPorPagina = value; }
        }

        public RespostaMensagem RespostaMensagem
        {
            get { return _respostaMensagem; }
            set { _respostaMensagem = value; }
        }

        public String TipoManutencao
        {
            get { return _tipoManutencao; }
            set { _tipoManutencao = value; }
        }

    }
}