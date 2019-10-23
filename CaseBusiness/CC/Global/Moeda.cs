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

namespace CaseBusiness.CC.Global
{
    public class Moeda : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Global_Moeda_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache


        #region Atributos
        private String _siglaMoeda = String.Empty;
        private String _nomeMoeda = String.Empty;
        private DataTable _dtRestricoesExclusao;
        #endregion Atributos


        #region Propriedades
        public String SiglaMoeda
        {
            get { return _siglaMoeda; }
            set { _siglaMoeda = value; }
        }

        public String NomeMoeda
        {
            get { return _nomeMoeda; }
            set { _nomeMoeda = value; }
        }

        public Boolean ExclusaoPermitida
        {
            get
            {
                if (!IsLoaded)
                {
                    _dtRestricoesExclusao = null;
                    return false;
                }

                if (RestricoesExclusao == null)
                { return false; }
                else
                {
                    if (_dtRestricoesExclusao.Rows.Count <= 0)
                    { return true; }
                    else
                    { return false; }
                }
            }
        }

        public DataTable RestricoesExclusao
        {
            get
            {
                if (_dtRestricoesExclusao == null)
                {
                    _dtRestricoesExclusao = new DataTable();
                    _dtRestricoesExclusao = ObterRestricoesExclusao();
                }

                return _dtRestricoesExclusao;
            }
        }
        #endregion Propriedades


        #region Construtores
        /// <summary>
        /// Construtor classe Moeda - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Moeda(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Moeda
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Moeda(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Moeda utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Moeda(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Moeda e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="siglaGrupo">Código do Moeda</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Moeda(String siglaMoeda,
                     Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(siglaMoeda);
        }

        #endregion Construtores


        #region Métodos
        /// <summary>
        /// Listar as Moedas
        /// </summary>
        public DataTable Listar()
        {
            DataTable dtOriginal = null;
            DataTable dtDeepCopy = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Moeda está operando em Modo Entidade Only"); }

                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dtOriginal = MemoryCache.Default[kCacheKey] as DataTable;
                }
                else
                {
                    dtOriginal = Buscar(String.Empty, String.Empty);

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
        /// Buscar Moeda
        /// </summary>
        /// <param name="siglaMoeda">Sigla Moeda</param>
        /// <param name="nomeMoeda">Nome Moeda</param>
        public DataTable Buscar(String siglaMoeda,
                                String nomeMoeda)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Moeda está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MOE_SG", siglaMoeda.Trim());
                acessoDadosBase.AddParameter("@MOE_NM", nomeMoeda.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMOE_SEL_BUSCAR").Tables[0];

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
        /// Consultar um Moeda
        /// </summary>
        /// <param name="siglaMoeda">Sigla Moeda</param>
        public void Consultar(String siglaMoeda)
        {
            try
            {
                //    DataTable dt;
                DataView dv = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organizacao está operando em Modo Entidade Only"); }

                // Fill Object
                __blnIsLoaded = false;

                dv = Listar().DefaultView;
                dv.RowFilter = "SiglaMoeda = '" + siglaMoeda + "'";

                //    acessoDadosBase.AddParameter("@MOE_SG", siglaMoeda);

                //    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                //                                        "prMOE_SEL_CONSULTAR").Tables[0];

                if (dv.Count > 0)
                {
                    SiglaMoeda = Convert.ToString(dv[0]["SiglaMoeda"]);
                    NomeMoeda = Convert.ToString(dv[0]["NomeMoeda"]);

                    __blnIsLoaded = true;
                }

                dv = null;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Inclui um Moeda
        /// </summary>
        /// <param name="siglaMoeda">Código do Moeda</param>
        /// <param name="nomeMoeda">Nome do Moeda</param>
        public void Incluir(String siglaMoeda,
                            String nomeMoeda)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Moeda está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MOE_SG", siglaMoeda.Trim());
                acessoDadosBase.AddParameter("@MOE_NM", nomeMoeda.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMOE_INS");

                // MemoryCache Clean
                RemoverCache();

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Alterar um Moeda
        /// </summary>
        /// <param name="siglaMoeda">Código do Moeda</param>
        /// <param name="nomeMoeda">Nome do Moeda</param>
        public void Alterar(String siglaMoeda,
                            String nomeMoeda)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Moeda está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MOE_SG", siglaMoeda.Trim());
                acessoDadosBase.AddParameter("@MOE_NM", nomeMoeda.Trim());
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMOE_UPD");

                // MemoryCache Clean
                RemoverCache();

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Exclui uma Moeda 
        /// </summary>
        /// <param name="siglaMoeda">Código da Moeda</param>
        public void Excluir(String siglaMoeda)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Moeda está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MOE_SG", siglaMoeda);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMOE_DEL");

                // MemoryCache Clean
                RemoverCache();

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Remove do Cache as Moedas
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }


        /// <summary>
        /// Obtem as Restrições de Exclusão da Moeda carregada
        /// </summary>
        private DataTable ObterRestricoesExclusao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Moeda está operando em Modo Entidade Only"); }

                //acessoDadosBase.AddParameter("@MOE_SG", SiglaMoeda);

                dt = new DataTable();
                //dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                //"prMOE_SEL_RESTRIC_DEL").Tables[0];

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
            if (dt.Columns.Contains("MOE_SG")) { dt.Columns["MOE_SG"].ColumnName = "SiglaMoeda"; }
            if (dt.Columns.Contains("MOE_NM")) { dt.Columns["MOE_NM"].ColumnName = "NomeMoeda"; }

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }
        #endregion Métodos
    }
}
