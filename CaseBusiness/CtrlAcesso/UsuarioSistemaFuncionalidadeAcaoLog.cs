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
    public class UsuarioSistemaFuncionalidadeAcaoLog : BusinessBase
    {
        #region Enums e Constantes
        public enum enumPermissaoOperacao { EMPTY, ADICAO, REMOCAO }
        public const string kPermissaoOperacao_ADICAO = "ADD";
        public const string kPermissaoOperacao_REMOCAO= "REM";

        public enum enumPermissaoOperacaoTexto { Adicao, Remocao}
        public const string kPermissaoOperacao_ADICAO_Texto = "Adição";
        public const string kPermissaoOperacao_REMOCAO_Texto = "Remoção";
        #endregion Enums e Constantes


        #region Construtores
        /// <summary>
        /// Construtor classe UsuarioSistemaFuncionalidadeAcaoLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public UsuarioSistemaFuncionalidadeAcaoLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe UsuarioSistemaFuncionalidadeAcaoLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public UsuarioSistemaFuncionalidadeAcaoLog(Int32 idUsuarioManutencao,
                                                   CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores


        #region Métodos
        /// <summary>
        /// Buscar Log de Alterações em Permissões de Usuários
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="idSistema">ID do Sistema</param>
        /// <param name="idSistemaFuncionalidade">ID da Funcionalidade do Sistema</param>
        /// <param name="codigoSistemaFuncionalidadeAcao">Código da Ação na Funcionalidade do Sistema</param>
        /// <param name="enmPermissaoOperacao">Operação: Adição ou Remoção de Permissão de Acesso</param>
        /// <param name="idUsuarioPermissao">ID do Usuário que efetivou a Operação de Permissão</param>
        /// <param name="dataOperacao_Inicio">Data da Efetivação a Operação de Permissão - Início</param>
        /// <param name="dataOperacao_Termino">Data da Efetivação a Operação de Permissão - Término</param>
        ///<param name="enmStatusUsuario">Status do Usuário</param>
        public DataTable Buscar(Int32 idUsuario,
                                Int32 idSistema,
                                Int32 idSistemaFuncionalidade,
                                String codigoSistemaFuncionalidadeAcao,
                                enumPermissaoOperacao enmPermissaoOperacao,
                                Int32 idUsuarioPermissao,
                                DateTime dataOperacao_Inicio,
                                DateTime dataOperacao_Termino,
                                CaseBusiness.CtrlAcesso.UsuarioStatus.enumStatus enmStatusUsuario)
        {
            DataTable dt = null;

            try
            {
                String _operacao = String.Empty;
                String _statusUsuario = String.Empty;

                switch (enmPermissaoOperacao)
                {
                    case enumPermissaoOperacao.EMPTY: _operacao = String.Empty; break;
                    case enumPermissaoOperacao.ADICAO: _operacao = kPermissaoOperacao_ADICAO; break;
                    case enumPermissaoOperacao.REMOCAO: _operacao = kPermissaoOperacao_REMOCAO; break;
                }

                _statusUsuario = UsuarioStatus.ObterDBValue(enmStatusUsuario);
                //switch(enmStatusUsuario)
                //{
                //    case UsuarioStatusLog.enumStatus.EMPTY: _statusUsuario = String.Empty; break;
                //    case UsuarioStatusLog.enumStatus.ATIVO: _statusUsuario = CaseBusiness.CtrlAcesso.UsuarioStatusLog.kStatus_ATIVO_DBValue; break;
                //    case UsuarioStatusLog.enumStatus.INATIVO: _statusUsuario = CaseBusiness.CtrlAcesso.UsuarioStatusLog.kStatus_INATIVO_DBValue; break;
                //}

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SISTFCN_ID", idSistemaFuncionalidade);
                acessoDadosBase.AddParameter("@SISTFCNACAO_CD", codigoSistemaFuncionalidadeAcao.Trim().ToUpper());
                acessoDadosBase.AddParameter("@USUACAOLOG_OPER_CD", _operacao);
                acessoDadosBase.AddParameter("@USU_ID_OPER", idUsuarioPermissao);
                acessoDadosBase.AddParameter("@USUACAOLOG_DH_INI",dataOperacao_Inicio );
                acessoDadosBase.AddParameter("@USUACAOLOG_DH_TER", dataOperacao_Termino);
                acessoDadosBase.AddParameter("@USU_ST", _statusUsuario);

                // Habilita Usuarios ID Negativos 2RP.Net se o Logado for um deles
                if (this.UsuarioManutencao.ID < 0)
                {
                    acessoDadosBase.AddParameter("@FLAG_MOSTRA_USU_ID_NEGATIVO", "S");
                }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSUACAOLOG_SEL_BUSCAR").Tables[0];

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
            if (dt.Columns.Contains("USU_ID")) { dt.Columns["USU_ID"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioNome"; }
            if (dt.Columns.Contains("USU_CD_LOGIN")) { dt.Columns["USU_CD_LOGIN"].ColumnName = "UsuarioLogin"; }
            if (dt.Columns.Contains("USU_ST")) { dt.Columns["USU_ST"].ColumnName = "UsuarioStatus"; }

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

            if (dt.Columns.Contains("USUACAOLOG_DH")) { dt.Columns["USUACAOLOG_DH"].ColumnName = "PermissaoDataOperacao"; }
            if (dt.Columns.Contains("USUACAOLOG_OPER_CD")) { dt.Columns["USUACAOLOG_OPER_CD"].ColumnName = "PermissaoCodigoOperacao"; }
            if (dt.Columns.Contains("USUACAOLOG_OPER_CD_TEXTO")) { dt.Columns["USUACAOLOG_OPER_CD_TEXTO"].ColumnName = "PermissaoCodigoOperacaoTexto"; }

            if (dt.Columns.Contains("USU_ID_OPER")) { dt.Columns["USU_ID_OPER"].ColumnName = "PermissaoIdUsuario"; }
            if (dt.Columns.Contains("USU_NM_OPER")) { dt.Columns["USU_NM_OPER"].ColumnName = "PermissaoUsuarioNome"; }
        }

        #endregion Métodos
    }
}
