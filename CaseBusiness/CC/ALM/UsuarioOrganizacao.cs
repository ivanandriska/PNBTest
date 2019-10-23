using CaseBusiness.Framework.BancoDados;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Collections;

namespace CaseBusiness.CC.ALM
{
    public class UsuarioOrganizacao : BusinessBase
    {
        //private const String FLAG_TIPO_MANUTENCAO_ADICIONADO = "ADI";
        //private const String FLAG_TIPO_MANUTENCAO_REMOVIDO = "REM";

        #region Atributos
        private Int32 _idUsuario = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private Int32 _idUsuarioAcesso = Int32.MinValue;
        private DateTime _dataAcesso = DateTime.MinValue;
        private DataTable _dtRestricoesExclusao;
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

        public Int32 IdUsuarioAcesso
        {
            get { return _idUsuarioAcesso; }
            set { _idUsuarioAcesso = value; }
        }

        public DateTime DataAcesso
        {
            get { return _dataAcesso; }
            set { _dataAcesso = value; }
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
                    ObterRestricoesExclusao();
                }

                return _dtRestricoesExclusao;
            }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe UsuarioOrganizacao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public UsuarioOrganizacao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe FilaLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public UsuarioOrganizacao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe FilaLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public UsuarioOrganizacao(Int32 idUsuarioManutencao,
                                  CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe UsuarioOrganizacao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public UsuarioOrganizacao(Int32 idUsuarioAcesso,
                                  Int32 codOrganizacao,
                                  Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
            Consultar(idUsuarioAcesso, codOrganizacao);
        }
        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Obtem as Restrições de Exclusão do Qualificador carregado
        /// </summary>
        private void ObterRestricoesExclusao()
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioFila está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", IdUsuario);
                acessoDadosBase.AddParameter("@ORG_CD", CodigoOrganizacao);

                _dtRestricoesExclusao = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                                       "prUSUORG_SEL_RESTRIC_DEL").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref _dtRestricoesExclusao);

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consulta uma Organizacao
        /// </summary>
        /// <param name="idUsuarioManutencao">Código da Organização</param>
        /// <param name="codigoOrganizacao">ID da Fila</param>
        private void Consultar(Int32 idUsuarioManutencao,
                               Int32 codigoOrganizacao)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioOrganizacao está operando em Modo Entidade Only"); }

                // Fill Object
                __blnIsLoaded = false;

                dt = Buscar(idUsuarioManutencao, codigoOrganizacao);


                if (dt.Rows.Count > 0)
                {
                    IdUsuario = Convert.ToInt32(dt.Rows[0]["IdUsuario"]);
                    CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["CodigoOrganizacao"]);
                    IdUsuarioAcesso = Convert.ToInt32(dt.Rows[0]["IdUsuarioAcesso"]);
                    DataAcesso = Convert.ToDateTime(dt.Rows[0]["DataAcesso"]);

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
        /// Inclui uma Usuário com permissão em uma Organização
        /// </summary>
        /// <param name="idUsuario">Id do Usuário com Permissão na Organização</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public void Incluir(Int32 idUsuario,
                            Int32 codigoOrganizacao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioOrganizacao está operando em Modo Entidade Only"); }

                DateTime dtPermissaoAcesso = DateTime.Now;

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@USU_ID_ACESSO", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@USUORG_DH_ACESSO", dtPermissaoAcesso);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prUSUORG_INS");

                CaseBusiness.CC.ALM.UsuarioOrganizacaoLog objUsuarioOrganizacaoLog = new CaseBusiness.CC.ALM.UsuarioOrganizacaoLog(UsuarioManutencao.ID);

                objUsuarioOrganizacaoLog.Incluir(idUsuario, codigoOrganizacao, CaseBusiness.CC.Global.FlagTipoManutencao.enumTipoManutencao.ADICIONADO, dtPermissaoAcesso);

                // Reset MemoryCache WUC
                ResetarMemoryCache();
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Excluir a Permissão de um Usuário em uma Organização
        /// </summary>
        /// <param name="idUsuario">Id do usuário inserido na Organização</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public void Excluir(Int32 idUsuario,
                            Int32 codigoOrganizacao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioOrganizacao está operando em Modo Entidade Only"); }

                CaseBusiness.CC.ALM.UsuarioOrganizacaoLog objUsuarioOrganizacaoLog = new CaseBusiness.CC.ALM.UsuarioOrganizacaoLog(UsuarioManutencao.ID);

                //
                //
                //
                objUsuarioOrganizacaoLog.Incluir(idUsuario, codigoOrganizacao, CaseBusiness.CC.Global.FlagTipoManutencao.enumTipoManutencao.REMOVIDO, DateTime.Now);

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prUSUORG_DEL");

                // Reset MemoryCache WUC
                ResetarMemoryCache();
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar Usuário x Organização
        /// </summary>
        /// <param name="idUsuario">Id do usuário inserido na Organização</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public DataTable Buscar(Int32 idUsuario,
                                Int32 codigoOrganizacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioOrganizacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSUORG_SEL_BUSCAR").Tables[0];

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
        /// Buscar Organizações do Usuário da Instância da classe
        /// </summary>
        public DataTable BuscarOrganizacao()
        {
            return Buscar(UsuarioManutencao.ID, Int32.MinValue);
        }

        /// <summary>
        /// Buscar Usuarios de uma Orhganizacao
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public DataTable BuscarUsuariosdaOrganizacao(Int32 codigoOrganizacao)
        {
            return Buscar(Int32.MinValue, codigoOrganizacao);
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("USU_ID")) { dt.Columns["USU_ID"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioNome"; }

            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "OrganizacaoNome"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "OrganizacaoCodigoNome"; }

            if (dt.Columns.Contains("USU_ID_ACESSO")) { dt.Columns["USU_ID_ACESSO"].ColumnName = "IdUsuarioAcesso"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
            if (dt.Columns.Contains("USUORG_DH_ACESSO")) { dt.Columns["USUORG_DH_ACESSO"].ColumnName = "DataAcesso"; }
        }


        /// <summary>
        /// Remove do MemoryCache, todos os conteúdos utilizados pelo wucddlOrganizacoesdoUsuario de TDOS OS USUÁRIOS
        /// </summary>
        public static void ResetarMemoryCache()
        {
            MemoryCache memoryCache = MemoryCache.Default;
            IDictionaryEnumerator cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)memoryCache).GetEnumerator();

            while (cacheEnumerator.MoveNext())
            {
                if (cacheEnumerator.Key.ToString().EndsWith("_wucddlOrganizacoesdoUsuario_DataTable"))
                {
                    memoryCache.Remove(cacheEnumerator.Key.ToString());
                }
            }
        }
        #endregion Métodos
    }
}
