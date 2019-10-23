#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Scheduler
{
    public class Agendamento : BusinessBase
    {
        #region Atributos
        private Int32 _idAgendamento = Int32.MinValue;
        private Int32 _idAgendamentoDependenteExecucao = Int32.MinValue;
        private Int32 _idSistema = Int32.MaxValue;
        private String _codigoSistemaModulo = String.Empty;
        private String _tipoAgendamento = "";
        private DateTime _horaAgendamento = DateTime.MinValue;
        private DateTime _dhUltimaExec = DateTime.MinValue;
        private DateTime _dhProximaExec = DateTime.MinValue;
        private enmStatus _statusAgendamento;
        private String _descricaoStatusAgendamento = "";
        private StatusAgendamentoProcessoExecucao.enmStatusExecucao _statusExecucaoAgendamento;
        private enmTipoExecucao _tipoExecucao;
        private DateTime _horaExecucao = DateTime.MinValue;
        private String _statusNomeUsuario = "";
        private DateTime _dhManutencaoStatus = DateTime.MinValue;
        private DataTable _dtRestricoesExclusao;

        #endregion Atributos

        #region Propriedades

        public DateTime DhProximaExec
        {
            get { return _dhProximaExec; }
            set { _dhProximaExec = value; }
        }

        public DateTime DhUltimaExec
        {
            get { return _dhUltimaExec; }
            set { _dhUltimaExec = value; }
        }

        public DateTime HoraAgendamento
        {
            get { return _horaAgendamento; }
            set { _horaAgendamento = value; }
        }

        public String TipoAgendamento
        {
            get { return _tipoAgendamento; }
            set { _tipoAgendamento = value; }
        }

        //public String StatusAgenExec
        //{
        //    get { return _tipoAgenExec; }
        //    set { _tipoAgenExec = value; }
        //}

        //public String StatusAgenProcesso
        //{
        //    get { return _tipoAgenProcesso; }
        //    set { _tipoAgenProcesso = value; }
        //}

        public Int32 IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }

        public String CodigoSistemaModulo
        {
            get { return _codigoSistemaModulo; }
            set { _codigoSistemaModulo = value; }
        }

        public Int32 IdAgendamento
        {
            get { return _idAgendamento; }
            set { _idAgendamento = value; }
        }

        public Int32 IdAgendamentoDependenteExecucao
        {
            get { return _idAgendamentoDependenteExecucao; }
            set { _idAgendamentoDependenteExecucao = value; }
        }

        public enmStatus StatusAgendamento
        {
            get { return _statusAgendamento; }
            set { _statusAgendamento = value; }
        }

        public String DescricaoStatusAgendamento
        {
            get { return _descricaoStatusAgendamento; }
            set { _descricaoStatusAgendamento = value; }
        }

        public StatusAgendamentoProcessoExecucao.enmStatusExecucao StatusExecucaoAgendamento
        {
            get { return _statusExecucaoAgendamento; }
            set { _statusExecucaoAgendamento = value; }
        }

        public enmTipoExecucao TipoExecucao
        {
            get { return _tipoExecucao; }
            set { _tipoExecucao = value; }
        }

        public DateTime HoraExecucao
        {
            get { return _horaExecucao; }
            set { _horaExecucao = value; }
        }

        public String StatusNomeUsuario
        {
            get { return _statusNomeUsuario; }
            set { _statusNomeUsuario = value; }
        }

        public DateTime DhManutencaoStatus
        {
            get { return _dhManutencaoStatus; }
            set { _dhManutencaoStatus = value; }
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

        public DataTable RestricoesExclusao
        {
            get
            {
                if (_dtRestricoesExclusao == null)
                {
                    _dtRestricoesExclusao = new DataTable();
                    _dtRestricoesExclusao = ObterRestricoesExclusao();
                }

                return _dtRestricoesExclusao;
            }
        }

        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Agendamento - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Agendamento(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Agendamento
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Agendamento(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }


        /// <summary>
        /// Construtor classe Agendamento
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Agendamento()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Agendamento utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Agendamento(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Agendamento e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoGrupo">Código do Agendamento</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Agendamento(Int32 codigoAgendamento,
                         Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(codigoAgendamento);
        }

        #endregion Construtores

        #region Enums e Constantes
        public enum enmTipoExecucao { Imediato, Recorrente, EMPTY }
        public const string kTipoExecucao_IMED = "IMED";
        public const string kTipoExecucao_RECO = "RECO";

        public enum enmTipoExecucaoTexto { Imediato, Recorrente }
        private const string kTipoExecucao_IMED_Texto = "Imediato";
        private const string kTipoExecucao_RECO_Texto = "Recorrente";


        public enum enmTipoProcesso
        {
            Individual, Grupo, EMPTY
        }
        public const string kTipoProcesso_INDIV = "INDIV";
        public const string kTipoProcesso_GRUPO = "GRUPO";

        public enum enmTipoProcessoTexto
        {
            Individual, Grupo
        }
        private const string kTipoProcesso_INDIV_Texto = "Individual";
        private const string kTipoProcesso_GRUPO_Texto = "Grupo";

        public enum enmStatus { Ativo, Inativo, EMPTY }
        private const string kStatus_Ativo = "A";
        private const string kStatus_Inativo = "I";

        public enum enmStatusTexto { Ativo, Inativo }
        private const string kStatus_Ativo_Texto = "Ativo";
        private const string kStatus_Inativo_Texto = "Inativo";

        #endregion Enums e Constantes

        #region Métodos

        /// <summary>
        /// Busca o próximo processo agendado pronto para execução
        /// </summary>        
        public DataTable BuscarProcessoParaExecucao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prAGPROC_SEL_LISTAR_AGEN").Tables[0];

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
        /// Lista todos os agendamentos
        /// </summary>
        /// <param name="statusExecucao">Status de execução do Agendamento</param>
        /// <param name="statusAgendamento">Status do agendamento</param>
        /// <returns></returns>
        public DataTable Listar(StatusAgendamentoProcessoExecucao.enmStatusExecucao statusExecucao, enmStatus statusAgendamento)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@STAPE_ST", StatusAgendamentoProcessoExecucao.ObterDBValue_StatusExecucao(statusExecucao));
                acessoDadosBase.AddParameter("@STAGPROC_ST", ObterDBValue_Status(statusAgendamento));

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prAGPROC_SEL_LISTAR").Tables[0];

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
        /// Atualizar Status de Execucao de um Agendamento
        /// </summary>
        /// <param name="idAgendamento">ID Agendamento</param>
        /// <param name="stExecucaoAgendamento">Status do Execução de Agendamento do processo </param>
        public void AtualizarStatusExecucaoAgendamento(Int32 idAgendamento
                                                , StatusAgendamentoProcessoExecucao.enmStatusExecucao stExecucaoAgendamento)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Parametro está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamento);
                acessoDadosBase.AddParameter("@STAPE_ST", StatusAgendamentoProcessoExecucao.ObterDBValue_StatusExecucao(stExecucaoAgendamento));

                switch (stExecucaoAgendamento)
                {
                    case StatusAgendamentoProcessoExecucao.enmStatusExecucao.Agendado:
                        acessoDadosBase.AddParameter("@AGPROC_DH_ULT_EXECUCAO", DateTime.MinValue);
                        break;
                    default:
                        acessoDadosBase.AddParameter("@AGPROC_DH_ULT_EXECUCAO", DateTime.Now);
                        break;
                }

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prAGPROC_UPD_STATUS_EXEC_AGEN");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Atualizar Execução de um Agendamento
        /// </summary>
        /// <param name="idAgendamento">ID Agendamento</param>
        /// <param name="tipoExecucao">Tipo Execucao. Recorrente ou Imediata</param>
        /// <param name="hrAgendamento">Hora de execução do agendamento</param>
        /// <param name="intervaloExecucao">Intervalo de tempo a ter o novo agendamento. Sempre em SEGUNDOS. Não obrigatório para Tipo Execução Imediata.</param>
        public void AtualizarExecucaoAgendamento(Int32 idAgendamento
                                                  , Agendamento.enmTipoExecucao tipoExecucao
                                                  , DateTime hrAgendamento
                                                  , Int32 intervaloExecucao = -1
                                                  )
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Parametro está operando em Modo Entidade Only"); }
                if (tipoExecucao == enmTipoExecucao.Recorrente && intervaloExecucao == -1) { throw new ApplicationException("Para execução do tipo Recorrente é necessário informar um intervalo de Execução"); }

                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamento);
                acessoDadosBase.AddParameter("@AGPROC_TP_EXECUCAO", ObterDBValue_TipoExecucao(tipoExecucao));
                acessoDadosBase.AddParameter("@INTERVALO", intervaloExecucao);
                acessoDadosBase.AddParameter("@AGPROC_HR_EXECUCAO", hrAgendamento);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prAGPROC_UPD_EXECUCAO_AGEN");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consulta um Agendamento
        /// </summary>
        /// <param name="idAgendamento">ID Agendamento</param>
        private void Consultar(Int32 idAgendamento)
        {
            try
            {
                DataTable dt;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamento);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prAGPROC_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdAgendamento = Convert.ToInt32(dt.Rows[0]["AGPROC_ID"]);
                    IdAgendamentoDependenteExecucao = dt.Rows[0]["AGPROC_ID_DEPEND_EXEC"] == DBNull.Value ? Int32.MinValue : Convert.ToInt32(dt.Rows[0]["AGPROC_ID_DEPEND_EXEC"]);
                    IdSistema = Convert.ToInt32(dt.Rows[0]["SIST_ID"]);
                    if (dt.Rows[0]["SIST_CD_MODULO"] != DBNull.Value) { CodigoSistemaModulo = Convert.ToString(dt.Rows[0]["SIST_CD_MODULO"]); }
                    StatusExecucaoAgendamento = StatusAgendamentoProcessoExecucao.ObterEnum_StatusExecucao(dt.Rows[0]["STAPE_ST"].ToString().Trim());
                    StatusAgendamento = ObterEnum_Status(dt.Rows[0]["STAGPROC_ST"].ToString().Trim());
                    DescricaoStatusAgendamento = Convert.ToString(dt.Rows[0]["STAGPROC_DS"]);
                    TipoExecucao = ObterEnum_TipoExecucao(dt.Rows[0]["AGPROC_TP_EXECUCAO"].ToString().Trim());
                    HoraExecucao = Convert.ToDateTime(dt.Rows[0]["AGPROC_HR_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_HR_EXECUCAO"]);
                    DhUltimaExec = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_ULT_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_ULT_EXECUCAO"]);
                    DhProximaExec = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_PROX_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_PROX_EXECUCAO"]);
                    DhManutencaoStatus = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_USUARIO_STATUS"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_USUARIO_STATUS"]);
                    StatusNomeUsuario = Convert.ToString(dt.Rows[0]["USU_NM"]);
                    __blnIsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consulta um Agendamento
        /// </summary>
        /// <param name="idAgendamento">ID Agendamento</param>
        public Agendamento ConsultarDependenteExecucao(Int32 idAgendamento)
        {
            try
            {
                DataTable dt;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamento);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prAGPROC_SEL_CONS_DEP_EXEC").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdAgendamento = Convert.ToInt32(dt.Rows[0]["AGPROC_ID"]);
                    IdAgendamentoDependenteExecucao = dt.Rows[0]["AGPROC_ID_DEPEND_EXEC"] == DBNull.Value ? Int32.MinValue : Convert.ToInt32(dt.Rows[0]["AGPROC_ID_DEPEND_EXEC"]);
                    IdSistema = Convert.ToInt32(dt.Rows[0]["SIST_ID"]);
                    if (dt.Rows[0]["SIST_CD_MODULO"] != DBNull.Value) { CodigoSistemaModulo = Convert.ToString(dt.Rows[0]["SIST_CD_MODULO"]); }
                    StatusExecucaoAgendamento = StatusAgendamentoProcessoExecucao.ObterEnum_StatusExecucao(dt.Rows[0]["STAPE_ST"].ToString().Trim());
                    StatusAgendamento = ObterEnum_Status(dt.Rows[0]["STAGPROC_ST"].ToString().Trim());
                    DescricaoStatusAgendamento = Convert.ToString(dt.Rows[0]["STAGPROC_DS"]);
                    TipoExecucao = ObterEnum_TipoExecucao(dt.Rows[0]["AGPROC_TP_EXECUCAO"].ToString().Trim());
                    HoraExecucao = Convert.ToDateTime(dt.Rows[0]["AGPROC_HR_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_HR_EXECUCAO"]);
                    DhUltimaExec = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_ULT_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_ULT_EXECUCAO"]);
                    DhProximaExec = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_PROX_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_PROX_EXECUCAO"]);
                    DhManutencaoStatus = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_USUARIO_STATUS"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_USUARIO_STATUS"]);
                    StatusNomeUsuario = Convert.ToString(dt.Rows[0]["USU_NM"]);
                    __blnIsLoaded = true;
                }

                return this;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consulta próxima execução de um agendamento de um sistema
        /// </summary>
        /// <param name="idSistema">ID do Sistema</param>
        public Agendamento ConsultarProcessoAgendamentoSistema(Int32 idSistema)
        {
            try
            {
                DataTable dt;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@SIST_ID", idSistema);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prAGPROC_SEL_CONSULTAR_SIST").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdAgendamento = Convert.ToInt32(dt.Rows[0]["AGPROC_ID"]);
                    IdSistema = Convert.ToInt32(dt.Rows[0]["SIST_ID"]);
                    StatusExecucaoAgendamento = StatusAgendamentoProcessoExecucao.ObterEnum_StatusExecucao(dt.Rows[0]["STAPE_ST"].ToString());
                    StatusAgendamento = ObterEnum_Status(dt.Rows[0]["STAGPROC_ST"].ToString());
                    DescricaoStatusAgendamento = Convert.ToString(dt.Rows[0]["STAGPROC_DS"]);
                    TipoExecucao = ObterEnum_TipoExecucao(dt.Rows[0]["AGPROC_TP_EXECUCAO"].ToString());
                    HoraExecucao = Convert.ToDateTime(dt.Rows[0]["AGPROC_HR_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_HR_EXECUCAO"]);
                    DhUltimaExec = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_ULT_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_ULT_EXECUCAO"]);
                    DhProximaExec = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_PROX_EXECUCAO"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_PROX_EXECUCAO"]);
                    DhManutencaoStatus = Convert.ToDateTime(dt.Rows[0]["AGPROC_DH_USUARIO_STATUS"] == DBNull.Value ? DateTime.MinValue : dt.Rows[0]["AGPROC_DH_USUARIO_STATUS"]);
                    StatusNomeUsuario = Convert.ToString(dt.Rows[0]["USU_NM"]);
                    __blnIsLoaded = true;
                }
                return this;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Lista todas as proximas execuções agendadas de um sistema 
        /// </summary>        
        /// <param name="idSistema">ID do Sistema</param>
        public DataTable ListarProcessosAgendamentosSistema(Int32 idSistema)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@SIST_ID", idSistema);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prAGPROC_SEL_LISTAR_SIST").Tables[0];

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
        /// Inclui um Agendamento
        /// </summary>
        /// <param name="idSistema">ID do Sistema que terá agendamento de execução de um processo</param>
        /// <param name="nome">Nome do processo que está sendo agendado</param>
        /// <param name="statusAgendamento">Status do Agendamento do processo</param>
        /// <param name="tipoExecucao">Tipo Execucao (IMED = Imediato; DIAR = Diário)</param>
        /// <param name="horaExecucao">Hora que será feito a execucao do processo</param>
        /// <param name="dhInclusao">Data Hora Inclusao</param>
        /// <param name="idAgendamento">Parâmetro de saída com o ID do Agendamento criado</param>
        public void Incluir(Int32 idSistema,
                            String nome,
                            Agendamento.enmStatus statusAgendamento,
                            Agendamento.enmTipoExecucao tipoExecucao,
                            DateTime horaExecucao,
                            DateTime horaProximaExecucao,
                            DateTime dhInclusao,
                            ref Int32 idAgendamento)
        {
            Incluir(idSistema,
                    String.Empty,
                    nome,
                    statusAgendamento,
                    tipoExecucao,
                    false,
                    horaExecucao,
                    horaProximaExecucao,
                    dhInclusao,
                    Agendamento.kTipoProcesso_GRUPO,
                    ref idAgendamento);
        }


        /// <summary>
        /// Inclui um Agendamento
        /// </summary>
        /// <param name="idSistema">ID do Sistema que terá agendamento de execução de um processo</param>
        /// <param name="codigoSistemaModulo">Código do Módulo do Sistema</param>
        /// <param name="nome">Nome do processo que está sendo agendado</param>
        /// <param name="statusAgendamento">Status do Agendamento do processo</param>
        /// <param name="tipoExecucao">Tipo Execucao (IMED = Imediato; DIAR = Diário)</param>
        /// <param name="horaExecucao">Hora que será feito a execucao do processo</param>
        /// <param name="dhInclusao">Data Hora Inclusao</param>
        /// <param name="idAgendamento">Parâmetro de saída com o ID do Agendamento criado</param>
        public void Incluir(Int32 idSistema,
                            String codigoSistemaModulo,
                            String nome,
                            Agendamento.enmStatus statusAgendamento,
                            Agendamento.enmTipoExecucao tipoExecucao,
                            Boolean permiteConcorrenciaExecucao,
                            DateTime horaExecucao,
                            DateTime horaProximaExecucao,
                            DateTime dhInclusao,
                            String tipoProcesso,
                            ref Int32 idAgendamento)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                idAgendamento = Int32.MinValue;

                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SIST_CD_MODULO", codigoSistemaModulo);
                acessoDadosBase.AddParameter("@AGPROC_NM", nome);
                acessoDadosBase.AddParameter("@STAGPROC_ST", ObterDBValue_Status(statusAgendamento));
                acessoDadosBase.AddParameter("@STAPE_ST", StatusAgendamentoProcessoExecucao.ObterDBValue_StatusExecucao(StatusAgendamentoProcessoExecucao.enmStatusExecucao.Agendado));
                acessoDadosBase.AddParameter("@AGPROC_TP_EXECUCAO", ObterDBValue_TipoExecucao(tipoExecucao));
                acessoDadosBase.AddParameter("@AGPROC_FL_CONCORRENCIA_EXEC", permiteConcorrenciaExecucao);
                acessoDadosBase.AddParameter("@AGPROC_HR_EXECUCAO", horaExecucao);
                acessoDadosBase.AddParameter("@AGPROC_DH_PROX_EXECUCAO", horaProximaExecucao);
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@AGPROC_DH_USUARIO_INS", dhInclusao);
                acessoDadosBase.AddParameter("@AGPROC_TP_PROC", tipoProcesso);
                acessoDadosBase.AddParameter("@AGPROC_ID", Int32.MaxValue, ParameterDirection.InputOutput);

                idAgendamento = Convert.ToInt32(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prAGPROC_INS")[0]);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Inclui um Agendamento com dependência
        /// </summary>
        /// <param name="idSistema">ID do Sistema que terá agendamento de execução de um processo</param>
        /// <param name="nome">Nome do processo que está sendo agendado</param>
        /// <param name="statusAgendamentoProcesso">Status do Agendamento do processo</param>
        /// <param name="tipoExecucao">Tipo Execucao (IMED = Imediato; DIAR = Diário)</param>
        /// <param name="horaExecucao">Hora que será feito a execucao do processo</param>
        /// <param name="dhInclusao">Data Hora Inclusao</param>
        /// <param name="idAgendamentoDependenciaExecucao">ID do Agendamento que o processo sendo agendendado é dependente de execução. Este agendamento só será executado após a execução com sucesso deste agendamento informado.</param>
        /// <param name="idAgendamento">Parâmetro de saída com o ID do Agendamento criado</param>
        public void Incluir(Int32 idSistema,
                            String nome,
                            Agendamento.enmStatus statusAgendamentoProcesso,
                            Agendamento.enmTipoExecucao tipoExecucao,
                            DateTime horaExecucao,
                            DateTime horaProximaExecucao,
                            DateTime dhInclusao,
                            int idAgendamentoDependenciaExecucao,
                            ref Int32 idAgendamento)
        {
            Incluir(idSistema,
                    String.Empty,
                    nome,
                    statusAgendamentoProcesso,
                    tipoExecucao,
                    horaExecucao,
                    horaProximaExecucao,
                    dhInclusao,
                    idAgendamentoDependenciaExecucao,
                    Agendamento.enmTipoProcesso.Grupo,
                    false,
                    ref idAgendamento);
        }

        /// <summary>
        /// Inclui um Agendamento com dependência
        /// </summary>
        /// <param name="idSistema">ID do Sistema que terá agendamento de execução de um processo</param>
        /// <param name="codigoSistemaModulo">Código do Módulo do Sistema</param>
        /// <param name="nome">Nome do processo que está sendo agendado</param>
        /// <param name="statusAgendamentoProcesso">Status do Agendamento do processo</param>
        /// <param name="tipoExecucao">Tipo Execucao (IMED = Imediato; DIAR = Diário)</param>
        /// <param name="horaExecucao">Hora que será feito a execucao do processo</param>
        /// <param name="dhInclusao">Data Hora Inclusao</param>
        /// <param name="idAgendamentoDependenciaExecucao">ID do Agendamento que o processo sendo agendendado é dependente de execução. Este agendamento só será executado após a execução com sucesso deste agendamento informado.</param>
        /// <param name="idAgendamento">Parâmetro de saída com o ID do Agendamento criado</param>
        public void Incluir(Int32 idSistema,
                            String codigoSistemaModulo,
                            String nome,
                            Agendamento.enmStatus statusAgendamentoProcesso,
                            Agendamento.enmTipoExecucao tipoExecucao,
                            DateTime horaExecucao,
                            DateTime horaProximaExecucao,
                            DateTime dhInclusao,
                            int idAgendamentoDependenciaExecucao,
                            Agendamento.enmTipoProcesso tipoProcesso,
                            Boolean permiteConcorrenciaExecucao,
                            ref Int32 idAgendamento)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                idAgendamento = Int32.MinValue;

                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SIST_CD_MODULO", codigoSistemaModulo);
                acessoDadosBase.AddParameter("@AGPROC_NM", nome);
                acessoDadosBase.AddParameter("@STAGPROC_ST", ObterDBValue_Status(statusAgendamentoProcesso));
                acessoDadosBase.AddParameter("@STAPE_ST", StatusAgendamentoProcessoExecucao.ObterDBValue_StatusExecucao(StatusAgendamentoProcessoExecucao.enmStatusExecucao.Agendado));
                acessoDadosBase.AddParameter("@AGPROC_TP_EXECUCAO", ObterDBValue_TipoExecucao(tipoExecucao));
                acessoDadosBase.AddParameter("@AGPROC_HR_EXECUCAO", horaExecucao);
                acessoDadosBase.AddParameter("@AGPROC_DH_PROX_EXECUCAO", horaProximaExecucao);
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@AGPROC_DH_USUARIO_INS", dhInclusao);
                acessoDadosBase.AddParameter("@AGPROC_ID_DEPEND_EXEC", idAgendamentoDependenciaExecucao);                
                acessoDadosBase.AddParameter("@AGPROC_TP_PROC", Agendamento.kTipoProcesso_GRUPO);
                acessoDadosBase.AddParameter("@AGPROC_FL_CONCORRENCIA_EXEC", permiteConcorrenciaExecucao);
                acessoDadosBase.AddParameter("@AGPROC_ID", Int32.MaxValue, ParameterDirection.InputOutput);

                idAgendamento = Convert.ToInt32(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prAGPROC_INS_DEPEND_EXEC")[0]);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Atualiza status de um Agendamento
        /// </summary>
        /// <param name="idAgendamento">ID Agendamento</param>
        /// <param name="statusAgendamento">Status Agendamento </param>
        public void AlterarStatus(Int32 idAgendamento,
                                  Agendamento.enmStatus statusAgendamento)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamento);
                acessoDadosBase.AddParameter("@STAGPROC_ST", ObterDBValue_Status(statusAgendamento));
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@AGPROC_DH_USUARIO_UPD", DateTime.Now);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prAGPROC_UPD_STATUS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Altera um Agendamento
        /// </summary>
        /// <param name="idSistema">ID do Sistema que terá agendamento de execução de um processo</param>
        /// <param name="statusAgendamentoProcessamentoExecucao">Status Agendamento Processamento Execucao</param>
        /// <param name="tipoExecucao">Tipo Execucao</param>
        /// <param name="horaExecucao">Hora que será feito a execucao do processo</param>
        /// <param name="dhInclusao">Data Hora Inclusao</param>
        /// <param name="idAgendamento">ID Agendamento</param>
        public void Alterar(Int32 idSistema,
                            StatusAgendamentoProcessoExecucao.enmStatusExecucao statusAgendamentoProcessamentoExecucao,
                            Agendamento.enmTipoExecucao tipoExecucao,
                            DateTime horaExecucao,
                            DateTime horaProximaExecucao,
                            DateTime dhInclusao,
                            Int32 idAgendamento)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@STAPE_ST", StatusAgendamentoProcessoExecucao.ObterDBValue_StatusExecucao(statusAgendamentoProcessamentoExecucao));
                acessoDadosBase.AddParameter("@AGPROC_TP_EXECUCAO", ObterDBValue_TipoExecucao(tipoExecucao));
                acessoDadosBase.AddParameter("@AGPROC_HR_EXECUCAO", horaExecucao);
                acessoDadosBase.AddParameter("@AGPROC_DH_PROX_EXECUCAO", horaProximaExecucao);
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@AGPROC_DH_USUARIO_UPD", dhInclusao);
                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamento);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prAGPROC_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Exclui um Agendamento 
        /// </summary>
        /// <param name="idAgendamento">ID Agendamento</param>
        public void Excluir(Int32 idAgendamento)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamento);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prAGPROC_DEL");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Obtem as Restrições de Exclusão da Regra carregada
        /// </summary>
        private DataTable ObterRestricoesExclusao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only"); }

                //acessoDadosBase.AddParameter("@SOFTO_CD", CodigoAgendamento);

                dt = new DataTable();
                //dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                //"prSOFTO_SEL_RESTRIC_DEL").Tables[0];

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
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("AGPROC_ID")) { dt.Columns["AGPROC_ID"].ColumnName = "IdAgendamentoProcesso"; }
            if (dt.Columns.Contains("SIST_ID")) { dt.Columns["SIST_ID"].ColumnName = "IdSistema"; }
            if (dt.Columns.Contains("SIST_CD_MODULO")) { dt.Columns["SIST_CD_MODULO"].ColumnName = "CodigoSistemaModulo"; }
            if (dt.Columns.Contains("AGPROC_NM")) { dt.Columns["AGPROC_NM"].ColumnName = "Nome"; }
            if (dt.Columns.Contains("ANY_ID")) { dt.Columns["ANY_ID"].ColumnName = "IdAnalytics"; }
            if (dt.Columns.Contains("STAGPROC_ST")) { dt.Columns["STAGPROC_ST"].ColumnName = "StAgenProcesso"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioIns"; }
            if (dt.Columns.Contains("AGPROC_DH_USUARIO_INS")) { dt.Columns["AGPROC_DH_USUARIO_INS"].ColumnName = "DhUsuarioIns"; }
            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "IdUsuarioUpd"; }
            if (dt.Columns.Contains("AGPROC_DH_USUARIO_UPD")) { dt.Columns["AGPROC_DH_USUARIO_UPD"].ColumnName = "DhUsuarioUpd"; }
            if (dt.Columns.Contains("STAPE_ST")) { dt.Columns["STAPE_ST"].ColumnName = "StAgenExec"; }
            if (dt.Columns.Contains("AGPROC_TP_EXECUCAO")) { dt.Columns["AGPROC_TP_EXECUCAO"].ColumnName = "TpExecucao"; }
            if (dt.Columns.Contains("AGPROC_HR_EXECUCAO")) { dt.Columns["AGPROC_HR_EXECUCAO"].ColumnName = "HrAgendamento"; }
            if (dt.Columns.Contains("AGPROC_DH_ULT_EXECUCAO")) { dt.Columns["AGPROC_DH_ULT_EXECUCAO"].ColumnName = "DHUltimaExec"; }
            if (dt.Columns.Contains("AGPROC_DH_PROX_EXECUCAO")) { dt.Columns["AGPROC_DH_PROX_EXECUCAO"].ColumnName = "DHProximaExec"; }
            if (dt.Columns.Contains("AGPROC_USU_ID_STATUS")) { dt.Columns["AGPROC_USU_ID_STATUS"].ColumnName = "IdUsuarioStatus"; }
            if (dt.Columns.Contains("AGPROC_DH_USUARIO_STATUS")) { dt.Columns["AGPROC_DH_USUARIO_STATUS"].ColumnName = "DhUsuarioStatus"; }
            if (dt.Columns.Contains("ANY_FL_ORIGEM")) { dt.Columns["ANY_FL_ORIGEM"].ColumnName = "FlOrigem"; }
            if (dt.Columns.Contains("ANY_FL_VOLUME_UTILIZADO")) { dt.Columns["ANY_FL_VOLUME_UTILIZADO"].ColumnName = "FlVolumeUtil"; }
            if (dt.Columns.Contains("AGPROC_ID_DEPEND_EXEC")) { dt.Columns["AGPROC_ID_DEPEND_EXEC"].ColumnName = "IdAgendamentoProcessoDependenciaExecucao"; }
            if (dt.Columns.Contains("AGPROC_FL_CONCORRENCIA_EXEC")) { dt.Columns["AGPROC_FL_CONCORRENCIA_EXEC"].ColumnName = "PermiteConcorrenciaExecucao"; }
            if (dt.Columns.Contains("AGPROC_TP_PROC")) { dt.Columns["AGPROC_TP_PROC"].ColumnName = "TipoProcesso"; }

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }

            if (dt.Columns.Contains("SIST_NM")) { dt.Columns["SIST_NM"].ColumnName = "NomeSistema"; }
            if (dt.Columns.Contains("PROCMISC_ID")) { dt.Columns["PROCMISC_ID"].ColumnName = "IdMiscelanea"; }
            if (dt.Columns.Contains("PROCMISC_NR_INTERVALO_EXEC")) { dt.Columns["PROCMISC_NR_INTERVALO_EXEC"].ColumnName = "IntervaloExec"; }

        }

        #region Suporte Métodos

        static public enmTipoExecucao ObterEnum_TipoExecucao(String tipoExecucao)
        {
            enmTipoExecucao enumItem = enmTipoExecucao.EMPTY;

            switch (tipoExecucao.Trim())
            {
                case kTipoExecucao_RECO: enumItem = enmTipoExecucao.Recorrente; break;
                case kTipoExecucao_IMED: enumItem = enmTipoExecucao.Imediato; break;
            }
            return enumItem;
        }

        static internal String ObterDBValue_TipoProcesso(enmTipoProcesso tipoProcesso)
        {
            String dbvalue = String.Empty;

            switch (tipoProcesso)
            {
                case enmTipoProcesso.Individual: dbvalue = kTipoProcesso_INDIV; break;
                case enmTipoProcesso.Grupo: dbvalue = kTipoProcesso_GRUPO; break;
            }
            return dbvalue;
        }


        static public enmTipoProcesso ObterEnum_TipoProcesso(String tipoProcesso)
        {
            enmTipoProcesso enumItem = enmTipoProcesso.EMPTY;

            switch (tipoProcesso.Trim())
            {
                case kTipoProcesso_INDIV:
                    enumItem = enmTipoProcesso.Individual;
                    break;
                case kTipoProcesso_GRUPO:
                    enumItem = enmTipoProcesso.Grupo;
                    break;
            }
            return enumItem;
        }

        static public String ObterDBValue_TipoExecucao(enmTipoExecucao tipoExecucao)
        {
            String dbvalue = String.Empty;

            switch (tipoExecucao)
            {
                case enmTipoExecucao.Imediato:
                    dbvalue = kTipoExecucao_IMED;
                    break;
                case enmTipoExecucao.Recorrente:
                    dbvalue = kTipoExecucao_RECO;
                    break;
            }
            return dbvalue;
        }


        static public enmStatus ObterEnum_Status(String status)
        {
            enmStatus enumItem = enmStatus.EMPTY;

            switch (status.Trim())
            {
                case kStatus_Ativo: enumItem = enmStatus.Ativo; break;
                case kStatus_Inativo: enumItem = enmStatus.Inativo; break;
            }
            return enumItem;
        }

        static public String ObterDBValue_Status(enmStatus status)
        {
            String dbvalue = String.Empty;

            switch (status)
            {
                case enmStatus.Ativo: dbvalue = kStatus_Ativo; break;
                case enmStatus.Inativo: dbvalue = kStatus_Inativo; break;
            }
            return dbvalue;
        }

        #endregion

        #endregion Métodos
    }
}