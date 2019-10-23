#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Runtime.Caching;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Mensageria
{
    public class CampanhaLogExecucao : BusinessBase
    {

        #region Atributos
        #endregion Atributos

        #region Propriedades
      
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Configuracao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public CampanhaLogExecucao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Configuracao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public CampanhaLogExecucao()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public CampanhaLogExecucao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public CampanhaLogExecucao(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// 
        /// </summary>
        public void Incluir(Int32 idCampanha, String descricaoLog, DateTime dhLog, Int32 idCanal, Int32 nrLote)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@CMPLOGEX_DS", descricaoLog);
                acessoDadosBase.AddParameter("@CMPLOGEX_DH", dhLog);
                acessoDadosBase.AddParameter("@CMPLOGEX_NR_LOTE", nrLote);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "PRCMPLOGEX_INS");

                // Renomear Colunas
                RenomearColunas(ref dt);
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
            if (dt.Columns.Contains("CMP_ID")) { dt.Columns["CMP_ID"].ColumnName = "idCampanha"; }
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "idCanal"; }
            if (dt.Columns.Contains("CMPLOGEX_DS")) { dt.Columns["CMPLOGEX_DS"].ColumnName = "DescricaoLog"; }
            if (dt.Columns.Contains("CMPLOGEX_DH")) { dt.Columns["CMPLOGEX_DH"].ColumnName = "DataHoraLog"; }
            if (dt.Columns.Contains("CMPLOGEX_NR_LOTE")) { dt.Columns["CMPLOGEX_NR_LOTE"].ColumnName = "NumeroLote"; }
        }
        #endregion Métodos
    }
}
