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
    public class Fornecedora : BusinessBase
    {
       #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_Fornecedora_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idFornecedora = Int32.MinValue;
        private String _cdFornecedora = String.Empty;
        private String _nomeFornecedora = String.Empty;
        private Int32 _idCanal = Int32.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdFornecedora
        {
            get { return _idFornecedora; }
            set { _idFornecedora = value; }
        }

        public String CdFornecedora
        {
            get { return _cdFornecedora; }
            set { _cdFornecedora = value; }
        }

        public String NomeFornecedora
        {
            get { return _nomeFornecedora; }
            set { _nomeFornecedora = value; }
        }

        public Int32 IdCanal
        {
            get { return _idCanal; }
            set { _idCanal = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Fornecedora - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Fornecedora(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Fornecedora
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Fornecedora(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Fornecedora utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Fornecedora(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Fornecedora e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Fornecedora(Int32 idFornecedora, Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idFornecedora);
        }

        /// <summary>
        /// Construtor classe Fornecedora e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="cdFornecedora">Cd da Fornecedora</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Fornecedora(String cdFornecedora, Int32 idUsuarioManutencao) 
            : this(idUsuarioManutencao)
        {
            Consultar(cdFornecedora);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Consulta uma Fornecedora por ID
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        private void Consultar(Int32 idFornecedora)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Fornecedora está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                
                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORN_SEL_CONSULTARID").Tables[0];

                // Fill objetct
                PreencherAtributos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consulta uma Fornecedora por CD
        /// </summary>
        /// <param name="cdFornecedora">CD da Fornecedora</param>
        private void Consultar(String cdFornecedora)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Fornecedora está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMFORN_CD", cdFornecedora);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORN_SEL_CONSULTARCD").Tables[0];

                // Fill objetct
                PreencherAtributos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Listar as Fornecedoras
        /// </summary>
        public DataTable Listar()
        {
            DataTable dtOriginal = null;
            DataTable dtDeepCopy = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Fornecedora está operando em Modo Entidade Only"); }

                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dtOriginal = MemoryCache.Default[kCacheKey] as DataTable;
                }
                else
                {
                    dtOriginal = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORN_SEL_LISTAR").Tables[0];

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
        /// Remove do Cache as Organizações
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }

        /// <summary>
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                IdFornecedora = Convert.ToInt32(dt.Rows[0]["COMFORN_ID"]);
                CdFornecedora = (String)dt.Rows[0]["COMFORN_CD"];
                NomeFornecedora = (String)dt.Rows[0]["COMFORN_NM"];
                IdCanal = Convert.ToInt32(dt.Rows[0]["COMCNL_ID"]);

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("COMFORN_ID")) { dt.Columns["COMFORN_ID"].ColumnName = "IdFornecedora"; }
            if (dt.Columns.Contains("COMFORN_CD")) { dt.Columns["COMFORN_CD"].ColumnName = "CdFornecedora"; }
            if (dt.Columns.Contains("COMFORN_NM")) { dt.Columns["COMFORN_NM"].ColumnName = "NomeFornecedora"; }
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "IdCanal"; }            

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }
        #endregion Métodos
    }
}