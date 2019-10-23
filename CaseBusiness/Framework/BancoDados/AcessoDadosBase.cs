using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CaseBusiness.Framework.BancoDados
{
    public class AcessoDadosBase
    {
        #region Atributos

        private DbConnection _connection = null;
        private DbTransaction _transaction = null;
        private DbParameter[] _parameters = new DbParameter[0];
        private Entidade.Configuracao conf = null;
        private Boolean _reutilizarConexao = false;
        private Boolean _habilitarModoExecucaoLongo = false; 

        #endregion Atributos

        #region Enum
        public enum SpecialType
        {
            None,
            CharacterLargeObject,
            NCharacterLargeObject
        }

        #endregion

        #region Propriedades
        public Boolean ReutilizarConexao
        {
            get { return _reutilizarConexao; }
            set { _reutilizarConexao = value; }
        }

        public DbParameter[] Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        ///  Altera a conexão da instância atual para Connection Timeout = 0 (Infinito). Utilizar o valor True apenas para execução de queries com alto tempo de execução e chamadas assíncronas
        ///  Valor default: False
        /// </summary>
        public bool HabilitarModoExecucaoLongo
        {
            get
            {
                return _habilitarModoExecucaoLongo;
            }

            set
            {
                _habilitarModoExecucaoLongo = value;
            }
        }
        #endregion

        #region Construtores

        public AcessoDadosBase()
        {
            try
            {
                CarregarStringConexao(CaseBusiness.Framework.Configuracao.Configuracao.BancoPrincipal);
            }
            catch (System.Exception ex)
            {
                CaseBusiness.Framework.Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }
        }

        public AcessoDadosBase(CaseBusiness.Framework.BancoDeDados bancoDados)
        {
            try
            {
                CarregarStringConexao(bancoDados);
            }
            catch (System.Exception ex)
            {
                Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }
        }

        public AcessoDadosBase(Transacao transacao)
        {
            try
            {
                if (transacao != null)
                {
                    _connection = transacao.Connection;
                    _transaction = transacao.Transaction;
                    conf = transacao.Conf;
                    _reutilizarConexao = true;
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }
        }

        #endregion Construtores

        #region Conexao
        private void CarregarStringConexao(CaseBusiness.Framework.BancoDeDados banco)
        {
            try
            {
                Processo.Configuracao confProcesso = new CaseBusiness.Framework.BancoDados.Processo.Configuracao();
                conf = confProcesso.BuscarStringConexao(banco);
            }
            catch (System.Exception ex)
            {
                CaseBusiness.Framework.Log.Log.LogarArquivo("Erro (CarregarStringConexao): " + ex.Message + ex.InnerException, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }
        }

        private void AbrirConexao()
        {
            try
            {
                if (HabilitarModoExecucaoLongo)
                {
                    String tempConnString = conf.StringConexao;
                    Int32 posIni = tempConnString.IndexOf(";Connect Timeout=");
                    Int32 posFim = tempConnString.IndexOf(";", posIni + 1);
                    tempConnString = tempConnString.Remove(posIni, posFim - posIni);
                    tempConnString = tempConnString + ";Connect Timeout=0";

                    if (conf.Sgdb == SGDB.SQLServer)
                        _connection = new SqlConnection(tempConnString);
                    else
                        _connection = new OracleConnection(tempConnString);
                }
                else
                {
                    if (conf.Sgdb == SGDB.SQLServer)
                        _connection = new SqlConnection(conf.StringConexao);
                    else
                        _connection = new OracleConnection(conf.StringConexao);
                }

                if (_connection != null)
                {
                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    //SOLUÇÃO ALTERNATIVA DO PROBLEMA DE TIMEOUT DO CONNECTION POOL CASO O CONNECTION LIFETIME NAO FECHAR AS CONEXOES 
                    //if (conf.Sgdb == SGDB.Oracle)
                    //{
                    //    if (Configuracao.Configuracao._ultimaLimpezaOraclePool.AddMinutes(10) < DateTime.Now)
                    //    {
                    //        OracleConnection.ClearPool((OracleConnection)_connection);
                    //        Configuracao.Configuracao._ultimaLimpezaOraclePool = DateTime.Now;
                    //    }
                    //}
                }
            }
            catch (System.Exception ex)
            {
                CaseBusiness.Framework.Log.Log.LogarArquivo("Erro (AbrirConexao): " + ex.Message + ex.InnerException, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }
        }

        #endregion Conexao

        #region Lê Parametros ao Command de saida
        /// <summary>
        /// This method is used to attach array of DbParameters to a DbCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of DbParameters to be added to command</param>
        private static List<object> LeParametrosSaida(DbCommand command, DbParameter[] commandParameters)
        {
            List<object> listaParametrosSaida = new List<object>();

            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                        {
                            if (p.Value == DBNull.Value)
                            {
                                listaParametrosSaida.Add("");
                            }
                            else
                            {
                                listaParametrosSaida.Add(command.Parameters[p.ParameterName].Value.ToString());
                            }
                        }
                    }
                }
            }
            return listaParametrosSaida;
        }

        private static Dictionary<String, Object> LeParametrosSaidaDicionario(DbCommand command, DbParameter[] commandParameters)
        {
            Dictionary<String, Object> dicionarioParametrosSaida = new Dictionary<String, Object>();

            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                        {
                            if (p.Value == DBNull.Value)
                            {
                                dicionarioParametrosSaida.Add(p.ParameterName, "");
                            }
                            else
                            {
                                dicionarioParametrosSaida.Add(p.ParameterName, command.Parameters[p.ParameterName].Value);
                            }
                        }
                    }
                }
            }
            return dicionarioParametrosSaida;
        }
        #endregion

        #region Adiciona Parametros ao Command
        /// <summary>
        /// This method is used to attach array of DbParameters to a DbCommand.
        /// 
        /// This method will assign a value of DbNull to any parameter with a direction of
        /// InputOutput and a value of null.  
        /// 
        /// This behavior will prevent default values from being used, but
        /// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        /// where the user provided no input value.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added</param>
        /// <param name="commandParameters">An array of DbParameters to be added to command</param>
        private static void AttachParameters(ref DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("Command is NULL");
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// Adiciona Parâmetro
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro a ser adicionado</param>
        /// <param name="value">Valor do parâmetro a ser adicionado</param>
        public void AddParameter(String parameterName, Object value)
        {
            AddParameters(parameterName, value, ParameterDirection.Input, SpecialType.None);
        }
        /// <summary>
        /// Adiciona Parâmetro
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro a ser adicionado</param>
        /// <param name="value">Valor do parâmetro a ser adicionado</param>
        /// <param name="specialType">Parametro do Tipo Especial (CLOB - Exclusivamente para conexões Oracle)</param>
        public void AddParameter(String parameterName, Object value, SpecialType specialType)
        {
            AddParameters(parameterName, value, ParameterDirection.Input, specialType);
        }

        /// <summary>
        /// Adiciona Parâmetro com direção de uso do mesmo (IN/OUT)
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro a ser adicionado</param>
        /// <param name="value">Valor do parâmetro a ser adicionado</param>
        /// <param name="direction">Direção do parâmetro a ser adicionado</param> 
        public void AddParameter(String parameterName, Object value, ParameterDirection direction)
        {
            AddParameters(parameterName, value, direction, SpecialType.None);
        }

        /// <summary>
        /// Adiciona Parâmetro com direção de uso do mesmo (IN/OUT)
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro a ser adicionado</param>
        /// <param name="value">Valor do parâmetro a ser adicionado</param>
        /// <param name="direction">Direção do parâmetro a ser adicionado</param> 
        /// <param name="specialType">Parametro do Tipo Especial (CLOB - Exclusivamente para conexões Oracle)</param>
        public void AddParameter(String parameterName, Object value, ParameterDirection direction, SpecialType specialType)
        {
            AddParameters(parameterName, value, direction, specialType);
        }

        /// <summary>
        /// Adiciona Parâmetro Opcional
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro a ser adicionado</param>
        /// <param name="value">Valor do parâmetro a ser adicionado</param>
        public void AddOptionalParameter(String parameterName, Object value)
        {
            AddOptionalParameters(parameterName, value, ParameterDirection.Input, SpecialType.None);
        }

        /// <summary>
        /// Adiciona Parâmetro Opcional com opção de tipo especial
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro a ser adicionado</param>
        /// <param name="value">Valor do parâmetro a ser adicionado</param>
        /// <param name="specialType">Parametro do Tipo Especial (CLOB - Exclusivamente para conexões Oracle)</param>
        public void AddOptionalParameter(String parameterName, Object value, SpecialType specialType)
        {
            AddOptionalParameters(parameterName, value, ParameterDirection.Input, specialType);
        }

        private void AddParameters(String parameterName, Object value, ParameterDirection direction, SpecialType specialType)
        {
            try
            {
               // Boolean typeDefined = false;

                if (conf.Sgdb == SGDB.SQLServer)
                {
                    if (_parameters == null)
                        _parameters = new SqlParameter[0];

                    Array.Resize(ref _parameters, _parameters.Length + 1);

                    if (value is Byte)
                    {
                        if (Convert.ToByte(value) == Byte.MinValue)
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, SqlDbType.TinyInt);
                        else
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToByte(value));

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Int16)
                    {
                        if (Convert.ToInt16(value) == Int16.MinValue)
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, SqlDbType.SmallInt);
                        else
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToInt16(value));

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Int32)
                    {
                        if (Convert.ToInt32(value) == Int32.MinValue)
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, SqlDbType.Int);
                        else
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToInt32(value));

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Int64)
                    {
                        if (Convert.ToInt64(value) == Int64.MinValue)
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, SqlDbType.BigInt);
                        else
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToInt64(value));

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Double)
                    {
                        if (Convert.ToDouble(value) == Double.MinValue)
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, SqlDbType.Float);
                        else
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToDouble(value));

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Decimal)
                    {
                        if (Convert.ToDecimal(value) == Decimal.MinValue)
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, SqlDbType.Decimal);
                        else
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToDecimal(value));

                        //_parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is String)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(value)))
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, SqlDbType.VarChar);
                        else
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToString(value));

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is DateTime)
                    {
                        if (Convert.ToDateTime(value) == DateTime.MinValue)
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, SqlDbType.DateTime);
                        else
                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToDateTime(value));

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Boolean)
                    {
                        _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToBoolean(value));
                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else
                    {
                        _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, DBNull.Value);
                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                }
                else
                {
                    if (_parameters == null)
                        _parameters = new OracleParameter[0];

                    if (direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput || direction == ParameterDirection.Output)
                    {
                        parameterName = parameterName.Replace("@", "");
                        parameterName = "v_" + parameterName;
                    }

                    Array.Resize(ref _parameters, _parameters.Length + 1);

                    if (specialType == SpecialType.CharacterLargeObject)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(value)))
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Clob, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Clob, value.ToString(), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;

                    }
                    else if (specialType == SpecialType.NCharacterLargeObject)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(value)))
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.NClob, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.NClob, value.ToString(), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;

                    }
                    else if (value is Byte)
                    {
                        if (Convert.ToByte(value) == Byte.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Byte, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Byte, Convert.ToByte(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Int16)
                    {
                        if (Convert.ToInt16(value) == Int16.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int16, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int16, Convert.ToInt16(value), direction);

                        //typeDefined = true;
                    }
                    else if (value is Int32)
                    {
                        if (Convert.ToInt32(value) == Int32.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int32, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int32, Convert.ToInt32(value), direction);

                        //typeDefined = true;
                    }
                    else if(value is Int64)
                    {
                        if (Convert.ToInt64(value) == Int64.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int64, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int64, Convert.ToInt64(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Double)
                    {
                        if (Convert.ToDouble(value) == Double.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Double, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Double, Convert.ToDouble(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Decimal)
                    {
                        if (Convert.ToDecimal(value) == Decimal.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Decimal, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Decimal, Convert.ToDecimal(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is String)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(value)))
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Varchar2, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Varchar2, Convert.ToString(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is DateTime)
                    {
                        if (Convert.ToDateTime(value) == DateTime.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.TimeStamp, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.TimeStamp, Convert.ToDateTime(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }
                    else if (value is Boolean)
                    {
                        _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int16, Convert.ToBoolean(value), direction);
                        //typeDefined = true;
                    }
                    else
                    {
                        _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, DBNull.Value);
                        _parameters[_parameters.Length - 1].Direction = direction;
                        //typeDefined = true;
                    }                    
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void AddOptionalParameters(String parameterName, Object value, ParameterDirection direction, SpecialType specialType)
        {
            try
            {
                if (conf.Sgdb == SGDB.SQLServer)
                {
                    if (_parameters == null)
                        _parameters = new SqlParameter[0];


                    if (value is Byte)
                    {
                        if (Convert.ToByte(value) != Byte.MinValue)
                        {
                            Array.Resize(ref _parameters, _parameters.Length + 1);

                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToByte(value));
                            _parameters[_parameters.Length - 1].Direction = direction;
                        }
                        
                    }
                    else if (value is Int16)
                    {
                        if (Convert.ToInt16(value) != Int16.MinValue)
                        {
                            Array.Resize(ref _parameters, _parameters.Length + 1);

                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToInt16(value));
                            _parameters[_parameters.Length - 1].Direction = direction;
                        }
                    }
                    else if (value is Int32)
                    {
                        if (Convert.ToInt32(value) != Int32.MinValue)
                        {
                            Array.Resize(ref _parameters, _parameters.Length + 1);

                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToInt32(value));
                            _parameters[_parameters.Length - 1].Direction = direction;
                        }
                    }
                    else if (value is Int64)
                    {
                        if (Convert.ToInt64(value) != Int64.MinValue)
                        {
                            Array.Resize(ref _parameters, _parameters.Length + 1);

                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToInt64(value));
                            _parameters[_parameters.Length - 1].Direction = direction;
                        }
                    }
                    else if (value is Double)
                    {
                        if (Convert.ToDouble(value) != Double.MinValue)
                        {
                            Array.Resize(ref _parameters, _parameters.Length + 1);

                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToDouble(value));
                            _parameters[_parameters.Length - 1].Direction = direction;
                        }
                    }
                    else if (value is Decimal)
                    {
                        if (Convert.ToDecimal(value) != Decimal.MinValue)
                        {
                            Array.Resize(ref _parameters, _parameters.Length + 1);

                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToDecimal(value));
                            _parameters[_parameters.Length - 1].Direction = direction;
                        }
                    }
                    else if (value is String)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(value)))
                        {
                            Array.Resize(ref _parameters, _parameters.Length + 1);

                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToString(value));
                            _parameters[_parameters.Length - 1].Direction = direction;
                        }
                    }
                    else if (value is DateTime)
                    {
                        if (Convert.ToDateTime(value) != DateTime.MinValue)
                        {
                            Array.Resize(ref _parameters, _parameters.Length + 1);

                            _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToDateTime(value));
                            _parameters[_parameters.Length - 1].Direction = direction;
                        }
                    }
                    else if (value is Boolean)
                    {
                        Array.Resize(ref _parameters, _parameters.Length + 1);

                        _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, Convert.ToBoolean(value));
                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                    else if (value is DBNull)
                    {
                        Array.Resize(ref _parameters, _parameters.Length + 1);

                        _parameters[_parameters.Length - 1] = new SqlParameter(parameterName, DBNull.Value);
                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                }
                else
                {
                    if (_parameters == null)
                        _parameters = new OracleParameter[0];

                    if (direction == ParameterDirection.Input || direction == ParameterDirection.InputOutput || direction == ParameterDirection.Output)
                    {
                        parameterName = parameterName.Replace("@", "");
                        parameterName = "v_" + parameterName;
                    }

                    Array.Resize(ref _parameters, _parameters.Length + 1);

                    if (specialType == SpecialType.CharacterLargeObject)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(value)))
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Clob, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Clob, value.ToString(), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;

                    }
                    else if (value is Byte)
                    {
                        if (Convert.ToByte(value) == Byte.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Byte, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Byte, Convert.ToByte(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                    else if (value is Int16)
                    {
                        if (Convert.ToInt16(value) == Int16.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int16, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int16, Convert.ToInt16(value), direction);
                    }
                    else if (value is Int32)
                    {
                        if (Convert.ToInt32(value) == Int32.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int32, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int32, Convert.ToInt32(value), direction);

                    }
                    else if (value is Int64)
                    {
                        if (Convert.ToInt64(value) == Int64.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int64, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int64, Convert.ToInt64(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                    else if (value is Double)
                    {
                        if (Convert.ToDouble(value) == Double.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Double, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Double, Convert.ToDouble(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                    else if (value is Decimal)
                    {
                        if (Convert.ToDecimal(value) == Decimal.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Decimal, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Decimal, Convert.ToDecimal(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                    else if (value is String)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(value)))
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Varchar2, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Varchar2, Convert.ToString(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                    else if (value is DateTime)
                    {
                        if (Convert.ToDateTime(value) == DateTime.MinValue)
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.TimeStamp, DBNull.Value, direction);
                        else
                            _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.TimeStamp, Convert.ToDateTime(value), direction);

                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                    else if (value is Boolean)
                    {
                        _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, OracleDbType.Int16, Convert.ToBoolean(value), direction);
                    }
                    else if (value is DBNull)
                    {
                        _parameters[_parameters.Length - 1] = new OracleParameter(parameterName, DBNull.Value);
                        _parameters[_parameters.Length - 1].Direction = direction;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Prepara Command para Execução
        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command
        /// </summary>
        /// <param name="command">The DbCommand to be prepared</param>
        /// <param name="connection">A valid DbConnection, on which to execute this command</param>
        /// <param name="transaction">A valid DbTransaction, or 'null'</param>
        /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">The stored procedure name or T-Db command</param>
        /// <param name="commandParameters">An array of DbParameters to be associated with the command or 'null' if no parameters are required</param>
        /// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
        private void PrepareCommand(ref DbCommand command, CommandType commandType, string commandText, DbParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("Command is NULL");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText is NULL or not specified");

            if (_reutilizarConexao)
            {
                if (_connection == null)
                    AbrirConexao();
                else
                {
                    if (_connection.State != ConnectionState.Open)
                        AbrirConexao();
                }
            }
            else
                AbrirConexao();

            //Define a conexão do comando
            command.Connection = _connection;

            //Verifica se existe transação aberta
            if (_transaction != null)
                command.Transaction = _transaction;

            // Set the command text (stored procedure name or Db statement)
            command.CommandText = commandText;

            // Set the command type
            command.CommandType = commandType;

            //Set timeout
            command.CommandTimeout = _connection.ConnectionTimeout;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
                AttachParameters(ref command, commandParameters);
        }
        #endregion

        #region ExecuteNonQuery

        public void ExecuteNonQuerySemRetorno(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;
            }

            try
            {
                PrepareCommand(ref cmd, cmdType, cmdText, commandParameters);
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (cmd.Transaction == null)
                {
                    if (!_reutilizarConexao)
                        FecharConexao();
                }
            }
        }

        public void ExecuteNonQuerySemRetorno(CommandType cmdType, string cmdText)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;
            }

            try
            {
                PrepareCommand(ref cmd, cmdType, cmdText, _parameters);
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (cmd.Transaction == null)
                {
                    if (!_reutilizarConexao)
                        FecharConexao();
                }
            }
        }

        public List<Object> ExecuteNonQueryComRetorno(CommandType cmdType, String cmdText, params DbParameter[] commandParameters)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;
            }

            try
            {
                PrepareCommand(ref cmd, cmdType, cmdText, commandParameters);
                cmd.ExecuteNonQuery();

                return LeParametrosSaida(cmd, commandParameters);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (cmd.Transaction == null)
                {
                    if (!_reutilizarConexao)
                        FecharConexao();
                }
            }
        }

        public Dictionary<String, Object> ExecuteNonQueryComRetornoDicionario(CommandType cmdType, String cmdText, params DbParameter[] commandParameters)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;
            }

            try
            {
                PrepareCommand(ref cmd, cmdType, cmdText, commandParameters);
                cmd.ExecuteNonQuery();

                return LeParametrosSaidaDicionario(cmd, commandParameters);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (cmd.Transaction == null)
                {
                    if (!_reutilizarConexao)
                        FecharConexao();
                }
            }
        }

        public List<Object> ExecuteNonQueryComRetorno(CommandType cmdType, String cmdText)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;
            }

            try
            {
                PrepareCommand(ref cmd, cmdType, cmdText, _parameters);
                cmd.ExecuteNonQuery();

                return LeParametrosSaida(cmd, _parameters);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (cmd.Transaction == null)
                {
                    if (!_reutilizarConexao)
                        FecharConexao();
                }
            }
        }

        public Dictionary<String, Object> ExecuteNonQueryComRetornoDicionario(CommandType cmdType, String cmdText)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;
            }

            try
            {
                PrepareCommand(ref cmd, cmdType, cmdText, _parameters);
                cmd.ExecuteNonQuery();

                return LeParametrosSaidaDicionario(cmd, _parameters);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (cmd.Transaction == null)
                {
                    if (!_reutilizarConexao)
                        FecharConexao();
                }
            }
        }


        /// <summary>
        /// Método de teste de Async (NÃO UTILIZAR)
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        //public async Task ExecuteNonQuerySemRetornoAsync(CommandType cmdType, string cmdText)
        //{
        //    DbCommand cmd = null;

        //    if (conf.Sgdb == SGDB.SQLServer)
        //        cmd = new SqlCommand();
        //    else
        //    {
        //        OracleCommand cmdOra = new OracleCommand();
        //        cmdOra.BindByName = true;
        //        cmd = cmdOra;
        //    }

        //    try
        //    {
        //        PrepareCommand(ref cmd, cmdType, cmdText, _parameters);
        //        await cmd.ExecuteNonQueryAsync();
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (conf.Sgdb == SGDB.SQLServer)
        //            _parameters = new SqlParameter[0];
        //        else
        //            _parameters = new OracleParameter[0];

        //        if (cmd.Transaction == null)
        //        {
        //            if (!_reutilizarConexao)
        //                FecharConexao();
        //        }
        //    }
        //}

        #endregion

        #region ExecuteQueryReader
        /// <summary>
        /// Esse método deve ser usado para chamar procedures que retornem dados em mais de uma linha através de um DbDataReader.
        /// </summary>
        /// <returns>
        /// Retorna um DbDataReader contendo os dados devolvidos pela procedure.
        /// </returns>
        public DbDataReader ExecuteQueryReader(CommandType commandType, string spName, params DbParameter[] parametros)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;

                Array.Resize(ref _parameters, _parameters.Length + 1);
                _parameters[_parameters.Length - 1] = new OracleParameter("cv_1", OracleDbType.RefCursor, ParameterDirection.Output);
            }

            try
            {
                PrepareCommand(ref cmd, commandType, spName, parametros);
                return cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];
            }
        }

        public DbDataReader ExecuteQueryReader(CommandType commandType, string spName)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;

                Array.Resize(ref _parameters, _parameters.Length + 1);
                _parameters[_parameters.Length - 1] = new OracleParameter("cv_1", OracleDbType.RefCursor, ParameterDirection.Output);
            }

            try
            {
                PrepareCommand(ref cmd, commandType, spName, _parameters);
                return cmd.ExecuteReader();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];
            }
        }
        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet(CommandType commandType, string spName, params DbParameter[] parametros)
        {
            DbCommand cmd = null;
            DbDataAdapter daAdapter;
            DataSet dstRetorno;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;

                Array.Resize(ref _parameters, _parameters.Length + 1);
                _parameters[_parameters.Length - 1] = new OracleParameter("cv_1", OracleDbType.RefCursor, ParameterDirection.Output);
            }

            try
            {
                PrepareCommand(ref cmd, commandType, spName, parametros);

                dstRetorno = new DataSet();

                if (conf.Sgdb == SGDB.SQLServer)
                {
                    daAdapter = new SqlDataAdapter();
                    daAdapter.SelectCommand = cmd;
                }
                else
                {
                    daAdapter = new OracleDataAdapter();
                    daAdapter.SelectCommand = cmd;
                }

                daAdapter.Fill(dstRetorno);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (!_reutilizarConexao)
                    FecharConexao();
            }

            return dstRetorno;
        }

        public DataSet ExecuteDataSet(CommandType commandType, string spName)
        {
            DbCommand cmd = null;
            DbDataAdapter daAdapter;
            DataSet dstRetorno;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;

                Array.Resize(ref _parameters, _parameters.Length + 1);
                _parameters[_parameters.Length - 1] = new OracleParameter("cv_1", OracleDbType.RefCursor, ParameterDirection.Output);
            }

            try
            {
                PrepareCommand(ref cmd, commandType, spName, _parameters);

                dstRetorno = new DataSet();

                if (conf.Sgdb == SGDB.SQLServer)
                {
                    daAdapter = new SqlDataAdapter();
                    daAdapter.SelectCommand = cmd;
                }
                else
                {
                    daAdapter = new OracleDataAdapter();
                    daAdapter.SelectCommand = cmd;
                }

                daAdapter.Fill(dstRetorno);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (!_reutilizarConexao)
                    FecharConexao();
            }

            return dstRetorno;
        }
        #endregion

        #region Execute Scalar

        protected void ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;
            }

            try
            {
                PrepareCommand(ref cmd, cmdType, cmdText, commandParameters);
                cmd.ExecuteScalar();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (!_reutilizarConexao)
                    FecharConexao();
            }
        }

        protected void ExecuteScalar(CommandType cmdType, string cmdText)
        {
            DbCommand cmd = null;

            if (conf.Sgdb == SGDB.SQLServer)
                cmd = new SqlCommand();
            else
            {
                OracleCommand cmdOra = new OracleCommand();
                cmdOra.BindByName = true;
                cmd = cmdOra;
            }

            try
            {
                PrepareCommand(ref cmd, cmdType, cmdText, _parameters);
                cmd.ExecuteScalar();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conf.Sgdb == SGDB.SQLServer)
                    _parameters = new SqlParameter[0];
                else
                    _parameters = new OracleParameter[0];

                if (!_reutilizarConexao)
                    FecharConexao();
            }
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
        #endregion

        #region FecharConexao
        public void FecharConexao()
        {
            if (_connection != null)
            {
                if (conf.Sgdb == SGDB.SQLServer)
                {
                    ((SqlConnection)_connection).Close();
                    ((SqlConnection)_connection).Dispose();
                }
                else
                {
                    ((OracleConnection)_connection).Close();
                    ((OracleConnection)_connection).Dispose();
                }
            }
        }

		public void LimparPoolConexoes()
		{
			if (conf.Sgdb == SGDB.Oracle)
			{
				if (_connection == null)
					AbrirConexao();

				OracleConnection.ClearPool((OracleConnection)_connection);

				FecharConexao();
			}
		}
		#endregion
	}
}
