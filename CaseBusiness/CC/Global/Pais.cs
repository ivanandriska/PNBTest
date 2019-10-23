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
    public class Pais : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Global_Pais_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        
        #region Atributos
        private String _siglaPais = String.Empty;
        private String _nomePais = String.Empty;
        private DataTable _dtRestricoesExclusao;
        #endregion Atributos


        #region Propriedades
        public String SiglaPais
        {
            get { return _siglaPais; }
            set { _siglaPais = value; }
        }

        public String NomePais
        {
            get { return _nomePais; }
            set { _nomePais = value; }
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
        /// Construtor classe Pais - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Pais(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Pais
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Pais(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Pais utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Pais(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Pais e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="siglaGrupo">Código do Pais</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Pais(String siglaPais,
                     Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(siglaPais);
        }

        #endregion Construtores


        #region Métodos
        /// <summary>
        /// Listar os Paises
        /// </summary>
        public DataTable Listar()
        {
            DataTable dtOriginal = null;
            DataTable dtDeepCopy = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Pais está operando em Modo Entidade Only"); }

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
        /// Buscar Pais
        /// </summary>
        /// <param name="siglaPais">Sigla Pais</param>
        /// <param name="nomePais">Nome Pais</param>
        public DataTable Buscar(String siglaPais,
                                String nomePais)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Pais está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PAIS_SG", siglaPais.Trim());
                acessoDadosBase.AddParameter("@PAIS_NM", nomePais.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prPAIS_SEL_BUSCAR").Tables[0];

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
        /// Consultar um Pais
        /// </summary>
        /// <param name="siglaPais">Sigla Pais</param>
        public  void Consultar(String siglaPais)
        {
            try
            {
                //    DataTable dt;
                DataView dv = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Pais está operando em Modo Entidade Only"); }

                // Fill Object
                __blnIsLoaded = false;
                
                dv = Listar().DefaultView;
                dv.RowFilter = "SiglaPais = '" + siglaPais + "'";

                //acessoDadosBase.AddParameter("@PAIS_SG", siglaPais);

                //dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                //                                    "prPAIS_SEL_CONSULTAR").Tables[0];

                
                if (dv.Count > 0)
                {
                    SiglaPais = Convert.ToString(dv[0]["SiglaPais"]);
                    NomePais = Convert.ToString(dv[0]["NomePais"]);

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
        /// Inclui um Pais
        /// </summary>
        /// <param name="siglaPais">Código do Pais</param>
        /// <param name="nomePais">Nome do Pais</param>
        public void Incluir(String siglaPais,
                            String nomePais)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Pais está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PAIS_SG", siglaPais.Trim());
                acessoDadosBase.AddParameter("@PAIS_NM", nomePais.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prPAIS_INS");

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
        /// Alterar um Pais
        /// </summary>
        /// <param name="siglaPais">Código do Pais</param>
        /// <param name="nomePais">Nome do Pais</param>
        public void Alterar(String siglaPais,
                            String nomePais)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Pais está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PAIS_SG", siglaPais.Trim());
                acessoDadosBase.AddParameter("@PAIS_NM", nomePais.Trim());
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prPAIS_UPD");

                // MemoryCache Clean
                RemoverCache();

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        ///// <summary>
        ///// Exclui uma Pais 
        ///// </summary>
        ///// <param name="siglaPais">Código da Pais</param>
        public void Excluir(String siglaPais)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Pais está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@PAIS_SG", siglaPais);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prPAIS_DEL");

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
        /// Obtem as Restrições de Exclusão da Pais carregada
        /// </summary>
        private DataTable ObterRestricoesExclusao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Pais está operando em Modo Entidade Only"); }

                //acessoDadosBase.AddParameter("@PAIS_SG", SiglaPais);

                dt = new DataTable();
                //dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                //"prPAIS_SEL_RESTRIC_DEL").Tables[0];

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
            if (dt.Columns.Contains("PAIS_SG")) { dt.Columns["PAIS_SG"].ColumnName = "SiglaPais"; }
            if (dt.Columns.Contains("PAIS_NM")) { dt.Columns["PAIS_NM"].ColumnName = "NomePais"; }

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }
        #endregion Métodos
    }
}
