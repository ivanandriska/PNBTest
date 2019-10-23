#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Runtime.Caching;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Mensageria
{
    public class ConfiguracaoLog : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_ConfiguracaoLog_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idConfiguracaoLog = Int32.MinValue; 
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private Int32 _idCanal = Int32.MinValue;

        // Clinte
        private DateTime _horaEnvioInicioCliente = DateTime.MinValue;
        private DateTime _horaEnvioFimCliente = DateTime.MinValue;
        private Int32 _quantidadeTentativaEnvioCliente = Int32.MinValue;
        private Int32 _tempoMaximoEnviarMensagemCliente = Int32.MinValue;

        // Monitoria
        private DateTime _horaEnvioInicioMonitoria = DateTime.MinValue;
        private DateTime _horaEnvioFimMonitoria = DateTime.MinValue;
        private Int32 _quantidadeMaximaMensagenEnviadaTransacaoMonitoria = Int32.MinValue;
        private Int32 _quantidadeMaximaMensagemEnviadaMonitoria = Int32.MinValue;
        private Int32 _quantidadeIntervaloMensagemMonitoria = Int32.MinValue;
        private Int32 _quantidadeTentativaEnvioMonitoria = Int32.MinValue;
        private Int32 _tempoMaximoEnviarMensagemMonitoria = Int32.MinValue;

        private Int32 _idUsuario = Int32.MinValue;
        private DateTime _dataUsuarioInclusao = DateTime.MinValue;
        private String _operacao = String.Empty;
        #endregion Atributos

        #region Propriedades
        public Int32 IdConfiguracaoLog
        {
            get { return _idConfiguracaoLog; }
            set { _idConfiguracaoLog = value; }
        }

        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public Int32 IdCanal
        {
            get { return _idCanal; }
            set { _idCanal = value; }
        }

        public DateTime HoraEnvioInicioCliente
        {
            get { return _horaEnvioInicioCliente; }
            set { _horaEnvioInicioCliente = value; }
        }

        public DateTime HoraEnvioFimCliente
        {
            get { return _horaEnvioFimCliente; }
            set { _horaEnvioFimCliente = value; }
        }

        public Int32 QuantidadeTentativaEnvioCliente
        {
            get { return _quantidadeTentativaEnvioCliente; }
            set { _quantidadeTentativaEnvioCliente = value; }
        }

        public Int32 TempoMaximoEnviarMensagemCliente
        {
            get { return _tempoMaximoEnviarMensagemCliente; }
            set { _tempoMaximoEnviarMensagemCliente = value; }
        }

        public DateTime HoraEnvioInicioMonitoria
        {
            get { return _horaEnvioInicioMonitoria; }
            set { _horaEnvioInicioMonitoria = value; }
        }

        public DateTime HoraEnvioFimMonitoria
        {
            get { return _horaEnvioFimMonitoria; }
            set { _horaEnvioFimMonitoria = value; }
        }

        public Int32 QuantidadeMaximaMensagenEnviadaTransacaoMonitoria
        {
            get { return _quantidadeMaximaMensagenEnviadaTransacaoMonitoria; }
            set { _quantidadeMaximaMensagenEnviadaTransacaoMonitoria = value; }
        }

        public Int32 QuantidadeMaximaMensagemEnviadaMonitoria
        {
            get { return _quantidadeMaximaMensagemEnviadaMonitoria; }
            set { _quantidadeMaximaMensagemEnviadaMonitoria = value; }
        }

        public Int32 QuantidadeIntervaloMensagemMonitoria
        {
            get { return _quantidadeIntervaloMensagemMonitoria; }
            set { _quantidadeIntervaloMensagemMonitoria = value; }
        }

        public Int32 QuantidadeTentativaEnvioMonitoria
        {
            get { return _quantidadeTentativaEnvioMonitoria; }
            set { _quantidadeTentativaEnvioMonitoria = value; }
        }

        public Int32 TempoMaximoEnviarMensagemMonitoria
        {
            get { return _tempoMaximoEnviarMensagemMonitoria; }
            set { _tempoMaximoEnviarMensagemMonitoria = value; }
        }

        public Int32 IdUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }

        public DateTime DataUsuarioInclusao
        {
            get { return _dataUsuarioInclusao; }
            set { _dataUsuarioInclusao = value; }
        }

        public String Operacao
        {
            get { return _operacao; }
            set { _operacao = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe ConfiguracaoLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public ConfiguracaoLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Configuracao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe ConfiguracaoLog
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public ConfiguracaoLog()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe ConfiguracaoLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ConfiguracaoLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe ConfiguracaoLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public ConfiguracaoLog(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Conexao e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idConfiguracaoLog">ID do Log da Configuração</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ConfiguracaoLog(Int32 idConfiguracaoLog, Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idConfiguracaoLog);
        }
        #endregion Construtores

        #region Métodos
          /// <summary>
        /// Remove do Cache as Organizações
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }

        /// <summary>
        /// Inclui um Log de Configuração de Envio
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id do Canaç</param>
        /// <param name="horaEnvioInicioCliente">Hora Envio Início Cliente</param>
        /// <param name="horaEnvioFimCliente">Hora Envio Fim Cliente</param>
        /// <param name="quantidadeTentativaEnvioCliente">Quantidade de Tentativa de Envio Cliente</param>
        /// <param name="tempoMaximoEnviarMensagemCliente">Tempo Máximo de Enviar a Mensagem Cliente</param>
        /// <param name="tempoMaximoRequisitarMensagemCliente">Tempo Máximo de Requisitar a Mensagem Cliente</param>
        /// <param name="horaEnvioInicioMonitoria">Hora Envio Início Monitoria</param>
        /// <param name="horaEnvioFimMonitoria">Hora Envio Fim Monitoria</param>
        /// <param name="quantidadeMaximaMensagenEnviadaTransacaoMonitoria">Quantidade Máxima Mensagen Enviada por Transação Monitoria</param>
        /// <param name="quantidadeMaximaMensagemEnviadaMonitoria">Quantidade Máxima Mensagem Enviada Monitoria</param>
        /// <param name="quantidadeIntervaloMensagemMonitoria">Quantidade Intervalo Mensagem Monitoria</param>
        /// <param name="quantidadeTentativaEnvioMonitoria">Quantidade de Tentativa de Envio Monitoria</param>
        /// <param name="dataHoraInclusao">Data/Hora Inclusão do Log</param>
        /// <param name="operacao">Operação</param>        /// 
        public void IncluirConfiguracaoLog(Int32 codigoOrganizacao,
                                        Int32 idCanal,
                                        DateTime horaEnvioInicioCliente,
                                        DateTime horaEnvioFimCliente,
                                        Int32 quantidadeTentativaEnvioCliente, 
                                        Int32 tempoMaximoEnviarMensagemCliente, 
                                        Int32 tempoMaximoRequisitarMensagemCliente,
                                        DateTime horaEnvioInicioMonitoria,
                                        DateTime horaEnvioFimMonitoria,
                                        Int32 quantidadeMaximaMensagenEnviadaTransacaoMonitoria,
                                        Int32 quantidadeMaximaMensagemEnviadaMonitoria,
                                        Int32 quantidadeIntervaloMensagemMonitoria,
                                        Int32 quantidadeTentativaEnvioMonitoria, 
                                        DateTime dataHoraInclusao,
                                        String operacao)
        {
            Incluir(codigoOrganizacao,
                    idCanal,
                    horaEnvioInicioCliente,
                    horaEnvioFimCliente,
                    quantidadeTentativaEnvioCliente, 
                    tempoMaximoEnviarMensagemCliente,
                    tempoMaximoRequisitarMensagemCliente,
                    horaEnvioInicioMonitoria,
                    horaEnvioFimMonitoria,
                    quantidadeMaximaMensagenEnviadaTransacaoMonitoria,
                    quantidadeMaximaMensagemEnviadaMonitoria,
                    quantidadeIntervaloMensagemMonitoria,
                    quantidadeTentativaEnvioMonitoria, 
                    dataHoraInclusao, 
                    operacao);
        }



        /// <summary>
        /// Inclui um Log de Configuração de Envio
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="horaEnvioInicioCliente">Hora Envio Início Cliente</param>
        /// <param name="horaEnvioFimCliente">Hora Envio Fim Cliente</param>
        /// <param name="quantidadeTentativaEnvioCliente">Quantidade de Tentativa de Envio Cliente</param>
        /// <param name="tempoMaximoEnviarMensagemCliente">Tempo Máximo de Enviar a Mensagem Cliente</param>
        /// <param name="tempoMaximoRequisitarMensagemCliente">Tempo Máximo de Requisitar a Mensagem Cliente</param>
        /// <param name="horaEnvioInicioMonitoria">Hora Envio Início Monitoria</param>
        /// <param name="horaEnvioFimMonitoria">Hora Envio Fim Monitoria</param>
        /// <param name="quantidadeMaximaMensagenEnviadaTransacaoMonitoria">Quantidade Máxima Mensagen Enviada por Transação Monitoria</param>
        /// <param name="quantidadeMaximaMensagemEnviadaMonitoria">Quantidade Máxima Mensagem Enviada Monitoria</param>
        /// <param name="quantidadeIntervaloMensagemMonitoria">Quantidade Intervalo Mensagem Monitoria</param>
        /// <param name="quantidadeTentativaEnvioMonitoria">Quantidade de Tentativa de Envio Monitoria</param>
        /// <param name="dataHoraInclusao">Data/Hora Inclusão do Log</param>
        /// <param name="operacao">Operação</param>
        private void Incluir(Int32 codigoOrganizacao, 
                            Int32 idCanal, 
                            DateTime horaEnvioInicioCliente, 
                            DateTime horaEnvioFimCliente, 
                            Int32 quantidadeTentativaEnvioCliente, 
                            Int32 tempoMaximoEnviarMensagemCliente, 
                            Int32 tempoMaximoRequisitarMensagemCliente, 
                            DateTime horaEnvioInicioMonitoria, 
                            DateTime horaEnvioFimMonitoria, 
                            Int32 quantidadeMaximaMensagenEnviadaTransacaoMonitoria, 
                            Int32 quantidadeMaximaMensagemEnviadaMonitoria, 
                            Int32 quantidadeIntervaloMensagemMonitoria, 
                            Int32 quantidadeTentativaEnvioMonitoria, 
                            DateTime dataHoraInclusao, 
                            String operacao)

        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Conexão está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCFGLOG_CLIEN_DH_ENVIO_INI", horaEnvioInicioCliente);
                acessoDadosBase.AddParameter("@MGCFGLOG_CLIEN_DH_ENVIO_FIM", horaEnvioFimCliente);
                acessoDadosBase.AddParameter("@MGCFGLOG_CLIEN_QT_TENT_ENV", quantidadeTentativaEnvioCliente);
                acessoDadosBase.AddParameter("@MGCFGLOG_CLIEN_QT_TE_MAX_ENV", tempoMaximoEnviarMensagemCliente);
                acessoDadosBase.AddParameter("@MGCFGLOG_CLIEN_QT_TE_MAX_REQ", tempoMaximoRequisitarMensagemCliente);
                acessoDadosBase.AddParameter("@MGCFGLOG_MONIT_DH_ENVIO_INI", horaEnvioInicioMonitoria);
                acessoDadosBase.AddParameter("@MGCFGLOG_MONIT_DH_ENVIO_FIM", horaEnvioFimMonitoria);
                acessoDadosBase.AddParameter("@MGCFGLOG_MONIT_QT_MAX_MG_TRS", quantidadeMaximaMensagenEnviadaTransacaoMonitoria);
                acessoDadosBase.AddParameter("@MGCFGLOG_MONIT_QT_MAX_MSG", quantidadeMaximaMensagemEnviadaMonitoria);
                acessoDadosBase.AddParameter("@MGCFGLOG_MONIT_QT_INTER_MSG", quantidadeIntervaloMensagemMonitoria);
                acessoDadosBase.AddParameter("@MGCFGLOG_MONIT_QT_TENT_ENV", quantidadeTentativaEnvioMonitoria);
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGCFGLOG_DH_USU_INS", dataHoraInclusao);
                acessoDadosBase.AddParameter("@MGCFGLOG_OPERACAO", operacao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMGCFGLOG_INS");

                // MemoryCache Clean
                RemoverCache();
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar Log de Configuração de Envio
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id do Canal</param>
        public DataTable Buscar(Int32 codigoOrganizacao, Int32 idCanal)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGCFGLOG_SEL_BUSCAR").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// Consultar uma Conexao por ID
        /// </summary>
        /// <param name="idConfiguracaoLog">ID do Log da Configuração</param>
        private void Consultar(Int32 idConfiguracaoLog)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Conexao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGCFGLOG_ID", idConfiguracaoLog);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGCFGLOG_SEL_CONSULTARID").Tables[0];

                // Fill Object
                PreencherAtributos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                IdConfiguracaoLog = Convert.ToInt32(dt.Rows[0]["MGCFGLOG_ID"]);
                CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                IdCanal = Convert.ToInt32(dt.Rows[0]["COMCNL_ID"]);

                // Cliente
                HoraEnvioInicioCliente = Convert.ToDateTime(dt.Rows[0]["MGCFGLOG_CLIEN_DH_ENVIO_INI"]);
                HoraEnvioFimCliente = Convert.ToDateTime(dt.Rows[0]["MGCFGLOG_CLIEN_DH_ENVIO_FIM"]);
                QuantidadeTentativaEnvioCliente = Convert.ToInt32(dt.Rows[0]["MGCFGLOG_CLIEN_QT_TENT_ENV"]);
                TempoMaximoEnviarMensagemCliente = Convert.ToInt32(dt.Rows[0]["MGCFGLOG_CLIEN_QT_TE_MAX_ENV"]);

                // Monitoria
                HoraEnvioInicioMonitoria = Convert.ToDateTime(dt.Rows[0]["MGCFGLOG_MONIT_DH_ENVIO_INI"]);
                HoraEnvioFimMonitoria = Convert.ToDateTime(dt.Rows[0]["MGCFGLOG_MONIT_DH_ENVIO_FIM"]);
                QuantidadeMaximaMensagenEnviadaTransacaoMonitoria = Convert.ToInt32(dt.Rows[0]["MGCFGLOG_MONIT_QT_MAX_MG_TRS"]);
                QuantidadeMaximaMensagemEnviadaMonitoria = Convert.ToInt32(dt.Rows[0]["MGCFGLOG_MONIT_QT_MAX_MSG"]);
                QuantidadeIntervaloMensagemMonitoria = Convert.ToInt32(dt.Rows[0]["MGCFGLOG_MONIT_QT_INTER_MSG"]);
                QuantidadeTentativaEnvioMonitoria = Convert.ToInt32(dt.Rows[0]["MGCFGLOG_MONIT_QT_TENT_ENV"]);
                TempoMaximoEnviarMensagemMonitoria = Convert.ToInt32(dt.Rows[0]["MGCFGLOG_MONIT_QT_TE_MAX_ENV"]);

                IdUsuario = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                DataUsuarioInclusao = Convert.ToDateTime(dt.Rows[0]["MGCFGLOG_DH_USU_INS"]);
                Operacao = dt.Rows[0]["MGCFGLOG_OPERACAO"].ToString();

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "NomeOrganizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "IdCanal"; }
            if (dt.Columns.Contains("COMCNL_NM")) { dt.Columns["COMCNL_NM"].ColumnName = "NomeCanal"; }
            if (dt.Columns.Contains("MGCFGLOG_ID")) { dt.Columns["MGCFGLOG_ID"].ColumnName = "IdConfiguracaoLog"; }

            //Cliente
            if (dt.Columns.Contains("MGCFGLOG_DH_ENVIO_INI")) { dt.Columns["MGCFGLOG_DH_ENVIO_INI"].ColumnName = "HoraEnvioInicioCliente"; }
            if (dt.Columns.Contains("MGCFGLOG_DH_ENVIO_FIM")) { dt.Columns["MGCFGLOG_DH_ENVIO_FIM"].ColumnName = "HoraEnvioFimCliente"; }
            if (dt.Columns.Contains("DH_ENVIO_CLIENTE")) { dt.Columns["DH_ENVIO_CLIENTE"].ColumnName = "HoraEnvioCliente"; }
            if (dt.Columns.Contains("MGCFGLOG_CLIEN_QT_TENT_ENV")) { dt.Columns["MGCFGLOG_CLIEN_QT_TENT_ENV"].ColumnName = "QuantidadeTentativaEnvioCliente"; }
            if (dt.Columns.Contains("MGCFGLOG_CLIEN_QT_TE_MAX_ENV")) { dt.Columns["MGCFGLOG_CLIEN_QT_TE_MAX_ENV"].ColumnName = "TempoMaximoEnviarMensagemCliente"; }
            if (dt.Columns.Contains("MGCFGLOG_CLIEN_QT_TE_MAX_REQ")) { dt.Columns["MGCFGLOG_CLIEN_QT_TE_MAX_REQ"].ColumnName = "TempoMaximoRequisitarMensagemCliente"; }

            // Monitoria
            if (dt.Columns.Contains("MGCFGLOG_MONIT_DH_ENVIO_INI")) { dt.Columns["MGCFGLOG_MONIT_DH_ENVIO_INI"].ColumnName = "HoraEnvioInicioMonitoria"; }
            if (dt.Columns.Contains("MGCFGLOG_MONIT_DH_ENVIO_FIM")) { dt.Columns["MGCFGLOG_MONIT_DH_ENVIO_FIM"].ColumnName = "HoraEnvioFimMonitoria"; }
            if (dt.Columns.Contains("DH_ENVIO_MONITORIA")) { dt.Columns["DH_ENVIO_MONITORIA"].ColumnName = "HoraEnvioMonitoria"; }
            if (dt.Columns.Contains("MGCFGLOG_MONIT_QT_MAX_MG_TRS")) { dt.Columns["MGCFGLOG_MONIT_QT_MAX_MG_TRS"].ColumnName = "QuantidadeMaximaMensagenEnviadaTransacaoMonitoria"; }
            if (dt.Columns.Contains("MGCFGLOG_MONIT_QT_MAX_MSG")) { dt.Columns["MGCFGLOG_MONIT_QT_MAX_MSG"].ColumnName = "QuantidadeMaximaMensagemEnviadaMonitoria"; }
            if (dt.Columns.Contains("MGCFGLOG_MONIT_QT_INTER_MSG")) { dt.Columns["MGCFGLOG_MONIT_QT_INTER_MSG"].ColumnName = "QuantidadeIntervaloMensagemMonitoria"; }
            if (dt.Columns.Contains("MGCFGLOG_MONIT_QT_TENT_ENV")) { dt.Columns["MGCFGLOG_MONIT_QT_TENT_ENV"].ColumnName = "QuantidadeTentativaEnvioMonitoria"; }
            if (dt.Columns.Contains("MGCFGLOG_MONIT_QT_TE_MAX_ENV")) { dt.Columns["MGCFGLOG_MONIT_QT_TE_MAX_ENV"].ColumnName = "TempoMaximoEnviarMensagemMonitoria"; }

            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("MGCFGLOG_DH_USU_INS")) { dt.Columns["MGCFGLOG_DH_USU_INS"].ColumnName = "DataHoraManutencao"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioManutencao"; }
            if (dt.Columns.Contains("MGCFGLOG_OPERACAO")) { dt.Columns["MGCFGLOG_OPERACAO"].ColumnName = "Operacao"; }
        }
        #endregion Métodos
    }
}
