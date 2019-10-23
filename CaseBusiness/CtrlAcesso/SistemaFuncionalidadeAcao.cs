#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CtrlAcesso
{
    public class SistemaFuncionalidadeAcao : BusinessBase
    {
        //#region Enums e Constantes
        //public enum enumStatus { EMPTY, ATIVA, INATIVA }
        //public const string kStatus_ATIVA = "A";
        //public const string kStatus_INATIVA = "I";

        //public enum enumStatusTexto { Ativa, Inativa }
        //public const string kStatus_ATIVA_Texto = "Ativa";
        //public const string kStatus_INATIVA_Texto = "Inativa";
        //#endregion Enums e Constantes


        #region Construtores
        ///// <summary>
        ///// Construtor classe SistemaFuncionalidadeAcao - Modo Entidade Only
        ///// </summary>
        ///// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        ///// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        ///// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        ///// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        //public SistemaFuncionalidadeAcao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        //{
        //    UsuarioManutencao.ID = idUsuarioManutencao;
        //    __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        //}

        /// <summary>
        /// Construtor classe SistemaFuncionalidadeAcao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public SistemaFuncionalidadeAcao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe SistemaFuncionalidadeAcao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public SistemaFuncionalidadeAcao(Int32 idUsuarioManutencao,
                                                CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores


        #region Métodos
        /// <summary>
        /// Buscar Ações
        /// </summary>
        /// <param name="idSistema">ID do Sistema</param>
        /// <param name="idSistemaFuncionalidade">ID da Funcionalidade do Sistema</param>
        /// <param name="codigoSistemaFuncionalidadeAcao">Código da Ação na Funcionalidade do Sistema</param>
        public DataTable Buscar(Int32 idSistema,
                                Int32 idSistemaFuncionalidade,
                                String codigoSistemaFuncionalidadeAcao)
        {
            DataTable dt = null;

            try
            {
                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SISTFCN_ID", idSistemaFuncionalidade);
                acessoDadosBase.AddParameter("@SISTFCNACAO_CD", codigoSistemaFuncionalidadeAcao.Trim().ToUpper());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prSISTFCNACAO_SEL_BUSCAR").Tables[0];

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
            if (dt.Columns.Contains("SIST_ID")) { dt.Columns["SIST_ID"].ColumnName = "IdSistema"; }
            if (dt.Columns.Contains("SIST_NM")) { dt.Columns["SIST_NM"].ColumnName = "SistemaNome"; }

            if (dt.Columns.Contains("SISTFCN_ID")) { dt.Columns["SISTFCN_ID"].ColumnName = "IdSistemaFuncionalidade"; }
            if (dt.Columns.Contains("SISTFCN_MODULO_SG")) { dt.Columns["SISTFCN_MODULO_SG"].ColumnName = "SistemaFuncionalidadeModulo"; }
            if (dt.Columns.Contains("SISTFCN_NM")) { dt.Columns["SISTFCN_NM"].ColumnName = "SistemaFuncionalidadeNome"; }
            if (dt.Columns.Contains("SISTFCN_ST")) { dt.Columns["SISTFCN_ST"].ColumnName = "SistemaFuncionalidadeStatus"; }

            if (dt.Columns.Contains("SISTFCNACAO_CD")) { dt.Columns["SISTFCNACAO_CD"].ColumnName = "CodigoSistemaFuncionalidadeAcao"; }
            if (dt.Columns.Contains("SISTFCNACAO_DS")) { dt.Columns["SISTFCNACAO_DS"].ColumnName = "SistemaFuncionalidadeAcaoDescricao"; }
            if (dt.Columns.Contains("SISTFCNACAO_FG_MENU")) { dt.Columns["SISTFCNACAO_FG_MENU"].ColumnName = "SistemaFuncionalidadeAcaoFlagMenu"; }
            if (dt.Columns.Contains("SISTFCNACAO_ST")) { dt.Columns["SISTFCNACAO_ST"].ColumnName = "SistemaFuncionalidadeAcaoStatus"; }
        }

        #endregion Métodos
    }
}
