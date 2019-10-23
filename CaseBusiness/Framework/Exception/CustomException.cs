//using System;

//namespace CaseBusiness.Framework.Exception
//{
//    public class CustomException : System.Exception  //ApplicationException
//    {
//        #region Atributos
//        private Nivel _nivel;
//        private TipoLog _tipoLog;
//        private String _innerException;
//        private String _mensagem;
//        private String _stackTrace;
//        private String _source;
//        private Int32 _codigoErro;
//        private Int32 _idLog = 0;

//        #endregion Atributos

//        #region Propriedades

//        public TipoLog TipoLog
//        {
//            get { return _tipoLog; }
//            set { _tipoLog = value; }
//        }

//        public Int32 CodigoErro
//        {
//            get { return _codigoErro; }
//        }

//        public String StackTrace
//        {
//          get { return _stackTrace; }
//        }

//        public String Source
//        {
//            get { return _source; }
//        }

//        public Nivel Nivel
//        {
//            get { return _nivel; }
//        }        

//        public String InnerException
//        {
//            get { return _innerException; }
//        }

//        public String Mensagem
//        {
//            get { return _mensagem; }
//        }

//        public Int32 IdLog
//        {
//            get { return _idLog; }
//        }

//        #endregion Propriedades

//        #region Construtores

//        public CustomException(Nivel nivel, System.Exception exception)
//        {
//            this._nivel = nivel;            
//            this._innerException = exception.Message;
//            this._stackTrace = exception.StackTrace;
//            this._source = exception.Source;

//            if (exception is System.Data.OleDb.OleDbException)
//                _codigoErro = ((System.Data.OleDb.OleDbException)exception).ErrorCode;

//            try
//            {
//                _idLog = Log.Log.Logar((TipoLog)nivel, exception.Message, exception.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, 0, 0);
//            }
//            catch (System.Exception)
//            {
//            }
//        }

//        public CustomException(Nivel nivel, System.Exception exception, Int32 cdLog)
//        {
//            this._nivel = nivel;
//            this._innerException = exception.Message;
//            this._stackTrace = exception.StackTrace;
//            this._source = exception.Source;

//            if (exception is System.Data.OleDb.OleDbException)
//                _codigoErro = ((System.Data.OleDb.OleDbException)exception).ErrorCode;

//            try
//            {
//                _idLog = Log.Log.Logar((TipoLog)nivel, exception.Message, exception.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, 0, cdLog);
//            }
//            catch (System.Exception)
//            {
//            }
//        }

//        public CustomException(Nivel nivel, System.Exception exception, CaseBusiness.Framework.Tela tela)
//        {
//            this._nivel = nivel;
//            this._innerException = exception.Message;
//            this._stackTrace = exception.StackTrace;
//            this._source = exception.Source;

//            if (exception is System.Data.OleDb.OleDbException)
//                _codigoErro = ((System.Data.OleDb.OleDbException)exception).ErrorCode;

//            try
//            {
//                _idLog = Log.Log.Logar((TipoLog)nivel, exception.Message, exception.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, tela, 0);
//            }
//            catch (System.Exception)
//            {
//            }
//        }

//        public CustomException(Nivel nivel, String mensagem)
//        {
//            this._nivel = nivel;
//            this._mensagem = mensagem;

//            try
//            {
//                _idLog = Log.Log.Logar((TipoLog)nivel, mensagem, "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, 0, 0);
//            }
//            catch (System.Exception)
//            {
//            }
//        }

//        public CustomException(TipoLog tipoLog, System.Exception exception, Boolean gravarLog)
//        {
//            this._tipoLog = tipoLog;
//            this._innerException = exception.Message;
//            this._stackTrace = exception.StackTrace;
//            this._source = exception.Source;

//            if (exception is System.Data.OleDb.OleDbException)
//                _codigoErro = ((System.Data.OleDb.OleDbException)exception).ErrorCode;

//            try
//            {
//                if (gravarLog)
//                    _idLog = Log.Log.Logar(tipoLog, exception.Message, exception.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, 0, 0);
//            }
//            catch (System.Exception)
//            {
//            }
//        }

//        public CustomException(TipoLog tipoLog, System.Exception exception, Int32 cdLog)
//        {
//            this._tipoLog = tipoLog;
//            this._innerException = exception.Message;
//            this._stackTrace = exception.StackTrace;
//            this._source = exception.Source;

//            if (exception is System.Data.OleDb.OleDbException)
//                _codigoErro = ((System.Data.OleDb.OleDbException)exception).ErrorCode;

//            try
//            {
//                _idLog = Log.Log.Logar(tipoLog, exception.Message, exception.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, 0, cdLog);
//            }
//            catch (System.Exception)
//            {
//            }
//        }

//        public CustomException(TipoLog tipoLog, System.Exception exception, CaseBusiness.Framework.Tela tela)
//        {
//            this._tipoLog = tipoLog;
//            this._innerException = exception.Message;
//            this._stackTrace = exception.StackTrace;
//            this._source = exception.Source;

//            if (exception is System.Data.OleDb.OleDbException)
//                _codigoErro = ((System.Data.OleDb.OleDbException)exception).ErrorCode;

//            try
//            {
//                _idLog = Log.Log.Logar(tipoLog, exception.Message, exception.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, tela, 0);
//            }
//            catch (System.Exception)
//            {
//            }
//        }

//        public CustomException(TipoLog tipoLog, String mensagem, Boolean gravarLog)
//        {
//            this._tipoLog = tipoLog;
//            this._mensagem = mensagem;

//            try
//            {
//                if (gravarLog)
//                    _idLog = Log.Log.Logar(tipoLog, mensagem, "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, 0, 0);
//            }
//            catch (System.Exception)
//            {
//            }
//        }

//        #endregion Construtores
//    }
//}
