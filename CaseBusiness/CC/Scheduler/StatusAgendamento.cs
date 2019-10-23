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
    public class StatusAgendamento : BusinessBase
    {
        #region Atributos
        private String _codigoStatusAgendamento = "";
        private String _descricaoStatusAgendamento = "";
        #endregion Atributos


        #region Propriedades
        public String CodigoStatusAgendamento
        {
            get { return _codigoStatusAgendamento; }
            set { _codigoStatusAgendamento = value; }
        }

        public String DescricaoStatusAgendamento
        {
            get { return _descricaoStatusAgendamento; }
            set { _descricaoStatusAgendamento = value; }
        }
        #endregion Propriedades


        #region Construtores
        /// <summary>
        /// Construtor classe StatusAgendamento - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public StatusAgendamento(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe StatusAgendamento
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public StatusAgendamento(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe StatusAgendamento utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public StatusAgendamento(Int32 idUsuarioManutencao,
                                 CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe StatusAgendamento e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoGrupo">Código do StatusAgendamento</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public StatusAgendamento(Int32 codigoStatusAgendamento,
                                 Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(codigoStatusAgendamento);
        }

        #endregion Construtores


        #region Métodos

        /// <summary>
        /// Listar StatusAgendamento
        /// </summary>
        public DataTable Listar()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe StatusAgendamento está operando em Modo Entidade Only"); }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prSTAGPROC_SEL_LISTAR").Tables[0];

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
        /// Consultar um StatusAgendamento
        /// </summary>
        /// <param name="codigoRamoAtividade">Codigo StatusAgendamento</param>
        private void Consultar(Int32 codigoStatusAgendamento)
        {
            try
            {
                DataTable dt;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe StatusAgendamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@STAGPROC_ST", codigoStatusAgendamento);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prSTAGPROC_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    CodigoStatusAgendamento = Convert.ToString(dt.Rows[0]["STAGPROC_ST"]);
                    DescricaoStatusAgendamento = Convert.ToString(dt.Rows[0]["STAGPROC_DS"]);
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
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("STAGPROC_ST")) { dt.Columns["STAGPROC_ST"].ColumnName = "CodigoStatusAgendamento"; }
            if (dt.Columns.Contains("STAGPROC_DS")) { dt.Columns["STAGPROC_DS"].ColumnName = "DescricaoStatusAgendamento"; }
        }
        #endregion Métodos
    }
}
