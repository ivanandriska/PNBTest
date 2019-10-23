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
namespace CaseBusiness.CC.Comunicacao
{
    public class Canal : BusinessBase
    {       
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_Canal_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idCanal = Int32.MinValue;
        private String _nomeCanal = String.Empty;
        #endregion Atributos

        #region Propriedades
        public Int32 IdCanal
        {
            get { return _idCanal; }
            set { _idCanal = value; }
        }

        public String NomeCanal
        {
            get { return _nomeCanal; }
            set { _nomeCanal = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Canal - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Canal(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Canal
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Canal(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Canal utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Canal(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Canal e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idCanal">Código da Organização</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        //public Canal(Int32 idCanal, Int32 idUsuarioManutencao)
        //    : this(idUsuarioManutencao)
        //{
        //    Consultar(idCanal);
        //}
        #endregion Construtores        

        #region Métodos
        /// <summary>
        /// Consulta uma Canal
        /// </summary>
        /// <param name="idCanal">Código da Canal</param>
        //private void Consultar(Int32 idCanal)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Listar as Canais
        /// </summary>
        public DataTable Listar()
        {
            DataTable dtOriginal = null;
            DataTable dtDeepCopy = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Canal está operando em Modo Entidade Only"); }

                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dtOriginal = MemoryCache.Default[kCacheKey] as DataTable;
                }
                else
                {
                    dtOriginal = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMCNL_SEL_LISTAR").Tables[0];

                    // Renomear Colunas
                    RenomearColunas(ref dtOriginal);

                    MemoryCache.Default.Set(kCacheKey,
                                            dtOriginal,
                                            new CacheItemPolicy()
                                            {
                                                AbsoluteExpiration = DateTime.Now.AddMinutes(kCache_ABSOLUTEEXPIRATION_MINUTES)
                                            });
                }

                //=========================================================
                //  DEEP COPY
                //=========================================================
                dtDeepCopy = dtOriginal.Clone();
                foreach (DataRow dr in dtOriginal.Rows)
                {
                    dtDeepCopy.ImportRow(dr);
                }
                //=========================================================
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dtDeepCopy;
        }

        /// <summary>
        /// Busca a Canal por Org
        /// </summary>
        /// <param name="codigoOrganizacao"></param>
        /// <returns></returns>
        public DataTable BuscarPorOrg(Int32 codigoOrganizacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Canal está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCOMCNL_SEL_BUSCAR_POR_ORG").Tables[0];

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
        /// Remove o Cache da Canal
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "IdCanal"; }
            if (dt.Columns.Contains("COMCNL_NM")) { dt.Columns["COMCNL_NM"].ColumnName = "NomeCanal"; }

            //if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            //if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }
        #endregion Métodos
    }

    #region Enums e Constantes
    public enum CanalEnum : int
    {
        SMS = 1,
        EMAIL = 2,
        PUSH = 3,
        PDV = 4
    }

    #endregion Enums e Constantes
}
