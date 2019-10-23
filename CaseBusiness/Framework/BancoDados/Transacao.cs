using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace CaseBusiness.Framework.BancoDados
{
	public class Transacao
	{
		#region Atributos

		private DbConnection _connection = null;
		private DbTransaction _transaction = null;
        private Entidade.Configuracao _conf = null;

		#endregion Atributos

		#region Construtores

		public Transacao() 
		{
			try
			{
				CarregarStringConexao(CaseBusiness.Framework.Configuracao.Configuracao.BancoPrincipal);
                AbrirConexao();
				_transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
			}
			catch (System.Exception ex)
			{
				Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
				throw;
			}
		}

		public Transacao(IsolationLevel isolationLevel)
		{
			try
			{
				CarregarStringConexao(CaseBusiness.Framework.Configuracao.Configuracao.BancoPrincipal);
                AbrirConexao();
				_transaction = _connection.BeginTransaction(isolationLevel);
			}
			catch (System.Exception ex)
			{
				Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
				throw;
			}
		}

		public Transacao(CaseBusiness.Framework.BancoDeDados bancoDados)
		{
			try
			{
				CarregarStringConexao(bancoDados);
                AbrirConexao();
				_transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
			}
			catch (System.Exception ex)
			{
				Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
				throw;
			}
		}

		public Transacao(CaseBusiness.Framework.BancoDeDados bancoDados, IsolationLevel isolationLevel)
		{
			try
			{
				CarregarStringConexao(bancoDados);
                AbrirConexao();
				_transaction = _connection.BeginTransaction(isolationLevel);
			}
			catch (System.Exception ex)
			{
				Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
				throw;
			}
		}

		#endregion Construtores

        private void AbrirConexao()
        {
            if (_conf.Sgdb == SGDB.SQLServer)
                _connection = new SqlConnection(_conf.StringConexao);
            else
                _connection = new OracleConnection(_conf.StringConexao);

            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
            }
        }

        private void CarregarStringConexao(CaseBusiness.Framework.BancoDeDados banco)
		{
			try
			{
				Processo.Configuracao confProcesso = new CaseBusiness.Framework.BancoDados.Processo.Configuracao();
				_conf = confProcesso.BuscarStringConexao(banco);
			}
			catch (System.Exception ex)
			{
				CaseBusiness.Framework.Log.Log.LogarArquivo("Erro (MontarConexao): " + ex.Message + ex.InnerException, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
				throw;
			}
		}

		protected void EndTransaction()
		{
			if (_transaction != null) _transaction.Dispose();
			_transaction = null;
		}

		protected void CloseConnection()
		{			
			if (_connection != null && _connection.State != ConnectionState.Closed)
			{
				_connection.Close();
				_connection.Dispose();
			}
		}

		#region Transação
		
		public void CommitTrans() {
			try
			{
				if (_transaction != null)
					_transaction.Commit();
			}
			catch(System.Exception)
			{
				throw;
			}
			finally
			{
				EndTransaction();
				CloseConnection();
			}		
		}

		public void RollbackTrans() {
			try
			{
				if (_transaction != null)
					_transaction.Rollback();
			}
			catch(System.Exception)
			{
				throw;
			}
			finally
			{
				EndTransaction();
				CloseConnection();
			}
		}
		#endregion

		#region Propriedades

		public DbConnection Connection
		{
			get
			{
				return _connection;
			}
		}

		public DbTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}


        internal Entidade.Configuracao Conf
        {
            get { return _conf; }
        }


		#endregion

		#region IDisposable
		protected void Dispose()
		{
			if (_connection != null)
			{
				_connection.Close();
				_connection.Dispose();
			}
		}
		#endregion
	}
}
