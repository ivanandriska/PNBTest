using CaseBusiness.Framework.BancoDados;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CaseBusiness.CC.ALM
{
    public class UsuarioOrganizacaoLog : BusinessBase
    {
        #region Atributos
        private Int32 _idUsuario = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private CaseBusiness.CC.Global.FlagTipoManutencao.enumTipoManutencao _flagTipoManutencao = CaseBusiness.CC.Global.FlagTipoManutencao.enumTipoManutencao.EMPTY;
        private Int32 _idUsuarioInsert = Int32.MinValue;
        private DateTime _usuarioInsert_usuarioOrganizacaoLogDH;
        #endregion Atributos

        #region Propriedades
        public Int32 IdUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }
        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }
        public CaseBusiness.CC.Global.FlagTipoManutencao.enumTipoManutencao FlagTipoManutencao
        {
            get { return _flagTipoManutencao; }
            set { _flagTipoManutencao = value; }
        }
        public Int32 IdUsuarioInsert
        {
            get { return _idUsuarioInsert; }
            set { _idUsuarioInsert = value; }
        }
        public DateTime UsuarioInsert_usuarioOrganizacaoLogDH
        {
            get { return _usuarioInsert_usuarioOrganizacaoLogDH; }
            set { _usuarioInsert_usuarioOrganizacaoLogDH = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe UsuarioOrganizacaoLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public UsuarioOrganizacaoLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe UsuarioOrganizacaoLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public UsuarioOrganizacaoLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe UsuarioOrganizacaoLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public UsuarioOrganizacaoLog(Int32 idUsuarioManutencao,
                                     CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores

        #region Métodos


        /// <summary>
        /// Mantém o registro de Usuário x Fila
        /// </summary>
        /// <param name="idUsu">ID do Usuário</param>
        /// <param name="idUsuFilaLog">ID de log do usuário na fila </param>
        /// <param name="cdOrg">Código da Organização</param>
        /// <param name="cdOperUsuOrganizacaoLog">??????????????????</param>
        /// <param name="idUsuIns">ID do usuário de inserção</param>
        public void Incluir(Int32 idUsu,
                            Int32 cdOrg,
                            CaseBusiness.CC.Global.FlagTipoManutencao.enumTipoManutencao cdOperUsuOrganizacaoLog,
                            DateTime dataHoraAcesso)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioOrganizacaoLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsu);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrg);
                acessoDadosBase.AddParameter("@USUORGLOG_OPER_CD", CaseBusiness.CC.Global.FlagTipoManutencao.ObterDBValue(cdOperUsuOrganizacaoLog));
                acessoDadosBase.AddParameter("@USU_ID_ACESSO", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@USUORGLOG_DH_ACESSO", dataHoraAcesso);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prUSUORGLOG_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar Usuário x Filas
        /// </summary>
        /// <param name="idUsu">ID do usuário</param>
        public DataTable Buscar(Int32 idUsu,
                                Int32 cdOrg,
                                CaseBusiness.CC.Global.FlagTipoManutencao.enumTipoManutencao cdOperUsuOrganizacaoLog,
                                DateTime dataHoraAcessoInicio,
                                DateTime dataHoraAcessoTermino,
                                Int32 idUsuarioAcesso)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioOrganizacaoLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsu);
                acessoDadosBase.AddParameter("@ORG_CD", cdOrg);
                acessoDadosBase.AddParameter("@USUORGLOG_OPER_CD", CaseBusiness.CC.Global.FlagTipoManutencao.ObterDBValue(cdOperUsuOrganizacaoLog));
                acessoDadosBase.AddParameter("@USU_ID_ACESSO", idUsuarioAcesso);
                acessoDadosBase.AddParameter("@USUORGLOG_DH_ACESSO_INI", dataHoraAcessoInicio);
                acessoDadosBase.AddParameter("@USUORGLOG_DH_ACESSO_TER", dataHoraAcessoTermino);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSUORGLOG_SEL_BUSCAR").Tables[0];

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

            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "DescricaoOrganizacao"; }

            if (dt.Columns.Contains("USUORGLOG_OPER_CD")) { dt.Columns["USUORGLOG_OPER_CD"].ColumnName = "CodigoTipoManutencao"; }
            if (dt.Columns.Contains("USU_NM_ACESSO")) { dt.Columns["USU_NM_ACESSO"].ColumnName = "UsuarioNomeAcesso"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioNome"; }
            if (dt.Columns.Contains("USUORGLOG_DH_ACESSO")) { dt.Columns["USUORGLOG_DH_ACESSO"].ColumnName = "DataAcesso"; }
        }
        #endregion Métodos
    }
}
