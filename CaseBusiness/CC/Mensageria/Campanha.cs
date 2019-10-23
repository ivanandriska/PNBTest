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
    public class Campanha : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "Campanha_";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 120;  // 60 minutos * 24 horas
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 30;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idCampanhas = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private String _codigoCampanha = String.Empty;
        private String _nomeCampanha = String.Empty;
        private String _objetivoCampanha = String.Empty;
        private DateTime _dataCampanhaInicio = DateTime.MinValue;
        private DateTime _dataCampanhaTermino = DateTime.MinValue;
        private String _tipoCampanha = String.Empty;
        //private String _periodicidadeExec = String.Empty;
        private String _statusCampanha = String.Empty;
        private Int32 _diasVigenciaCliente = Int32.MinValue;
        private Int32 _idUsuarioIns = Int32.MinValue;
        private DateTime _dataUsuarioIns = DateTime.MinValue;
        private Int32 _idUsuarioUpd = Int32.MinValue;
        private String _nomeUsuarioUpd = String.Empty;
        private String _nomeUsuarioIns = String.Empty;
        private DateTime _dataUsuarioUpd = DateTime.MinValue;
        private Int32 _qtdDiasRegraDescQqCampanha = Int32.MinValue;
        private String _queryCampanha = String.Empty;
        private String _queryMensuracao = String.Empty;
        private String _observacaoCampanha = String.Empty;
        private String _configuracaoDisparo = String.Empty;
        private Boolean _flagRegraDescQqCampanha = false;
        private Boolean _flagRegraDescMsmCampanha = false;
        private Int32 _qtdDiasRegraMsmCampanha = Int32.MinValue;
        private Boolean _flagRegraDescTipoCampanha = false;
        private String _listaRegraDescTipoCampanha = String.Empty;
        private DateTime _dataProximaExec = DateTime.MinValue;

        private DataTable _dtRestricoesExclusao;

        private DataTable _dtRestricoesAlteracao = null;
        #endregion Atributos

        #region Propriedades
        public Int32 IdCampanhas
        {
            get { return _idCampanhas; }
            set { _idCampanhas = value; }
        }

        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public String CodigoCampanha
        {
            get { return _codigoCampanha; }
            set { _codigoCampanha = value; }
        }

        public String NomeCampanha
        {
            get { return _nomeCampanha; }
            set { _nomeCampanha = value; }
        }

        public String ObjetivoCampanha
        {
            get { return _objetivoCampanha; }
            set { _objetivoCampanha = value; }
        }

        public DateTime DataCampanhaInicio
        {
            get { return _dataCampanhaInicio; }
            set { _dataCampanhaInicio = value; }
        }

        public DateTime DataCampanhaTermino
        {
            get { return _dataCampanhaTermino; }
            set { _dataCampanhaTermino = value; }
        }

        public String TipoCampanha
        {
            get { return _tipoCampanha; }
            set { _tipoCampanha = value; }
        }

        //public String PeriodicidadeExec
        //{
        //    get { return _periodicidadeExec; }
        //    set { _periodicidadeExec = value; }
        //}
        public String StatusCampanha
        {
            get { return _statusCampanha; }
            set { _statusCampanha = value; }
        }
        public Int32 DiasVigenciaCliente
        {
            get { return _diasVigenciaCliente; }
            set { _diasVigenciaCliente = value; }
        }
        public Int32 IdUsuarioIns
        {
            get { return _idUsuarioIns; }
            set { _idUsuarioIns = value; }
        }
        public String NomeUsuarioIns
        {
            get { return _nomeUsuarioIns; }
            set { _nomeUsuarioIns = value; }
        }
        public DateTime DataUsuarioIns
        {
            get { return _dataUsuarioIns; }
            set { _dataUsuarioIns = value; }
        }
        public Int32 IdUsuarioUpd
        {
            get { return _idUsuarioUpd; }
            set { _idUsuarioUpd = value; }
        }
        public String NomeUsuarioUpd
        {
            get { return _nomeUsuarioUpd; }
            set { _nomeUsuarioUpd = value; }
        }
        public DateTime DataUsuarioUpd
        {
            get { return _dataUsuarioUpd; }
            set { _dataUsuarioUpd = value; }
        }
        public Int32 QtdDiasRegraDescQqCampanha
        {
            get { return _qtdDiasRegraDescQqCampanha; }
            set { _qtdDiasRegraDescQqCampanha = value; }
        }
        public String QueryCampanha
        {
            get { return _queryCampanha; }
            set { _queryCampanha = value; }
        }
        public String QueryMensuracao
        {
            get { return _queryMensuracao; }
            set { _queryMensuracao = value; }
        }
        public String ObservacaoCampanha
        {
            get { return _observacaoCampanha; }
            set { _observacaoCampanha = value; }
        }
        public String ConfiguracaoDisparo
        {
            get { return _configuracaoDisparo; }
            set { _configuracaoDisparo = value; }
        }
        public Boolean FlagRegraDescQqCampanha
        {
            get { return _flagRegraDescQqCampanha; }
            set { _flagRegraDescQqCampanha = value; }
        }
        public Boolean FlagRegraDescMsmCampanha
        {
            get { return _flagRegraDescMsmCampanha; }
            set { _flagRegraDescMsmCampanha = value; }
        }
        public Int32 QtdDiasRegraMsmCampanha
        {
            get { return _qtdDiasRegraMsmCampanha; }
            set { _qtdDiasRegraMsmCampanha = value; }
        }
        public Boolean FlagRegraDescTipoCampanha
        {
            get { return _flagRegraDescTipoCampanha; }
            set { _flagRegraDescTipoCampanha = value; }
        }
        public String ListaRegraDescTipoCampanha
        {
            get { return _listaRegraDescTipoCampanha; }
            set { _listaRegraDescTipoCampanha = value; }
        }
        public DateTime DataProximaExec
        {
            get { return _dataProximaExec; }
            set { _dataProximaExec = value; }
        }

        public Boolean ExclusaoPermitida
        {
            get
            {
                if (!IsLoaded)
                {
                    _dtRestricoesExclusao = null;
                    return false;
                }

                if (RestricoesExclusao == null)
                { return false; }
                else
                {
                    if (_dtRestricoesExclusao.Rows.Count <= 0)
                    { return true; }
                    else
                    { return false; }
                }
            }
        }

        public Boolean AlteracaoPermitida
        {
            get
            {
                if (!IsLoaded)
                {
                    _dtRestricoesAlteracao = null;
                    return false;
                }

                if (RestricaoAlteracao == null)
                {
                    return false;
                }
                else
                {
                    if (_dtRestricoesAlteracao.Rows.Count <= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public DataTable RestricoesExclusao
        {
            get
            {
                if (_dtRestricoesExclusao == null)
                {
                    ObterRestricoesExclusao();
                }

                return _dtRestricoesExclusao;
            }
        }

        public DataTable RestricaoAlteracao
        {
            get
            {
                if (_dtRestricoesAlteracao == null)
                {
                    _dtRestricoesAlteracao = new DataTable();
                    _dtRestricoesAlteracao = ObterRestricaoAlteracao();
                }

                return _dtRestricoesAlteracao;
            }
        }

        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Campanhas - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Campanha(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Campanhas com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Campanhas
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Campanha(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Campanhas utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Campanha(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Campanhas e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idCampanhas">ID Restrição</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Campanha(Int32 idCampanhas, Int32 idUsuarioManutencao)
            :   this(idUsuarioManutencao)
        {
            Consultar(idCampanhas);
        }
        #endregion Construtores

        #region Métodos

        // <summary>
        /// Listar Usuarios de Envelopes de Análise
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public DataTable UsuarioListar(Int32 codigoOrganizacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe EnvelopeAnalise está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMP_SEL_USU_LISTAR").Tables[0];

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

        /// <summary>
        /// Inclui Campanha
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="codigoCampanha">Codigo Campanha</param>
        /// <param name="nomeCampanha">Nome Campanha</param>
        /// <param name="objetivoCampanha">Objetivo Campanha</param>
        /// <param name="dataCampanhaInicio">Data/Hora Inicial Campanha</param>
        /// <param name="dataCampanhaTermino">Data/Hora Termino Campanha</param>
        /// <param name="tipoCampanha">Tipo Campanha</param>
        /// <param name="statusCampanha">Status Campanha</param>
        /// <param name="dataHoraInclusao">Data/hora inclusão Campanha</param>
        /// <param name="configuracaoDisparo">Comfiguaração Disparo Campanha</param>
        /// <param name="flagRegraDescQqCampanha">Flag Regra Descanso Qualquer Campanha</param>
        /// <param name="flagRegraDescMsmCampanha">Flag Regra Descanso Mesma Campanha</param>
        /// <param name="qtdDiasRegraMsmCampanha">Dias Regra Descanso Mesma Campanha</param>
        /// <param name="flagRegraDescTipoCampanha">Flag Regra Descanso Tipo Campanha</param>
        /// <param name="listaRegraDescTipoCampanha">Lista Regra Descanso Tipo Campanha</param>
        /// <param name="qtdDiasRegraDescQqCampanha">Dias Regra Descanso Qualquer Campanha</param>        
        /// <returns>Id Campanha    </returns>
        public Int32 Incluir(Int32 codigoOrganizacao,
                                String codigoCampanha,
                                String nomeCampanha,
                                String objetivoCampanha,
                                DateTime dataCampanhaInicio,
                                DateTime dataCampanhaTermino,
                                String tipoCampanha,
                                String statusCampanha,
                                Int32 diasVigenciaCliente,
                                DateTime dataHoraInclusao,
                                String observacaoCampanha,
                                String configuracaoDisparo,
                                String flagRegraDescQqCampanha,
                                String flagRegraDescMsmCampanha,
                                Int32 qtdDiasRegraMsmCampanha,
                                String flagRegraDescTipoCampanha,
                                String listaRegraDescTipoCampanha,
                                Int32 qtdDiasRegraDescQqCampanha,
                                ref Int32 idCampanha)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Campanhas está operando em Modo Entidade Only"); }

                idCampanha = Int32.MinValue;

                acessoDadosBase.AddParameter("@CMP_CD", codigoCampanha);
                acessoDadosBase.AddParameter("@CMP_NOME", nomeCampanha);
                acessoDadosBase.AddParameter("@CMP_OBJETIVO", objetivoCampanha);
                acessoDadosBase.AddParameter("@CMD_PERIODO_INI", dataCampanhaInicio);
                acessoDadosBase.AddParameter("@CMD_PERIODO_FIM", dataCampanhaTermino);
                acessoDadosBase.AddParameter("@CMP_TIPO", tipoCampanha);
                acessoDadosBase.AddParameter("@CMP_ST", statusCampanha);
                acessoDadosBase.AddParameter("@CMP_QT_DIAS_VIGENCIA_CLIENTE", diasVigenciaCliente);
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@CMP_DH_USUARIO_INS", dataHoraInclusao);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@CMP_OBSERVACAO", observacaoCampanha);
                acessoDadosBase.AddParameter("@CMP_CONF_DISPARO", configuracaoDisparo);
                acessoDadosBase.AddParameter("@CMP_FL_RGR_DES_QQ_CMP", flagRegraDescQqCampanha);
                acessoDadosBase.AddParameter("@CMP_FL_RGR_DES_MSM_CMP", flagRegraDescMsmCampanha);
                acessoDadosBase.AddParameter("@CMP_RGR_DES_MSM_CMP_QT_DIAS", qtdDiasRegraMsmCampanha);
                acessoDadosBase.AddParameter("@CMP_FL_RGR_DES_CMP_TP", flagRegraDescTipoCampanha);
                acessoDadosBase.AddParameter("@CMP_RGR_DES_CMP_TP_LISTA", listaRegraDescTipoCampanha);
                acessoDadosBase.AddParameter("@CMP_RGR_DES_QQ_CMP_QT_DIAS", qtdDiasRegraDescQqCampanha);
                acessoDadosBase.AddParameter("@CMP_ID", Int32.MaxValue, ParameterDirection.InputOutput);

                idCampanha = Convert.ToInt32(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prCMP_INS")[0]);

                return _idCampanhas;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }       

        /// <summary>
        /// Obtem as Restrições de Exclusão da Campanha carregada
        /// </summary>
        private void ObterRestricoesExclusao()
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe QualificadorDefinicao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", IdCampanhas);

                _dtRestricoesExclusao = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                                       "prCMP_SEL_RESTRIC_DEL").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref _dtRestricoesExclusao);

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Inclui Campanha
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="codigoCampanha">Codigo Campanha</param>
        /// <param name="nomeCampanha">Nome Campanha</param>
        /// <param name="objetivoCampanha">Objetivo Campanha</param>
        /// <param name="dataCampanhaInicio">Data/Hora Inicial Campanha</param>
        /// <param name="dataCampanhaTermino">Data/Hora Termino Campanha</param>
        /// <param name="tipoCampanha">Tipo Campanha</param>
        /// <param name="statusCampanha">Status Campanha</param>
        /// <param name="dataHoraInclusao">Data/hora inclusão Campanha</param>
        /// <param name="configuracaoDisparo">Comfiguaração Disparo Campanha</param>
        /// <param name="flagRegraDescQqCampanha">Flag Regra Descanso Qualquer Campanha</param>
        /// <param name="flagRegraDescMsmCampanha">Flag Regra Descanso Mesma Campanha</param>
        /// <param name="qtdDiasRegraMsmCampanha">Dias Regra Descanso Mesma Campanha</param>
        /// <param name="flagRegraDescTipoCampanha">Flag Regra Descanso Tipo Campanha</param>
        /// <param name="listaRegraDescTipoCampanha">Lista Regra Descanso Tipo Campanha</param>
        /// <param name="qtdDiasRegraDescQqCampanha">Dias Regra Descanso Qualquer Campanha</param>        
        /// <returns>Id Campanha    </returns>
        public void Alterar(   //Int32 codigoOrganizacao,
                               //String codigoCampanha,
                                String nomeCampanha,
                                String objetivoCampanha,
                                DateTime dataCampanhaInicio,
                                DateTime dataCampanhaTermino,
                                String tipoCampanha,
                                String statusCampanha,
                                Int32 diasVigenciaCliente,
                                DateTime dataHoraAtualizacao,
                                String observacaoCampanha,
                                String configuracaoDisparo,
                                String flagRegraDescQqCampanha,
                                String flagRegraDescMsmCampanha,
                                Int32 qtdDiasRegraMsmCampanha,
                                String flagRegraDescTipoCampanha,
                                String listaRegraDescTipoCampanha,
                                Int32 qtdDiasRegraDescQqCampanha,
                                Int32 idCampanha)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                //acessoDadosBase.AddParameter("@CMP_CD", codigoCampanha);
                acessoDadosBase.AddParameter("@CMP_NOME", nomeCampanha);
                acessoDadosBase.AddParameter("@CMP_OBJETIVO", objetivoCampanha);
                acessoDadosBase.AddParameter("@CMD_PERIODO_INI", dataCampanhaInicio);
                acessoDadosBase.AddParameter("@CMD_PERIODO_FIM", dataCampanhaTermino);
                acessoDadosBase.AddParameter("@CMP_TIPO", tipoCampanha);
                acessoDadosBase.AddParameter("@CMP_ST", statusCampanha);
                acessoDadosBase.AddParameter("@CMP_QT_DIAS_VIGENCIA_CLIENTE", diasVigenciaCliente);
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@CMP_DH_USUARIO_UPD", dataHoraAtualizacao);
                //acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@CMP_OBSERVACAO", observacaoCampanha);
                acessoDadosBase.AddParameter("@CMP_CONF_DISPARO", configuracaoDisparo);
                acessoDadosBase.AddParameter("@CMP_FL_RGR_DES_QQ_CMP", flagRegraDescQqCampanha);
                acessoDadosBase.AddParameter("@CMP_FL_RGR_DES_MSM_CMP", flagRegraDescMsmCampanha);
                acessoDadosBase.AddParameter("@CMP_RGR_DES_MSM_CMP_QT_DIAS", qtdDiasRegraMsmCampanha);
                acessoDadosBase.AddParameter("@CMP_FL_RGR_DES_CMP_TP", flagRegraDescTipoCampanha);
                acessoDadosBase.AddParameter("@CMP_RGR_DES_CMP_TP_LISTA", listaRegraDescTipoCampanha);
                acessoDadosBase.AddParameter("@CMP_RGR_DES_QQ_CMP_QT_DIAS", qtdDiasRegraDescQqCampanha);
                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prCMP_UPD");
                // MemoryCache Clean
                RemoverCache(idCampanha);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Remove do Cache as Moedas
        /// </summary>
        public void RemoverCache(Int32 idCampanha)
        {
            MemoryCache.Default.Remove(kCacheKey + idCampanha.ToString());
        }

        /// <summary>
        /// Buscar Campanhas
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="campanhaCodigo">Código da Campanha</param>
        /// <param name="campanhaNome">Nome da Campanha</param>
        /// <param name="campanhaUsuario">Usuário Campanha</param>
        /// <param name="statusCampanha">Status Campanha</param>
        /// <param name="datahoraCampanhaInicio">Data Inicial da Campanha</param>
        /// <param name="datahoraCampanhaTermino">Data Termino da Campanha</param>        
        /// <param name="campanhaCanal">Lista de Canais de Comunicação</param>
        /// <returns></returns>
        public DataTable Buscar(Int32 codigoOrganizacao,
                                      String campanhaCodigo,
                                      String campanhaNome,
                                      String campanhaUsuario,
                                      CaseBusiness.CC.Mensageria.StatusCampanha.enumStatus statusCampanha,
                                      DateTime datahoraCampanhaInicio,
                                      DateTime datahoraCampanhaTermino,
                                      String campanhaCanal)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Campanhas está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@CMP_CD", campanhaCodigo);
                acessoDadosBase.AddParameter("@CMP_NOME", campanhaNome);
                acessoDadosBase.AddParameter("@USU_ID_INS", campanhaUsuario);
                acessoDadosBase.AddParameter("@CMP_ST", CaseBusiness.CC.Mensageria.StatusCampanha.ObterDBValue(statusCampanha));
                acessoDadosBase.AddParameter("@CMD_PERIODO_INI", datahoraCampanhaInicio);
                acessoDadosBase.AddParameter("@CMD_PERIODO_FIM", datahoraCampanhaTermino);
                acessoDadosBase.AddParameter("@COMCNL_ID", campanhaCanal);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMP_SEL_BUSCAR").Tables[0];

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
       
        /// <summary>
        /// Caso tenha restrição definitiva, retorna mensagem
        /// </summary>
        /// <param name="idCampanhas">id Campanhas</param>       
        private void Consultar(Int32 idCampanhas)
        {
            DataTable dt = null;
            //String kCacheKey = "Campanha_" + idCampanhas.ToString();            
            String _kCacheKey = kCacheKey + idCampanhas.ToString();            

            try
            {
                if (MemoryCache.Default.Contains(_kCacheKey))
                {
                    dt = MemoryCache.Default[_kCacheKey] as DataTable;
                }
                else
                {
                    if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Campanhas está operando em Modo Entidade Only"); }

                    acessoDadosBase.AddParameter("@CMP_ID", idCampanhas);

                    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMP_SEL_CONSULTAR").Tables[0];
                }

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdCampanhas = Convert.ToInt32(dt.Rows[0]["CMP_ID"]);
                    CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                    CodigoCampanha = dt.Rows[0]["CMP_CD"].ToString();
                    NomeCampanha = dt.Rows[0]["CMP_NOME"].ToString();
                    ObjetivoCampanha = dt.Rows[0]["CMP_OBJETIVO"].ToString();
                    DataCampanhaInicio = Convert.ToDateTime(dt.Rows[0]["CMD_PERIODO_INI"]);
                    DataCampanhaTermino = Convert.ToDateTime(dt.Rows[0]["CMD_PERIODO_FIM"]);
                    TipoCampanha = dt.Rows[0]["CMP_TIPO"].ToString();
                    //PeriodicidadeExec = dt.Rows[0]["CMP_PERIODICIDADE_EXEC"].ToString();
                    StatusCampanha = dt.Rows[0]["CMP_ST"].ToString();

                    if (dt.Rows[0]["CMP_QT_DIAS_VIGENCIA_CLIENTE"] != DBNull.Value)
                    {
                        DiasVigenciaCliente = Convert.ToInt32(dt.Rows[0]["CMP_QT_DIAS_VIGENCIA_CLIENTE"]);
                    }

                    IdUsuarioIns = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                    NomeUsuarioIns = dt.Rows[0]["USU_NM_INS"].ToString();
                    DataUsuarioIns = Convert.ToDateTime(dt.Rows[0]["CMP_DH_USUARIO_INS"]);

                    if (dt.Rows[0]["USU_ID_UPD"] != DBNull.Value)
                    {
                        IdUsuarioUpd = Convert.ToInt32(dt.Rows[0]["USU_ID_UPD"]);
                        NomeUsuarioUpd = dt.Rows[0]["USU_NM_UPD"].ToString();
                        DataUsuarioUpd = Convert.ToDateTime(dt.Rows[0]["CMP_DH_USUARIO_UPD"]);
                    }

                    //HoraExecucao = Convert.ToDateTime(dt.Rows[0]["CMP_HR_EXECUCAO"]);

                    if (dt.Rows[0]["CMP_RGR_DES_QQ_CMP_QT_DIAS"] != DBNull.Value)
                    {
                        QtdDiasRegraDescQqCampanha = Convert.ToInt32(dt.Rows[0]["CMP_RGR_DES_QQ_CMP_QT_DIAS"]);
                    }

                    QueryCampanha = dt.Rows[0]["CMD_TX_QUERY_CAMPANHA"].ToString();
                    QueryMensuracao = dt.Rows[0]["CMD_TX_QUERY_MENSURACAO"].ToString();
                    ObservacaoCampanha = dt.Rows[0]["CMP_OBSERVACAO"].ToString();
                    ConfiguracaoDisparo = dt.Rows[0]["CMP_CONF_DISPARO"].ToString();

                    if (dt.Columns.Contains("CMP_FL_RGR_DES_QQ_CMP"))
                    {
                        FlagRegraDescQqCampanha = dt.Rows[0]["CMP_FL_RGR_DES_QQ_CMP"].ToString().Equals("S") ? true : false;
                    }

                    if (dt.Columns.Contains("CMP_FL_RGR_DES_MSM_CMP"))
                    {
                        FlagRegraDescMsmCampanha = dt.Rows[0]["CMP_FL_RGR_DES_MSM_CMP"].ToString().Equals("S") ? true : false;
                    }

                    if (dt.Rows[0]["CMP_RGR_DES_MSM_CMP_QT_DIAS"] != DBNull.Value)
                    {
                        QtdDiasRegraMsmCampanha = Convert.ToInt32(dt.Rows[0]["CMP_RGR_DES_MSM_CMP_QT_DIAS"]);
                    }

                    if (dt.Columns.Contains("CMP_FL_RGR_DES_CMP_TP"))
                    {
                        FlagRegraDescTipoCampanha = dt.Rows[0]["CMP_FL_RGR_DES_CMP_TP"].ToString().Equals("S") ? true : false;
                    }

                    ListaRegraDescTipoCampanha = dt.Rows[0]["CMP_RGR_DES_CMP_TP_LISTA"].ToString();

                    if (dt.Rows[0]["CMP_DH_PROX_EXEC"] != DBNull.Value)
                    {
                        DataProximaExec = Convert.ToDateTime(dt.Rows[0]["CMP_DH_PROX_EXEC"]);
                    }

                    __blnIsLoaded = true;
                }

                MemoryCache.Default.Set(_kCacheKey, dt,
                new CacheItemPolicy()
                {
                    SlidingExpiration = new TimeSpan(DateTime.Now.AddMinutes(kCache_SLIDINGEXPIRATION_MINUTES).Ticks - DateTime.Now.Ticks)
                });
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Verifica se campanha ja foi cadastrada
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="codigoCampanha">Código da Campanha</param>        
        public DataTable Consultar_Codigo(Int32 codigoOrganizacao,
                                            String codigoCampanha)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Campanhas está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@CMP_CD", codigoCampanha);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMP_SEL_CONSULTAR_CD").Tables[0];

                // Fill Object
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
        /// Obtem as Restrições de Alteração da Conexção
        /// </summary>
        private DataTable ObterRestricaoAlteracao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Campanha está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", IdCampanhas);

                dt = new DataTable();
                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMP_SEL_RESTRIC_UPD").Tables[0];

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


        /// <summary>
        /// Excluir uma Campanha
        /// </summary>        
        /// <param name="idCampanha">Id Campanha</param>
        public void Excluir(Int32 idCampanha)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe QualificadorDefinicao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);                

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMP_DEL");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }
        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {

            if (dt.Columns.Contains("CMP_ID")) { dt.Columns["CMP_ID"].ColumnName = "IdCampanha"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "Organizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("CMP_CD")) { dt.Columns["CMP_CD"].ColumnName = "CodigoCampanha"; }
            if (dt.Columns.Contains("CMP_NOME")) { dt.Columns["CMP_NOME"].ColumnName = "NomeCampanha"; }
            if (dt.Columns.Contains("CMP_OBJETIVO")) { dt.Columns["CMP_OBJETIVO"].ColumnName = "ObjetivoCampanha"; }
            if (dt.Columns.Contains("CMD_PERIODO_INI")) { dt.Columns["CMD_PERIODO_INI"].ColumnName = "DataCampanhaInicio"; }
            if (dt.Columns.Contains("CMD_PERIODO_FIM")) { dt.Columns["CMD_PERIODO_FIM"].ColumnName = "DataCampanhaTermino"; }
            if (dt.Columns.Contains("CMP_TIPO")) { dt.Columns["CMP_TIPO"].ColumnName = "TipoCampanha"; }
            //if (dt.Columns.Contains("CMP_PERIODICIDADE_EXEC")) { dt.Columns["CMP_PERIODICIDADE_EXEC"].ColumnName = "PeriodicidadeExec"; }
            if (dt.Columns.Contains("CMP_ST")) { dt.Columns["CMP_ST"].ColumnName = "StatusCampanha"; }
            if (dt.Columns.Contains("CMP_QT_DIAS_VIGENCIA_CLIENTE")) { dt.Columns["CMP_QT_DIAS_VIGENCIA_CLIENTE"].ColumnName = "DiasVigenciaCliente"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioIns"; }
            if (dt.Columns.Contains("CMP_DH_USUARIO_INS")) { dt.Columns["CMP_DH_USUARIO_INS"].ColumnName = "DataUsuarioIns"; }
            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "IdUsuarioUpd"; }
            if (dt.Columns.Contains("USU_NM_UPD")) { dt.Columns["USU_NM_UPD"].ColumnName = "NomeUsuarioUpd"; }
            if (dt.Columns.Contains("CMP_DH_USUARIO_UPD")) { dt.Columns["CMP_DH_USUARIO_UPD"].ColumnName = "DataUsuarioUpd"; }
            //if (dt.Columns.Contains("CMP_HR_EXECUCAO")) { dt.Columns["CMP_HR_EXECUCAO"].ColumnName = "HoraExecucao"; }
            if (dt.Columns.Contains("CMP_RGR_DES_QQ_CMP_QT_DIAS")) { dt.Columns["CMP_RGR_DES_QQ_CMP_QT_DIAS"].ColumnName = "QtdDiasRegraDescQqCampanha"; }
            if (dt.Columns.Contains("CMD_TX_QUERY_CAMPANHA")) { dt.Columns["CMD_TX_QUERY_CAMPANHA"].ColumnName = "QueryCampanha"; }
            if (dt.Columns.Contains("CMD_TX_QUERY_MENSURACAO")) { dt.Columns["CMD_TX_QUERY_MENSURACAO"].ColumnName = "QueryMensuracao"; }
            if (dt.Columns.Contains("CMP_OBSERVACAO")) { dt.Columns["CMP_OBSERVACAO"].ColumnName = "ObservacaoCampanha"; }
            if (dt.Columns.Contains("CMP_CONF_DISPARO")) { dt.Columns["CMP_CONF_DISPARO"].ColumnName = "ConfiguracaoDisparo"; }
            if (dt.Columns.Contains("CMP_FL_RGR_DES_QQ_CMP")) { dt.Columns["CMP_FL_RGR_DES_QQ_CMP"].ColumnName = "FlagRegraDescQqCampanha"; }
            if (dt.Columns.Contains("CMP_FL_RGR_DES_MSM_CMP")) { dt.Columns["CMP_FL_RGR_DES_MSM_CMP"].ColumnName = "FlagRegraDescMsmCampanha"; }
            if (dt.Columns.Contains("CMP_RGR_DES_MSM_CMP_QT_DIAS")) { dt.Columns["CMP_RGR_DES_MSM_CMP_QT_DIAS"].ColumnName = "QtdDiasRegraMsmCampanha"; }
            if (dt.Columns.Contains("CMP_FL_RGR_DES_CMP_TP")) { dt.Columns["CMP_FL_RGR_DES_CMP_TP"].ColumnName = "FlagRegraDescTipoCampanha"; }
            if (dt.Columns.Contains("CMP_RGR_DES_CMP_TP_LISTA")) { dt.Columns["CMP_RGR_DES_CMP_TP_LISTA"].ColumnName = "ListaRegraDescTipoCampanha"; }
            if (dt.Columns.Contains("CMP_DH_PROX_EXEC")) { dt.Columns["CMP_DH_PROX_EXEC"].ColumnName = "DataProximaExec"; }

            if (dt.Columns.Contains("USU_CMP_NM")) { dt.Columns["USU_CMP_NM"].ColumnName = "UsuarioCampanhaNome"; }
            if (dt.Columns.Contains("USU_CMP_ST")) { dt.Columns["USU_CMP_ST"].ColumnName = "UsuarioCampanhaStatus"; }
            if (dt.Columns.Contains("USU_ID_CMP")) { dt.Columns["USU_ID_CMP"].ColumnName = "IdUsuarioCampanha"; }

            if (dt.Columns.Contains("LISTACANAL")) { dt.Columns["LISTACANAL"].ColumnName = "ListaCanal"; }
            if (dt.Columns.Contains("LOG_ROWCOUNT")) { dt.Columns["LOG_ROWCOUNT"].ColumnName = "LogCountAlteracao"; }

            if (dt.Columns.Contains("ROWS_COUNT_UPD")) { dt.Columns["ROWS_COUNT_UPD"].ColumnName = "RestricoesAlteracaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA_UPD")) { dt.Columns["RESTRICAO_TABELA_UPD"].ColumnName = "RestricoesAlteracaoTabela"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }

        }
        #endregion Métodos
    }
}

