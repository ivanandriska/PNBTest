using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;

namespace CaseBusiness.Framework.Configuracao
{
    [Serializable]
    public class Configuracao
    {
        internal static App _aplicacao = 0;
        internal static String _nomeAplicacao = "";
        internal static String _templateEmail = "";
        internal static List<String> _arquivosInicializacao = new List<String>();
        private static CaseBusiness.Framework.BancoDeDados _bancoPrincipal = CaseBusiness.Framework.BancoDeDados.Case;
        private static Dictionary<CaseBusiness.Framework.BancoDeDados, BancoDados.Entidade.Configuracao> _bancosDisponiveis = new Dictionary<CaseBusiness.Framework.BancoDeDados, BancoDados.Entidade.Configuracao>();
        internal static String _erroInicializacao = "";
        internal static TipoAplicacao _tipoAplicacao;
        internal static Boolean _apresentaTodosIndiacadores = true;
        private static String _versaoLayout = "";
        internal static String _chaveArmazenada = "2rpnet.";
        internal static Boolean _habilitaAcessoDadosPrivados = false;
        //private static String _IP = "";
        //private static Int32 _Port = 0;
        //private static Int32 _timeOut = 0;
        //private static String _caminhoLayout = "";
        //private static Int32 _registrosPorPagina = 0;
        internal static Autenticacao _autenticacao = Autenticacao.SQL;
        internal static Boolean _executaReprocessamento = false;
        internal static TipoComunicacao _tipoComunicacao = TipoComunicacao.Local;
        internal static CaseBusiness.Framework.BancoDeDados _bancoLog = BancoDeDados.Case;
        internal static Int32 _usuarioAplicacao = Int32.MinValue;
        private static SortedList<DateTime, String> _loading = new SortedList<DateTime, String>();
        internal static Framework.Cliente _cliente;
        internal static DateTime _ultimaLimpezaOraclePool = DateTime.Now;
        internal static ComplementaryCryptType _cryptType = ComplementaryCryptType.TripleDES; // TripleDES é a criptografia padrão complementar
		internal static String aChave_Atual = "";
		internal static String aNova_Chave = "";
		internal static Byte aSituacao = 0;
		internal static String aData_Validade = "";
		internal static String aData_Registro = "";
		internal static String aData_Expiracao_PCI = "";
		internal static Boolean pciCarregado = false;
        private static String _UrlZENVIA = "";

        public static CaseBusiness.Framework.BancoDeDados BancoLog
        {
            get { return Configuracao._bancoLog; }
            set { Configuracao._bancoLog = value; }
        }

        public static TipoComunicacao TipoComunicacao
        {
            get { return Configuracao._tipoComunicacao; }
        }

        public static Boolean ExecutaReprocessamento
        {
            get { return Configuracao._executaReprocessamento; }
            set { Configuracao._executaReprocessamento = value; }
        }

        public static String VersaoLayout
        {
            get { return Configuracao._versaoLayout; }
            set { Configuracao._versaoLayout = value; }
        }

        //public static Boolean ApresentaTodosIndiacadores
        //{
        //    get
        //    {
        //        new AcessoDados.Configuracao().BuscarConfiguracao(CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao);
        //        return Configuracao._apresentaTodosIndiacadores;
        //    }
        //}

        public static TipoAplicacao TipoAplicacao
        {
            get { return Configuracao._tipoAplicacao; }
        }

        public static App Aplicacao
        {
            get { return _aplicacao; }
        }

        public static String NomeAplicacao
        {
            get
            {
                if (String.IsNullOrEmpty(_nomeAplicacao))
                    _nomeAplicacao = _aplicacao.ToString();

                return _nomeAplicacao;
            }
        }

        public static String TemplateEmail
        {
            get { return _templateEmail; }
        }

        public static List<String> ArquivosInicializacao
        {
            get { return _arquivosInicializacao; }
            set { _arquivosInicializacao = value; }
        }

        public static String CaminhoAplicacao
        {
            get
            {
                if (CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao == TipoAplicacao.Web)
                    return AppDomain.CurrentDomain.BaseDirectory;
                else
                    return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static String CaminhoConfig
        {
            get
            {
                return CaminhoAplicacao + @"/App_Config";
            }
        }

        public static CaseBusiness.Framework.BancoDeDados BancoPrincipal
        {
            get { return Configuracao._bancoPrincipal; }
            set { Configuracao._bancoPrincipal = value; }
        }

        public static String ConnectionString(CaseBusiness.Framework.BancoDeDados banco)
        {
            if (_habilitaAcessoDadosPrivados)
            {
                try
                {
                    CaseBusiness.Framework.BancoDados.Entidade.Configuracao conf = null;
                    BancoDados.Processo.Configuracao confProcesso = new CaseBusiness.Framework.BancoDados.Processo.Configuracao();
                    conf = confProcesso.BuscarStringConexao(banco);

                    return conf.StringConexao;
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("ConnectionString: " + ex.Message + ex.InnerException, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                    throw;
                }
            }
            else
                throw new System.Exception("Informe a chave de acesso para ter acesso aos dados.");
        }

        public static String ErroInicializacao
        {
            get { return Configuracao._erroInicializacao; }
        }

        public static Autenticacao Autenticacao
        {
            get
            {
                return _autenticacao;
            }
        }

        public static Dictionary<CaseBusiness.Framework.BancoDeDados, BancoDados.Entidade.Configuracao> BancosDisponiveis
        {
            get
            {
                if (_habilitaAcessoDadosPrivados)
                    return Configuracao._bancosDisponiveis;
                else
                    throw new System.Exception("Informe a chave de acesso para ter acesso aos dados.");
            }
        }

        public static Int32 UsuarioAplicacao
        {
            get { return Configuracao._usuarioAplicacao; }
        }

        public static SortedList<DateTime, String> Loading
        {
            get { return Configuracao._loading; }
            set { Configuracao._loading = value; }
        }

        public static Framework.Cliente Cliente
        {
            get 
            {
                if (_cliente == 0)
                {
                    try
                    {
                        _cliente = (Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                    }
                    catch (System.Exception ex)
                    {
                        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Tag CodigoCliente não encontrado no arquivo .config - " + ex.ToString(), ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                        throw;
                    }
                    
                }

                return Configuracao._cliente; 
            }
        }

        public static ComplementaryCryptType ComplementaryCryptType
        {
            get
            {
                return _cryptType;
            }
            set
            {
                _cryptType = value;
            }
        }

        public static String UrlZENVIA
        {
            get
            {
                _UrlZENVIA = CaseBusiness.CB.AppSettings["UrlZENVIA"];
                return Configuracao._UrlZENVIA;
            }

            set
            {
                Configuracao._UrlZENVIA = value;
            }
        }


        #region Propriedades Internas

        //internal static String IP
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_IP))
        //            _IP = System.Configuration.ConfigurationSettings.AppSettings.Get("IP").ToString();

        //        return _IP;
        //    }
        //}

        //internal static Int32 Port
        //{
        //    get
        //    {
        //        if (_Port == 0)
        //            _Port = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("Port"));

        //        return _Port;
        //    }
        //}

        //internal static Int32 TimeOut
        //{
        //    get
        //    {
        //        if (_timeOut == 0)
        //            _timeOut = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("TimeOut"));

        //        return _timeOut;
        //    }
        //}

        //internal static String CaminhoLayout
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(_caminhoLayout))
        //            _caminhoLayout = _caminhoAplicacao + @"/" + System.Configuration.ConfigurationSettings.AppSettings.Get("CaminhoRelativoLayout");

        //        return _caminhoLayout;
        //    }
        //}



        #endregion Propriedades Internas

    }
}
