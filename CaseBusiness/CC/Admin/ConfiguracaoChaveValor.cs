#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
using System.Runtime.Caching;
#endregion Using

namespace CaseBusiness.CC.Admin
{
    public class ConfiguracaoChaveValor : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "ConfiguracaoChaveValor_";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 60; 
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 10; 
        #endregion MemoryCache

        #region Atributos
        private String _cdConfiguracaoChave = String.Empty;
        private String _dsConfiguracaoChave = String.Empty;
        private String _vlConfiguracao = String.Empty;

        public enum ConfiguracaoChave 
        { 
            TRACE_EXPURGO, 
            TRACE_GERACAO,
            NOME_JAR, 
            PASTA_JAR, 
            PASTA_JAVA,
            MENSAGERIA_EXPURGO,
            NOME_PY,
            PASTA_PY,
            PASTA_PYTHON,
            DIAS_CALC_CEP_PREVLC,
            ATU_CALC_CEP_PREVLC,
            DIG_CALC_CEP_PREVLC,
            ARMZ_GEREN_SRV_INDEX,
            ARMZ_DADOS_INDEX,
            ENDERECO_SERV_INDEX,
            AUTENTICA_SERV_INDEX,
            USUARIO_SERV_INDEX,
            SENHA_SERV_INDEX,
            NOME_INDEX_GEREN_SRV,
            NOME_INDEX_DADOS,
            NOME_TYPE_DADOS,
            ENDERECO_MQ,
            PORTA_MQ,
            AUTENTICA_MQ,
            USUARIO_MQ,
            SENHA_MQ,
            FILA_MQ,
            ATU_LB_DADOS_INMEMRY,
            CICLO_ENVIO_SMS,
            QTDE_ENVIO_SMS,
            URL_REQ_ENVIO_SMS,
            URL_PROC_ENVIO_SMS,
            KAFKA_SERVERS,
            SMSRetCntctEvntTopic,
            SMSSendDsptchrTopic,
            PROC_AGUAR_ENV_SMS,
            BASE_URL_TWW
        };

        private const string kCODIGOCHAVE_DBVALUE_TRACE_EXPURGO = "TRACE_EXPURGO";
        private const string kCODIGOCHAVE_DBVALUE_TRACE_GERACAO= "TRACE_GERACAO";
        private const string kCODIGOCHAVE_DBVALUE_NOME_JAR = "NOME_JAR";
        private const string kCODIGOCHAVE_DBVALUE_PASTA_JAR = "PASTA_JAR";
        private const string kCODIGOCHAVE_DBVALUE_PASTA_JAVA = "PASTA_JAVA";
        private const string kCODIGOCHAVE_DBVALUE_MENSAGERIA_EXPURGO = "MENSAGERIA_EXPURGO";
        private const string kCODIGOCHAVE_DBVALUE_NOME_PY = "NOME_PY";
        private const string kCODIGOCHAVE_DBVALUE_PASTA_PY = "PASTA_PY";
        private const string kCODIGOCHAVE_DBVALUE_PASTA_PYTHON = "PASTA_PYTHON";
        private const string kCODIGOCHAVE_DBVALUE_DIAS_CALC_CEP_PREVLC = "DIAS_CALC_CEP_PREVLC";
        private const string kCODIGOCHAVE_DBVALUE_ATU_CALC_CEP_PREVLC = "ATU_CALC_CEP_PREVLC";
        private const string kCODIGOCHAVE_DBVALUE_DIG_CALC_CEP_PREVLC = "DIG_CALC_CEP_PREVLC";

        private const string kCODIGOCHAVE_DBVALUE_ARMZ_GEREN_SRV_INDEX = "ARMZ_GEREN_SRV_INDEX";
        private const string kCODIGOCHAVE_DBVALUE_ARMZ_DADOS_INDEX = "ARMZ_DADOS_INDEX";
        private const string kCODIGOCHAVE_DBVALUE_ENDERECO_SERV_INDEX = "ENDERECO_SERV_INDEX";
        private const string kCODIGOCHAVE_DBVALUE_AUTENTICA_SERV_INDEX = "AUTENTICA_SERV_INDEX";
        private const string kCODIGOCHAVE_DBVALUE_USUARIO_SERV_INDEX = "USUARIO_SERV_INDEX";
        private const string kCODIGOCHAVE_DBVALUE_SENHA_SERV_INDEX = "SENHA_SERV_INDEX";
        private const string kCODIGOCHAVE_DBVALUE_NOME_INDEX_GEREN_SRV = "NOME_INDEX_GEREN_SRV";
        private const string kCODIGOCHAVE_DBVALUE_NOME_INDEX_DADOS = "NOME_INDEX_DADOS";
        private const string kCODIGOCHAVE_DBVALUE_NOME_TYPE_DADOS = "NOME_TYPE_DADOS";

        public const String kTRACE_GERACAO_ATIVADO_DBVALUE = "ATIV";
        public const String kTRACE_GERACAO_DESATIVADO_DBVALUE = "DESATIV";

        public const String kARMZ_GEREN_SERV_INDEX_ATIVADO_DBVALUE = "ATIV";
        public const String kARMZ_GEREN_SERV_INDEX_DESATIVADO_DBVALUE = "DESATIV";

        public const String kARMZ_DADOS_INDEX_ATIVADO_DBVALUE = "ATIV";
        public const String kARMZ_DADOS_INDEX_DESATIVADO_DBVALUE = "DESATIV";

        public const String kAUTENTICA_DADOS_INDEX_ATIVADO_DBVALUE = "ATIV";
        public const String kAUTENTICA_DADOS_INDEX_DESATIVADO_DBVALUE = "DESATIV";

        public const String kAUTENTICA_MQ_ATIVADO_DBVALUE = "ATIV";
        public const String kAUTENTICA_MQ_DESATIVADO_DBVALUE = "DESATIV";


        private const string kCODIGOCHAVE_DBVALUE_ENDERECO_MQ = "ENDERECO_MQ";
        private const string kCODIGOCHAVE_DBVALUE_PORTA_MQ = "PORTA_MQ";
        private const string kCODIGOCHAVE_DBVALUE_AUTENTICA_MQ = "AUTENTICA_MQ";
        private const string kCODIGOCHAVE_DBVALUE_USUARIO_MQ = "USUARIO_MQ";
        private const string kCODIGOCHAVE_DBVALUE_SENHA_MQ = "SENHA_MQ";
        private const string kCODIGOCHAVE_DBVALUE_FILA_MQ = "FILA_MQ";
        private const string kCODIGOCHAVE_DBVALUE_ATU_LB_DADOS_INMEMRY = "ATU_LB_DADOS_INMEMRY";

        private const string kCODIGOCHAVE_DBVALUE_CICLO_ENVIO_SMS = "CICLO_ENVIO_SMS";
        private const string kCODIGOCHAVE_DBVALUE_QTDE_ENVIO_SMS = "QTDE_ENVIO_SMS";

        private const string kCODIGOCHAVE_DBVALUE_URL_REQ_ENVIO_SMS = "URL_REQ_ENVIO_SMS";
        private const string kCODIGOCHAVE_DBVALUE_URL_PROC_ENVIO_SMS = "URL_PROC_ENVIO_SMS";

        private const string kCODIGOCHAVE_DBVALUE_KAFKA_SERVERS = "KAFKA_SERVERS";
        private const string kCODIGOCHAVE_DBVALUE_SMSRetCntctEvntTopic = "SMSRetCntctEvntTopic";
        private const string kCODIGOCHAVE_DBVALUE_SMSSendDsptchrTopic = "SMSSendDsptchrTopic";
        private const string kCODIGOCHAVE_DBVALUE_PROC_AGUAR_ENV_SMS = "PROC_AGUAR_ENV_SMS";
        private const string kCODIGOCHAVE_DBVALUE_BASE_URL_TWW = "BASE_URL_TWW";

        #endregion Atributos

        #region Propriedades
        public String CodigoConfiguracaoChave
        {
            get { return _cdConfiguracaoChave; }
            set { _cdConfiguracaoChave = value; }
        }

        public String DescricaoConfiguracaoChave
        {
            get { return _dsConfiguracaoChave; }
            set { _dsConfiguracaoChave = value; }
        }

        public String Valor
        {
            get { return _vlConfiguracao; }
            set { _vlConfiguracao = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe InterpretadorTraceConfiguracao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ConfiguracaoChaveValor(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe InterpretadorTraceConfiguracao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public ConfiguracaoChaveValor(Int32 idUsuarioManutencao,
                                      CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }


        /// <summary>
        /// Construtor classe InterpretadorTraceConfiguracao e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="enmConfiguracaoChave">Código de Configuração</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ConfiguracaoChaveValor(ConfiguracaoChave cdConfiguracaoChave,
                                      Int32 idUsuarioManutencao)
            //String cdInterpretadorTraceConfiguracao,  
            : this(idUsuarioManutencao)
        {
            Consultar(cdConfiguracaoChave);
        }


        /// <summary>
        /// Construtor classe InterpretadorTraceConfiguracao utilizando uma Transação e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="enmConfiguracaoChave">Código de Configuração</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public ConfiguracaoChaveValor(ConfiguracaoChave cdConfiguracaoChave,
                                      Int32 idUsuarioManutencao,
                                      CaseBusiness.Framework.BancoDados.Transacao transacao)
            : this(idUsuarioManutencao, transacao)
        {
            Consultar(cdConfiguracaoChave);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Altera o Valor de uma Chave de Configuração 
        /// </summary>
        /// <param name="enmConfiguracaoChave">Código de Configuração</param>
        /// <param name="valorConfiguracaoChave">Valor de uma Chave de Configuração</param>
        public void Alterar(ConfiguracaoChave cdConfiguracaoChave,
                            //String descricaoConfiguracaoChave,
                            String valorConfiguracaoChave)
        {
            try
            {
                acessoDadosBase.AddParameter("@CFGCHVL_CD", ObterConfiguracaoChave_DBValue(cdConfiguracaoChave));
                //acessoDadosBase.AddParameter("@CFGCHVL_DS", descricaoConfiguracaoChave);
                acessoDadosBase.AddParameter("@CFGCHVL_VL", valorConfiguracaoChave);
                acessoDadosBase.AddParameter("@USU_ID", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prCFGCHVL_UPD");

                RemoveCache(ObterConfiguracaoChave_DBValue(cdConfiguracaoChave));
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consultar o Valor de uma Chave de Configuração 
        /// </summary>
        /// <param name="enmConfiguracaoChave">Código de Configuração</param>
        private void Consultar(ConfiguracaoChave cdConfiguracaoChave)
        {
            try
            {
                DataTable dt;

                String _kCacheKey = kCacheKey + ObterConfiguracaoChave_DBValue(cdConfiguracaoChave);

                if (MemoryCache.Default.Contains(_kCacheKey))
                    dt = MemoryCache.Default[_kCacheKey] as DataTable;
                else
                {
                    acessoDadosBase.AddParameter("@CFGCHVL_CD", ObterConfiguracaoChave_DBValue(cdConfiguracaoChave));

                    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                        "prCFGCHVL_SEL_CONSULTAR").Tables[0];
                }
                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    CodigoConfiguracaoChave = Convert.ToString(dt.Rows[0]["CFGCHVL_CD"]);
                    DescricaoConfiguracaoChave = Convert.ToString(dt.Rows[0]["CFGCHVL_DS"]);
                    Valor = Convert.ToString(dt.Rows[0]["CFGCHVL_VL"]);

                    __blnIsLoaded = true;

                    MemoryCache.Default.Set(_kCacheKey, dt,
                    new CacheItemPolicy()
                    {
                        SlidingExpiration = new TimeSpan(DateTime.Now.AddMinutes(kCache_SLIDINGEXPIRATION_MINUTES).Ticks - DateTime.Now.Ticks)
                    });
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        public void RemoveCache(String cdConfiguracaoChave)
        {
            String _kCacheKey = kCacheKey + cdConfiguracaoChave;

            if (MemoryCache.Default.Contains(_kCacheKey))
                MemoryCache.Default.Remove(_kCacheKey);
        }

        private String ObterConfiguracaoChave_DBValue(ConfiguracaoChave enmConfiguracaoChave)
        {
            String cdConfiguracaoChave_DBValue = "{EMPTY}";

            switch (enmConfiguracaoChave)
            {
                case ConfiguracaoChave.TRACE_EXPURGO:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_TRACE_EXPURGO;
                    break;

                case ConfiguracaoChave.TRACE_GERACAO:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_TRACE_GERACAO;
                    break;

                case ConfiguracaoChave.NOME_JAR:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_NOME_JAR;
                    break;

                case ConfiguracaoChave.PASTA_JAR:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_PASTA_JAR;
                    break;

                case ConfiguracaoChave.PASTA_JAVA:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_PASTA_JAVA;
                    break;

                case ConfiguracaoChave.MENSAGERIA_EXPURGO:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_MENSAGERIA_EXPURGO;
                    break;

                case ConfiguracaoChave.NOME_PY:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_NOME_PY;
                    break;

                case ConfiguracaoChave.PASTA_PY:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_PASTA_PY;
                    break;

                case ConfiguracaoChave.PASTA_PYTHON:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_PASTA_PYTHON;
                    break;

                case ConfiguracaoChave.DIAS_CALC_CEP_PREVLC:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_DIAS_CALC_CEP_PREVLC;
                    break;
                case ConfiguracaoChave.ATU_CALC_CEP_PREVLC:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_ATU_CALC_CEP_PREVLC;
                    break;
                case ConfiguracaoChave.DIG_CALC_CEP_PREVLC:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_DIG_CALC_CEP_PREVLC;
                    break;

                case ConfiguracaoChave.ARMZ_GEREN_SRV_INDEX:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_ARMZ_GEREN_SRV_INDEX;
                    break;

                case ConfiguracaoChave.ARMZ_DADOS_INDEX:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_ARMZ_DADOS_INDEX;
                    break;

                case ConfiguracaoChave.ENDERECO_SERV_INDEX:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_ENDERECO_SERV_INDEX;
                    break;

                case ConfiguracaoChave.AUTENTICA_SERV_INDEX:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_AUTENTICA_SERV_INDEX;
                    break;

                case ConfiguracaoChave.USUARIO_SERV_INDEX:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_USUARIO_SERV_INDEX;
                    break;

                case ConfiguracaoChave.SENHA_SERV_INDEX:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_SENHA_SERV_INDEX;
                    break;

                case ConfiguracaoChave.NOME_INDEX_GEREN_SRV:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_NOME_INDEX_GEREN_SRV;
                    break;

                case ConfiguracaoChave.NOME_INDEX_DADOS:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_NOME_INDEX_DADOS;
                    break;

                case ConfiguracaoChave.NOME_TYPE_DADOS:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_NOME_TYPE_DADOS;
                    break;

                case ConfiguracaoChave.ENDERECO_MQ:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_ENDERECO_MQ;
                    break;

                case ConfiguracaoChave.PORTA_MQ:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_PORTA_MQ;
                    break;

                case ConfiguracaoChave.AUTENTICA_MQ:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_AUTENTICA_MQ;
                    break;
                case ConfiguracaoChave.USUARIO_MQ:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_USUARIO_MQ;
                    break;

                case ConfiguracaoChave.SENHA_MQ:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_SENHA_MQ;
                    break;

                case ConfiguracaoChave.FILA_MQ:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_FILA_MQ;
                    break;

                case ConfiguracaoChave.ATU_LB_DADOS_INMEMRY:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_ATU_LB_DADOS_INMEMRY;
                    break;

                case ConfiguracaoChave.CICLO_ENVIO_SMS:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_CICLO_ENVIO_SMS;
                    break;

                case ConfiguracaoChave.QTDE_ENVIO_SMS:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_QTDE_ENVIO_SMS;
                    break;

                case ConfiguracaoChave.URL_REQ_ENVIO_SMS:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_URL_REQ_ENVIO_SMS;
                    break;

                case ConfiguracaoChave.URL_PROC_ENVIO_SMS:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_URL_PROC_ENVIO_SMS;
                    break;

                case ConfiguracaoChave.KAFKA_SERVERS:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_KAFKA_SERVERS;
                    break;

                case ConfiguracaoChave.SMSRetCntctEvntTopic:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_SMSRetCntctEvntTopic;
                    break;

                case ConfiguracaoChave.SMSSendDsptchrTopic:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_SMSSendDsptchrTopic;
                    break;

                case ConfiguracaoChave.PROC_AGUAR_ENV_SMS:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_PROC_AGUAR_ENV_SMS;
                    break;

                case ConfiguracaoChave.BASE_URL_TWW:
                    cdConfiguracaoChave_DBValue = kCODIGOCHAVE_DBVALUE_BASE_URL_TWW;
                    break;
            }

            return cdConfiguracaoChave_DBValue;
        }

        /// Altera o Valor de uma Chave de Configuração 
        /// </summary>
        /// <param name="enmConfiguracaoChave">Código de Configuração</param>
        /// <param name="valorConfiguracaoChave">Valor de uma Chave de Configuração</param>
        public void Alterar(String cdConfiguracaoChave,
                            String descricaoConfiguracaoChave,
                            String valorConfiguracaoChave)
        {
            try
            {
                acessoDadosBase.AddParameter("@CFGCHVL_CD", cdConfiguracaoChave);
                acessoDadosBase.AddParameter("@CFGCHVL_DS", descricaoConfiguracaoChave);
                acessoDadosBase.AddParameter("@CFGCHVL_VL", valorConfiguracaoChave);
                acessoDadosBase.AddParameter("@USU_ID", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prCFGCHVL_UPD");

                RemoveCache(cdConfiguracaoChave);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consultar o Valor de uma Chave de Configuração 
        /// </summary>
        public DataTable Listar()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FilaLog está operando em Modo Entidade Only"); }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCFGCHVL_SEL_LISTAR").Tables[0];

                //Renomear Colunas
                RenomearColunas(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("CFGCHVL_CD")) { dt.Columns["CFGCHVL_CD"].ColumnName = "CodigoChaveValor"; }
            if (dt.Columns.Contains("CFGCHVL_DS")) { dt.Columns["CFGCHVL_DS"].ColumnName = "NomeChaveValor"; }
            if (dt.Columns.Contains("CFGCHVL_VL")) { dt.Columns["CFGCHVL_VL"].ColumnName = "ValorChaveValor"; }
            if (dt.Columns.Contains("CFGCHVL_TP_CAMPO")) { dt.Columns["CFGCHVL_TP_CAMPO"].ColumnName = "TipoCampoChaveValor"; }
            if (dt.Columns.Contains("SIST_ID")) { dt.Columns["SIST_ID"].ColumnName = "IdSistema"; }
            if (dt.Columns.Contains("SIST_NM")) { dt.Columns["SIST_NM"].ColumnName = "NomeSistema"; }
            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "IdUsuarioAlteracao"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "NomeUsuarioAlteracao"; }
            if (dt.Columns.Contains("CFGCHVL_DH_USUARIO_UPD")) { dt.Columns["CFGCHVL_DH_USUARIO_UPD"].ColumnName = "DataHoraUpdate"; }
        }
        #endregion Métodos
    }
}
