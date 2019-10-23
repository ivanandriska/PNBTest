#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CaseManager
{
    public class CaseManager : BusinessBase
    {

        #region Construtores
        /// <summary>
        /// Construtor classe CaseManager - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public CaseManager(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe CaseManager
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public CaseManager(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Sincroniza as respostas dadas no Case Manager no módulo ALM. Metodo exclusivo para chamada Async
        /// </summary>
        /// <param name="parametros">[0] dtInicio</param>
        public void SincronizarRespostasCase_ALM(Object parametros)
        {
            SincronizarRespostasCase_ALM(Convert.ToDateTime(((Object[])parametros)[0]));
        }


        /// <summary>
        /// Sincroniza as respostas dadas no Case Manager no módulo ALM
        /// </summary>
        /// <param name="dtInicio">A partir desta data/hora que as respostas dadas serão sincronizadas</param>
        public void SincronizarRespostasCase_ALM(DateTime dtInicio)
        {

            try
            {
                if (acessoDadosBase == null)
                    acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);

                if (ModoEntidadeOnly)
                {
                    throw new ApplicationException("Esta Instância da classe CEP está operando em Modo Entidade Only");
                }

                acessoDadosBase.AddParameter("@DT_INICIO", dtInicio);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                    "PR_MARCAR_RESPOSTAS_TRANSACAO_PORTAL");

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        #endregion Métodos
    }
}
