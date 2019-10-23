#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Mensageria
{
    public class MensagemErroLog : BusinessBase
    {
        #region Atributos
        private Int32 _idMensagemLog = Int32.MinValue;
        private Int64 _idMensagem = Int64.MinValue;
        private String _descricaoErro = "";
        private String _descricaoStackTrace = "";
        private Int32 _idUsuarioIns = Int32.MinValue;
        private DateTime _dhUsarioIns = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdMensagemLog
        {
            get { return _idMensagemLog; }
            set { _idMensagemLog = value; }
        }

        public Int64 IdMensagem
        {
            get { return _idMensagem; }
            set { _idMensagem = value; }
        }

        public String DescricaosErro
        {
            get { return _descricaoErro; }
            set { _descricaoErro = value; }
        }

        public String DescricaoStackTrace
        {
            get { return _descricaoStackTrace; }
            set { _descricaoStackTrace = value; }
        }

        public DateTime DhUsarioIns
        {
            get { return _dhUsarioIns; }
            set { _dhUsarioIns = value; }
        }

        public Int32 IdUsuarioIns
        {
            get { return _idUsuarioIns; }
            set { _idUsuarioIns = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe AnalyticsExecucaoErro - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public MensagemErroLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe AnalyticsExecucaoErro
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public MensagemErroLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe AnalyticsExecucaoErro utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public MensagemErroLog(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores
        
        #region Métodos
        /// <summary>
        /// Inclir um Log de Mensageria Erro
        /// </summary>
        /// <param name="descricaoErro">Descrição do Erro</param>
        /// <param name="descricaoStackTrace">Descrição do Stack Trace</param>
        /// <param name="dhUsarioIns">Data e Hora da Inclusão do Erro</param>
        public void Incluir(String idMensagem, 
                            String descricaoErro,
                            String descricaoStackTrace,
                            DateTime dhUsarioIns)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemErroLog está operando em Modo Entidade Only"); }
                
                if (idMensagem != "0")
                {
                    acessoDadosBase.AddParameter("MGSMS_ID", idMensagem);
                    acessoDadosBase.AddParameter("@MGLOGERRO_DS_ERRO", descricaoErro);
                    acessoDadosBase.AddParameter("@MGLOGERRO_DS_STACK_TRACE", descricaoStackTrace);
                    acessoDadosBase.AddParameter("@MGLOGERRO_DH_INCLUSAO", dhUsarioIns);
                    acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);

                    acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGLOGERRO_INS");
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar AnalyticsExecucaoErro
        /// </summary>
        /// <param name="nomeRamoAtividade">Codigo Org</param>
        /// <param name="nomeRamoAtividade">Nome Analytics</param>
        public DataTable Buscar(Int64 idMensagem)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MensagemErroLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagem);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGLOGERRO_SEL_BUSCAR").Tables[0];

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
            if (dt.Columns.Contains("MGLOGERRO_ID")) { dt.Columns["MGLOGERRO_ID"].ColumnName = "IdMensageLog"; }
            if (dt.Columns.Contains("MGSMS_ID")) { dt.Columns["MGMSG_ID"].ColumnName = "IdMensagem"; }
            if (dt.Columns.Contains("MGLOGERRO_DS_ERRO")) { dt.Columns["MGLOGERRO_DS_ERRO"].ColumnName = "DescricaoErro"; }
            if (dt.Columns.Contains("MGLOGERRO_DS_STACK_TRACE")) { dt.Columns["MGLOGERRO_DS_STACK_TRACE"].ColumnName = "DescricaoStackTrace"; }
            if (dt.Columns.Contains("MGLOGERRO_DH_INCLUSAO")) { dt.Columns["MGLOGERRO_DH_INCLUSAO"].ColumnName = "DataHoraInclusao"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioResponsavel"; }
        }
        #endregion Métodos
    }
}
