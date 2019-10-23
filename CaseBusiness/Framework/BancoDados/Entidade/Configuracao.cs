using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.BancoDados.Entidade
{
    public class Configuracao
    {
        private String _ambiente = "";
        private SGDB _sgdb = SGDB.SQLServer;
        private String _servidor = "";
        private String _porta = "";
        private String _BancoDeDados = "";
        private String _TrustedConnection = "";
        private String _usuario = "";
        private String _senha = "";
        private String _dominio = "";
        private String _grupoUsuario = "";
        private String _GravarLogEvento = "";
        private String _versaoSQL;
        private Int32 _timeout = 120;
		private Int32 _connectionLifetime = 10;
		private Int32 _decrPoolSize = 1;
		private Int32 _IncrPoolSize = 5;
		private String _HAEvents = "N";
		private Int32 _minPoolSize = 1;
        private Int32 _maxPoolSize = 100;
        private String _stringConexao = "";
        private Boolean _loadingBalance = false;
        private String _servico = "";

        public String Servico
        {
            get { return _servico; }
            set { _servico = value; }
        }

        public String Ambiente
        {
            get { return _ambiente; }
            set { _ambiente = value; }
        }

        public Boolean LoadingBalance
        {
            get { return _loadingBalance; }
            set { _loadingBalance = value; }
        }


        public String StringConexao
        {
            get { return _stringConexao; }
            set { _stringConexao = value; }
        }

        public SGDB Sgdb
        {
            get { return _sgdb; }
            set { _sgdb = value; }
        }

        public Int32 MinPoolSize
        {
            get { return _minPoolSize; }
            set { _minPoolSize = value; }
        }

        public Int32 MaxPoolSize
        {
            get { return _maxPoolSize; }
            set { _maxPoolSize = value; }
        }

        public Int32 Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public String Servidor
        {
            get { return _servidor; }
            set { _servidor = value; }
        }

        public String Porta
        {
            get { return _porta; }
            set { _porta = value; }
        }

        public String BancoDeDados
        {
            get { return _BancoDeDados; }
            set { _BancoDeDados = value; }
        }

        public String TrustedConnection
        {
            get { return _TrustedConnection; }
            set { _TrustedConnection = value; }
        }

        public String Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        public String Senha
        {
            get { return _senha; }
            set { _senha = value; }
        }
        
        public String Dominio
        {
            get { return _dominio; }
            set { _dominio = value; }
        }

        public String GrupoUsuario
        {
            get { return _grupoUsuario; }
            set { _grupoUsuario = value; }
        }

        public String GravarLogEvento
        {
            get { return _GravarLogEvento; }
            set { _GravarLogEvento = value; }
        }

        public String VersaoSQL
        {
            get { return _versaoSQL; }
            set { _versaoSQL = value; }
        }

		public int ConnectionLifetime
		{
			get
			{
				return _connectionLifetime;
			}

			set
			{
				_connectionLifetime = value;
			}
		}

		public int DecrPoolSize
		{
			get
			{
				return _decrPoolSize;
			}

			set
			{
				_decrPoolSize = value;
			}
		}

		public int IncrPoolSize
		{
			get
			{
				return _IncrPoolSize;
			}

			set
			{
				_IncrPoolSize = value;
			}
		}

		public String HAEvents
		{
			get
			{
				return _HAEvents;
			}

			set
			{
				_HAEvents = value;
			}
		}
	}
}
