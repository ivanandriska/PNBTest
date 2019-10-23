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
    public class StatusMensagem : BusinessBase
    {
        #region Atributos
        private String _codigoStatusMensagem = String.Empty;
        private String _descricaoStatusMensagem = String.Empty;
        #endregion Atributos

        #region Propriedades
        public String CodigoStatusMensagem
        {
            get { return _codigoStatusMensagem; }
            set { _codigoStatusMensagem = value; }
        }

        public String DescricaoStatusMensagem
        {
            get { return _descricaoStatusMensagem; }
            set { _descricaoStatusMensagem = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe StatusMensagem - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public StatusMensagem(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe StatusMensagem
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public StatusMensagem(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe StatusMensagem utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public StatusMensagem(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe StatusMensagem e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoGrupo">Código do StatusMensagem</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public StatusMensagem(String codigoStatusMensagem,
                         Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(codigoStatusMensagem);
        }
        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Listar StatusMensagem
        /// </summary>
        /// <param name="codigoConfiguracaoMensagem">Codigo ConfiguracaoMensagem</param>
        public DataTable Listar()
        {
            DataTable dt;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe StatusMensagem está operando em Modo Entidade Only"); }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGMSGST_SEL_LISTAR").Tables[0];

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
        /// Consultar um StatusMensagem
        /// </summary>
        /// <param name="statusMensagem">StatusMensagem</param>
        private void Consultar(String codigoStatusMensagem)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe StatusMensagem está operando em Modo Entidade Only"); }

                DataTable dt;

                acessoDadosBase.AddParameter("@MGMSGST_ST", codigoStatusMensagem);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGMSGST_SEL_CONSULTAR").Tables[0];

                //// Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    CodigoStatusMensagem = dt.Rows[0]["MGMSGST_ST"].ToString();
                    DescricaoStatusMensagem = dt.Rows[0]["MGMSGST_DS"].ToString();
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
            if (dt.Columns.Contains("MGMSGST_ST")) { dt.Columns["MGMSGST_ST"].ColumnName = "CodigoStatusMensagem"; }
            if (dt.Columns.Contains("MGMSGST_DS")) { dt.Columns["MGMSGST_DS"].ColumnName = "DescricaoStatusMensagem"; }
        }
        #endregion Métodos
    }
}
