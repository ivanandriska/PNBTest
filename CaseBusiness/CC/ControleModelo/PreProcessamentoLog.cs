#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.ControleModelo
{
    public class PreProcessamentoLog : BusinessBase
    {
        #region Atributos
        private Int32 _idPreProcessamentoLog = Int32.MinValue;
        private DateTime _dhErro = DateTime.MinValue;
        private String _descricaoErro = String.Empty;
        private Int32 _numeroErro = Int32.MinValue;
        private Int32 _severity = Int32.MinValue;
        private Int32 _state = Int32.MinValue;
        private Int32 _line = Int32.MinValue;
        private String _mensagemErro = String.Empty;
        private Int32 _idUsuarioIns = Int32.MinValue;
        private DateTime _dhUsarioIns = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdPreProcessamentoLog
        {
            get { return _idPreProcessamentoLog; }
            set { _idPreProcessamentoLog = value; }
        }

        public DateTime DhErro
        {
            get { return _dhErro; }
            set { _dhErro = value; }
        }

        public String DescricaoErro
        {
            get { return _descricaoErro; }
            set { _descricaoErro = value; }
        }

        public Int32 NumeroErro
        {
            get { return _numeroErro; }
            set { _numeroErro = value; }
        }

        public Int32 Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        public Int32 State
        {
            get { return _state; }
            set { _state = value; }
        }

        public Int32 Line
        {
            get { return _line; }
            set { _line = value; }
        }

        public String MensagemErro
        {
            get { return _mensagemErro; }
            set { _mensagemErro = value; }
        }

        public Int32 IdUsuarioIns
        {
            get { return _idUsuarioIns; }
            set { _idUsuarioIns = value; }
        }

        public DateTime DhUsarioIns
        {
            get { return _dhUsarioIns; }
            set { _dhUsarioIns = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe PreProcessamentoLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public PreProcessamentoLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe PreProcessamentoLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public PreProcessamentoLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe PreProcessamentoLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public PreProcessamentoLog(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        #endregion Construtores

        #region Métodos

        /// Consultar uma Conexao por ID
        /// </summary>
        /// <param name="idConfiguracao">ID da Configuração</param>
        public DataTable Listar(Int32 idPreProcessamento)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe PreProcessamentoLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMPPROC_ID", idPreProcessamento);
                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMPPROCLOG_SEL_LISTAR").Tables[0];


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
            if (dt.Columns.Contains("CMPPROCLERR_DH")) { dt.Columns["CMPPROCLERR_DH"].ColumnName = "DhErro"; }
            if (dt.Columns.Contains("CMPPROCLERR_DS_ERRO")) { dt.Columns["CMPPROCLERR_DS_ERRO"].ColumnName = "DescricaoErro"; }
            if (dt.Columns.Contains("CMPPROCLERR_ERROR_NUMBER")) { dt.Columns["CMPPROCLERR_ERROR_NUMBER"].ColumnName = "NumeroErro"; }
            if (dt.Columns.Contains("CMPPROCLERR_ERROR_SEVERITY")) { dt.Columns["CMPPROCLERR_ERROR_SEVERITY"].ColumnName = "Severity"; }
            if (dt.Columns.Contains("CMPPROCLERR_ERROR_STATE")) { dt.Columns["CMPPROCLERR_ERROR_STATE"].ColumnName = "State"; }
            if (dt.Columns.Contains("CMPPROCLERR_ERROR_LINE")) { dt.Columns["CMPPROCLERR_ERROR_LINE"].ColumnName = "Linha"; }
            if (dt.Columns.Contains("CMPPROCLERR_ERROR_MESSAGE")) { dt.Columns["CMPPROCLERR_ERROR_MESSAGE"].ColumnName = "MensagemErro"; }
        }
        #endregion Métodos
    }
}
