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
    public class ProcessoMiscelaneaExecucao : BusinessBase
    {
        #region Atributos
        private Int32 _idProcessoMiscelanea = Int32.MinValue;
        private String _statusExecucao = String.Empty;
        private Int32 _idProcessoMiscelaneaExecucao = Int32.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdProcessoMiscelanea
        {
            get { return _idProcessoMiscelanea; }
            set { _idProcessoMiscelanea = value; }
        }

        public String StatusExecucao
        {
            get { return _statusExecucao; }
            set { _statusExecucao = value; }
        }

        public Int32 IdProcessoMiscelaneaExecucao
        {
            get { return _idProcessoMiscelaneaExecucao; }
            set { _idProcessoMiscelaneaExecucao = value; }
        }
        #endregion Propriedades

        #region Construtores

        /// <summary>
        /// Construtor classe ProcessoMiscelaneaExecucao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ProcessoMiscelaneaExecucao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }
        /// <summary>
        /// Construtor classe ProcessoMiscelaneaExecucao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="bancoDeDados">Banco de dados a ter a tarefa executada</param>
        public ProcessoMiscelaneaExecucao(CaseBusiness.Framework.BancoDeDados bancoDeDados, Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(bancoDeDados);
        }

        /// <summary>
        /// Construtor classe ProcessoMiscelaneaExecucao
        /// </summary>
        /// <param name="bancoDeDados">Banco de dados a ter a tarefa executada</param>
        public ProcessoMiscelaneaExecucao(CaseBusiness.Framework.BancoDeDados bancoDeDados)
        {
            acessoDadosBase = new AcessoDadosBase(bancoDeDados);
        }

        /// <summary>
        /// Construtor classe Filtro e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idProcessoMiscelanea">Código do Filtro</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ProcessoMiscelaneaExecucao(Int32 idProcessoMiscelanea,
                                          Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idProcessoMiscelanea);
        }
        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Inclui um registro no Processo Miscelanea Execucao
        /// </summary>
        /// <param name="IdProcessoMiscelanea">ID do processo em execução</param>
        /// <param name="dataHoraInicio">Data/Hora de inicio da execução</param>
        /// <param name="dataHoraFim">Data/Hora de fim da execução</param>
        /// <param name="statusExecucao">Status da Execução</param>
        /// <param name="descricao">Descrição/Mensagens da execução</param>
        public Int32 Incluir(Int32 IdProcessoMiscelanea, DateTime dataHoraInicio, DateTime dataHoraFim, CaseBusiness.CC.Scheduler.StatusAgendamentoProcessoExecucao.enmStatusExecucao statusExecucao, String descricao)
        {
            Int32 _id = Int32.MinValue;

            try
            {
                if (ModoEntidadeOnly)
                {
                    throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only");
                }

                acessoDadosBase.AddParameter("@PROCMISC_ID", IdProcessoMiscelanea);
                acessoDadosBase.AddParameter("@PROCMISCEX_DH_INI", dataHoraInicio);
                acessoDadosBase.AddParameter("@PROCMISCEX_DH_FIM", dataHoraFim);
                acessoDadosBase.AddParameter("@STAPE_ST", StatusAgendamentoProcessoExecucao.ObterDBValue_StatusExecucao(statusExecucao));
                acessoDadosBase.AddParameter("@PROCMISCEX_DS", descricao);
                acessoDadosBase.AddParameter("@PROCMISCEX_ID", Int32.MaxValue, ParameterDirection.InputOutput);

                _id = Convert.ToInt32(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure,
                                                    "prPROCMISCEX_INS")[0]);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return _id;
        }

        /// <summary>
        /// Inclui um registro no Processo Miscelanea Execucao
        /// </summary>
        /// <param name="IdProcessoMiscelanea">ID do processo em execução</param>
        /// <param name="IdProcessoMiscExec">ID do processo Miscelanea em execução</param>
        /// <param name="dataHoraInicio">Data/Hora de inicio da execução</param>
        /// <param name="dataHoraFim">Data/Hora de fim da execução</param>
        /// <param name="statusExecucao">Status da Execução</param>
        /// <param name="descricao">Descrição/Mensagens da execução</param>
        public void Atualizar(Int32 IdProcessoMiscelanea,
                                Int32 IdProcessoMiscExec,
                                DateTime dataHoraInicio,
                                DateTime dataHoraFim,
                                CaseBusiness.CC.Scheduler.StatusAgendamentoProcessoExecucao.enmStatusExecucao statusExecucao,
                                String descricao)
        {

            try
            {
                if (ModoEntidadeOnly)
                {
                    throw new ApplicationException("Esta Instância da classe Agendamento está operando em Modo Entidade Only");
                }

                acessoDadosBase.AddParameter("@PROCMISC_ID", IdProcessoMiscelanea);
                acessoDadosBase.AddParameter("@PROCMISCEX_DH_INI", dataHoraInicio);
                acessoDadosBase.AddParameter("@PROCMISCEX_DH_FIM", dataHoraFim);
                acessoDadosBase.AddParameter("@STAPE_ST", StatusAgendamentoProcessoExecucao.ObterDBValue_StatusExecucao(statusExecucao));
                acessoDadosBase.AddParameter("@PROCMISCEX_DS", descricao);
                acessoDadosBase.AddParameter("@PROCMISCEX_ID", IdProcessoMiscExec);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                    "prPROCMISCEX_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consulta um registro no Processo Miscelanea Execucao
        /// </summary>
        /// <param name="IdProcessoMiscelanea">ID do processo em execução</param>
        public void Consultar(Int32 idProcessoMiscelanea)
        {
            try
            {
                DataTable dt;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ProcessoMiscelaneaExecucao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PROCMISC_ID", idProcessoMiscelanea);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prPROCMISCEX_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdProcessoMiscelanea = Convert.ToInt32(dt.Rows[0]["PROCMISC_ID"]);
                    StatusExecucao = Convert.ToString(dt.Rows[0]["STAPE_ST"]);
                    IdProcessoMiscelaneaExecucao = Convert.ToInt32(dt.Rows[0]["PROCMISCEX_ID"]);
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
        /// Consulta um registro no Processo Miscelanea Execucao
        /// </summary>
        /// <param name="IdProcessoMiscelanea">ID do processo em execução</param>
        public DataTable Buscar(Int32 idProcessoMiscelanea)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ProcessoMiscelaneaExecucao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PROCMISC_ID", idProcessoMiscelanea);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prPROCMISCEX_SEL_BUSCAR").Tables[0];

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

        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("PROCMISC_ID")) { dt.Columns["PROCMISC_ID"].ColumnName = "IdProcessoMiscelanea"; }
            if (dt.Columns.Contains("PROCMISCEX_DH_INI")) { dt.Columns["PROCMISCEX_DH_INI"].ColumnName = "DataHoraInicioExecucao"; }
            if (dt.Columns.Contains("PROCMISCEX_DH_FIM")) { dt.Columns["PROCMISCEX_DH_FIM"].ColumnName = "DataHoraFimExecucao"; }
            if (dt.Columns.Contains("STAPE_ST")) { dt.Columns["STAPE_ST"].ColumnName = "StatusAgendamentoProcessoExecucao"; }
            if (dt.Columns.Contains("PROCMISCEX_DS")) { dt.Columns["PROCMISCEX_DS"].ColumnName = "DescricaoProcessoMiscelaneaExecucao"; }
            if (dt.Columns.Contains("PROCMISCEX_ID")) { dt.Columns["PROCMISCEX_ID"].ColumnName = "IdProcessoMiscelaneaExecucao"; }
        }
        #endregion
    }
}
