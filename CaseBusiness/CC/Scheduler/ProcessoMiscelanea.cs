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
    public class ProcessoMiscelanea : BusinessBase
    {
        #region Enums e Constantes
        public enum enumStatus { EMPTY, ATIVO, INATIVO }
        public const string kStatus_ATIVO_DBValue = "ATIVO";
        public const string kStatus_INATIVO_DBValue = "INATI";
        public const string kStatus_EMPTY_DBValue = "";

        public enum enumStatusTexto { Ativo, Inativo }
        public const string kStatus_ATIVO_Texto = "Ativo";
        public const string kStatus_INATIVO_Texto = "Inativo";
        #endregion Enums e Constantes

        #region Atributos
        private Int32 _idProcessoMiscelanea = Int32.MinValue;
        private Int32 _idSistema = Int32.MinValue;
        private String _cdModulo = String.Empty;
        private String _nomeProcesso = String.Empty;
        private String _status = String.Empty;
        private DateTime _dhUltimaExecucao = DateTime.MinValue;
        private String _statusUltimaExecucao = String.Empty;
        private DateTime _dhProximaExecucao = DateTime.MinValue;
        private String _tipoExecucao;
        private Int32 _intervaloExecucao = Int32.MinValue;
        private Boolean _concorrenciaExecucao;
        private String _nomeProcedure = String.Empty;
        private String _bancoDadosExecucao = String.Empty;
        private Int32 _idAgendamento = Int32.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdProcessoMiscelanea
        {
            get { return _idProcessoMiscelanea; }
            set { _idProcessoMiscelanea = value; }
        }
        public Int32 IdSistema
        {
            get { return _idSistema; }
            set { _idSistema = value; }
        }
        public String CdModulo
        {
            get { return _cdModulo; }
            set { _cdModulo = value; }
        }
        public String NomeProcesso
        {
            get { return _nomeProcesso; }
            set { _nomeProcesso = value; }
        }
        public String Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public DateTime DhUltimaExecucao
        {
            get { return _dhUltimaExecucao; }
            set { _dhUltimaExecucao = value; }
        }
        public String StatusUltimaExecucao
        {
            get { return _statusUltimaExecucao; }
            set { _statusUltimaExecucao = value; }
        }
        public DateTime DhProximaExecucao
        {
            get { return _dhProximaExecucao; }
            set { _dhProximaExecucao = value; }
        }
        public String TipoExecucao
        {
            get { return _tipoExecucao; }
            set { _tipoExecucao = value; }
        }
        public Int32 IntervaloExecucao
        {
            get { return _intervaloExecucao; }
            set { _intervaloExecucao = value; }
        }
        public Boolean ConcorrenciaExecucao
        {
            get { return _concorrenciaExecucao; }
            set { _concorrenciaExecucao = value; }
        }
        public String NomeProcedure
        {
            get { return _nomeProcedure; }
            set { _nomeProcedure = value; }
        }
        public String BancoDadosExecucao
        {
            get { return _bancoDadosExecucao; }
            set { _bancoDadosExecucao = value; }
        }
        public Int32 IdAgendamento
        {
            get { return _idAgendamento; }
            set { _idAgendamento = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe ProcessoMiscelanea
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ProcessoMiscelanea(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }
        /// <summary>
        /// Construtor classe ProcessoMiscelanea
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="bancoDeDados">Banco de dados a ter a tarefa executada</param>
        public ProcessoMiscelanea(CaseBusiness.Framework.BancoDeDados bancoDeDados, Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(bancoDeDados);
        }

        /// <summary>
        /// Construtor classe ProcessoMiscelanea utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public ProcessoMiscelanea(Int32 idUsuarioManutencao,
                                  CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe ProcessoMiscelanea
        /// </summary>
        /// <param name="bancoDeDados">Banco de dados a ter a tarefa executada</param>
        public ProcessoMiscelanea(CaseBusiness.Framework.BancoDeDados bancoDeDados)
        {
            acessoDadosBase = new AcessoDadosBase(bancoDeDados);
        }

        /// <summary>
        /// Construtor classe Filtro e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idProcessoMiscelanea">Código do Filtro</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ProcessoMiscelanea(Int32 idProcessoMiscelanea,
                                  Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idProcessoMiscelanea);
        }
        #endregion Construtores

        #region Métodos
        public DataTable ConsultarAgendamento(Int32 idAgendamento)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly)
                {
                    throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only");
                }

                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamento);
                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prPROCMISC_SEL_CONSULTAR_AGEN").Tables[0];

                RenomearColunas(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        public DataTable Buscar(String nomeProcesso,
                                String status,
                                String sistema)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ProcessoMiscelanea está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PROCMISC_NM", nomeProcesso.Trim());
                acessoDadosBase.AddParameter("@SIST_ID", sistema);
                acessoDadosBase.AddParameter("@PROCMISC_ST", status);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prPROCMISC_SEL_BUSCAR").Tables[0];

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

        private void Consultar(Int32 idProcessoMiscelanea)
        {
            try
            {
                DataTable dt;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ProcessoMiscelanea está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PROCMISC_ID", idProcessoMiscelanea);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prPROCMISC_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdProcessoMiscelanea = Convert.ToInt32(dt.Rows[0]["PROCMISC_ID"]);
                    IdSistema = Convert.ToInt32(dt.Rows[0]["SIST_ID"]);
                    CdModulo = Convert.ToString(dt.Rows[0]["SIST_CD_MODULO"]);
                    NomeProcesso = Convert.ToString(dt.Rows[0]["PROCMISC_NM"]);
                    Status = Convert.ToString(dt.Rows[0]["PROCMISC_ST"]);
                    DhUltimaExecucao = dt.Rows[0]["PROCMISC_DH_ULT_EXEC"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["PROCMISC_DH_ULT_EXEC"]) : DateTime.MinValue;
                    StatusUltimaExecucao = Convert.ToString(dt.Rows[0]["PROCMISC_ST_ULT_EXEC"]);
                    DhProximaExecucao = dt.Rows[0]["PROCMISC_DH_PROX_EXEC"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["PROCMISC_DH_PROX_EXEC"]) : DateTime.MinValue;
                    TipoExecucao = dt.Rows[0]["AGPROC_TP_EXEC"].ToString().Trim();
                    IntervaloExecucao = dt.Rows[0]["PROCMISC_NR_INTERVALO_EXEC"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["PROCMISC_NR_INTERVALO_EXEC"]) : 0;
                    ConcorrenciaExecucao = Convert.ToBoolean(dt.Rows[0]["PROCMISC_FL_CONCORR_EXEC"]);
                    NomeProcedure = Convert.ToString(dt.Rows[0]["PROCMISC_NM_PROCEDURE"]);
                    BancoDadosExecucao = Convert.ToString(dt.Rows[0]["PROCMISC_NM_DB_EXEC"]);
                    IdAgendamento = dt.Rows[0]["AGPROC_ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["AGPROC_ID"]) : 0;

                    __blnIsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        public void Incluir(Int32 idSistema,
                            String cdModulo,
                            String nomeProcesso,
                            String status,
                            DateTime dhProximaExecucao,
                            String tipoExecucao,
                            Int32 intervaloExecucao,
                            Boolean concorrenciaExecucao,
                            String nomeProcedure,
                            String bancoDadosExecucao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ProcessoMiscelanea está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SIST_CD_MODULO", cdModulo);
                acessoDadosBase.AddParameter("@PROCMISC_NM", nomeProcesso);
                acessoDadosBase.AddParameter("@PROCMISC_ST", status);
                acessoDadosBase.AddParameter("@PROCMISC_DH_PROX_EXEC", dhProximaExecucao);
                acessoDadosBase.AddParameter("@AGPROC_TP_EXEC", tipoExecucao);
                acessoDadosBase.AddParameter("@PROCMISC_NR_INTERVALO_EXEC", intervaloExecucao);
                acessoDadosBase.AddParameter("@PROCMISC_FL_CONCORR_EXEC", concorrenciaExecucao);
                acessoDadosBase.AddParameter("@PROCMISC_NM_PROCEDURE", nomeProcedure);
                acessoDadosBase.AddParameter("@PROCMISC_NM_DB_EXEC", bancoDadosExecucao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prPROCMISC_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        public void Alterar(Int32 idProcessoMiscelanea,
                            Int32 idSistema,
                            String cdModulo,
                            String nomeProcesso,
                            String status,
                            DateTime dhProximaExecucao,
                            String tipoExecucao,
                            Int32 intervaloExecucao,
                            Boolean concorrenciaExecucao,
                            String nomeProcedure,
                            String bancoDadosExecucao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ProcessoMiscelanea está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PROCMISC_ID", idProcessoMiscelanea);
                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SIST_CD_MODULO", cdModulo);
                acessoDadosBase.AddParameter("@PROCMISC_NM", nomeProcesso);
                acessoDadosBase.AddParameter("@PROCMISC_ST", status);
                acessoDadosBase.AddParameter("@PROCMISC_DH_PROX_EXEC", dhProximaExecucao);
                acessoDadosBase.AddParameter("@AGPROC_TP_EXEC", tipoExecucao);
                acessoDadosBase.AddParameter("@PROCMISC_NR_INTERVALO_EXEC", intervaloExecucao);
                acessoDadosBase.AddParameter("@PROCMISC_FL_CONCORR_EXEC", concorrenciaExecucao);
                acessoDadosBase.AddParameter("@PROCMISC_NM_PROCEDURE", nomeProcedure);
                acessoDadosBase.AddParameter("@PROCMISC_NM_DB_EXEC", bancoDadosExecucao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prPROCMISC_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        public void AlterarStatus(Int32 idProcessoMiscelanea,
                                  String status,
                                  Int32 idAgendamentoProcesso)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ProcessoMiscelanea está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PROCMISC_ID", idProcessoMiscelanea);
                acessoDadosBase.AddParameter("@PROCMISC_ST", status);
                acessoDadosBase.AddParameter("@AGPROC_ID", idAgendamentoProcesso);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prPROCMISC_UPD_STATUS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("PROCMISC_ID")) { dt.Columns["PROCMISC_ID"].ColumnName = "IdProcessoMiscelanea"; }
            if (dt.Columns.Contains("PROCMISC_NM")) { dt.Columns["PROCMISC_NM"].ColumnName = "NomeProcessoMiscelanea"; }
            if (dt.Columns.Contains("PROCMISC_ST")) { dt.Columns["PROCMISC_ST"].ColumnName = "Status"; }
            if (dt.Columns.Contains("PROCMISC_DH_ULT_EXEC")) { dt.Columns["PROCMISC_DH_ULT_EXEC"].ColumnName = "DataHoraUltimaExecucao"; }
            if (dt.Columns.Contains("PROCMISC_DH_PROX_EXEC")) { dt.Columns["PROCMISC_DH_PROX_EXEC"].ColumnName = "DataHoraProximaExecucao"; }
            if (dt.Columns.Contains("AGPROC_TP_EXEC")) { dt.Columns["AGPROC_TP_EXEC"].ColumnName = "TpExecucao"; }
            if (dt.Columns.Contains("PROCMISC_NR_INTERVALO_EXEC")) { dt.Columns["PROCMISC_NR_INTERVALO_EXEC"].ColumnName = "IntervaloExecucao"; }
            if (dt.Columns.Contains("PROCMISC_FL_CONCORR_EXEC")) { dt.Columns["PROCMISC_FL_CONCORR_EXEC"].ColumnName = "PermiteConcorrenciaExecucao"; }
            if (dt.Columns.Contains("PROCMISC_NM_PROCEDURE")) { dt.Columns["PROCMISC_NM_PROCEDURE"].ColumnName = "NomeProcedure"; }
            if (dt.Columns.Contains("PROCMISC_FL_PARAM_ENT")) { dt.Columns["PROCMISC_FL_PARAM_ENT"].ColumnName = "PossuiParametroEntrada"; }
            if (dt.Columns.Contains("PROCMISC_NM_DB_EXEC")) { dt.Columns["PROCMISC_NM_DB_EXEC"].ColumnName = "NomeBancoExecucao"; }
            if (dt.Columns.Contains("SIST_ID")) { dt.Columns["SIST_ID"].ColumnName = "IdSistema"; }
            if (dt.Columns.Contains("SIST_NM")) { dt.Columns["SIST_NM"].ColumnName = "NomeSistema"; }
            if (dt.Columns.Contains("SIST_CD_MODULO")) { dt.Columns["SIST_CD_MODULO"].ColumnName = "CodigoModulo"; }
            if (dt.Columns.Contains("PROCMISC_ST_ULT_EXEC")) { dt.Columns["PROCMISC_ST_ULT_EXEC"].ColumnName = "StatusUltimaExecucao"; }
            if (dt.Columns.Contains("AGPROC_ID")) { dt.Columns["AGPROC_ID"].ColumnName = "IdAgendamentoProcesso"; }

            if (dt.Columns.Contains("QTD_LOG_EXECUCAO")) { dt.Columns["QTD_LOG_EXECUCAO"].ColumnName = "QtdLogExecucao"; }
        }

        public static String ObterDBValue(enumStatus enmstatus)
        {
            String _dbvalue = String.Empty;

            switch (enmstatus)
            {
                case enumStatus.ATIVO: _dbvalue = kStatus_ATIVO_DBValue; break;
                case enumStatus.INATIVO: _dbvalue = kStatus_INATIVO_DBValue; break;
            }

            return _dbvalue;
        }

        public static enumStatus ObterEnum(String status)
        {
            enumStatus _enmstatus = enumStatus.EMPTY;

            if (status.Trim().Length == 0)
            {
                _enmstatus = enumStatus.EMPTY;
            }
            else
            {
                switch (status)
                {
                    case kStatus_ATIVO_DBValue: _enmstatus = enumStatus.ATIVO; break;
                    case kStatus_INATIVO_DBValue: _enmstatus = enumStatus.INATIVO; break;
                }
            }
            return _enmstatus;
        }

        public static String ObterTexto(enumStatus enmStatus)
        {
            String DBValue = String.Empty;

            switch (enmStatus)
            {
                case enumStatus.ATIVO: DBValue = kStatus_ATIVO_Texto; break;
                case enumStatus.INATIVO: DBValue = kStatus_INATIVO_Texto; break;
            }

            return DBValue;
        }
        #endregion
    }
}