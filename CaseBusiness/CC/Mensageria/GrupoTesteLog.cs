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
    public class GrupoTesteLog : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_GrupoTesteLog_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idGrupoMonitoriaLog = Int32.MinValue;
        private Int32 _idGrupoMonitoria = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private String _nomeGrupoMonitoria = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private DateTime _dataInclusao = DateTime.MinValue;
        private String _operacao = String.Empty;
        #endregion Atributos

        #region Propriedades
        public Int32 IdGrupoMonitoriaLog
        {
            get { return _idGrupoMonitoriaLog; }
            set { _idGrupoMonitoriaLog = value; }
        }

        public Int32 IdGrupoMonitoria
        {
            get { return _idGrupoMonitoria; }
            set { _idGrupoMonitoria = value; }
        }

        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public String NomeGrupoMonitoria
        {
            get { return _nomeGrupoMonitoria; }
            set { _nomeGrupoMonitoria = value; }
        }

        public Int32 IdUsuarioInclusao
        {
            get { return _idUsuarioInclusao; }
            set { _idUsuarioInclusao = value; }
        }

        public DateTime DataInclusao
        {
            get { return _dataInclusao; }
            set { _dataInclusao = value; }
        }

        public String Operacao
        {
            get { return _operacao; }
            set { _operacao = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe GrupoTesteLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public GrupoTesteLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe GrupoTesteLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public GrupoTesteLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe GrupoTesteLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public GrupoTesteLog(Int32 idUsuarioManutencao,
                                 CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe GrupoTesteLog e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idGrupoMonitoriaLog">Código do Log do GrupoMonitoria</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public GrupoTesteLog(Int32 idGrupoMonitoriaLog,
                                 Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idGrupoMonitoriaLog);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar Log do Grupo de Teste
        /// </summary>
        /// <param name="codigoOrganizacao">Código da ORG</param>
        /// <param name="nomeGrupoMonitoria">Nome do Grupo de Monitoria</param>
        public DataTable Buscar(Int32 codigoOrganizacao,
                                String nomeGrupoMonitoria)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGGRPMONLOG_NM", nomeGrupoMonitoria);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPMONLOG_SEL_BUSCAR").Tables[0];

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
        /// Consultar um Log do  Grupo de Teste
        /// </summary>
        /// <param name="idGrupoMonitoriaLog">ID do Log do Grupo de Monitoria</param>
        private void Consultar(Int32 idGrupoMonitoriaLog)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteLog está operando em Modo Entidade Only"); }

                DataTable dt = null;

                acessoDadosBase.AddParameter("@MGGRPMONLOG_ID", idGrupoMonitoriaLog);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPMONLOG_SEL_CONSULTAID").Tables[0];

                //// Fill Object
                __blnIsLoaded = false;

                // Fill Object
                PreencherAtributos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Incluir Log do Grupo de Teste
        /// </summary>
        /// <param name="idGrupoMonitoria">Id do Grupo de Monitoria</param>
        /// <param name="codigoOrganizacao">Código da ORG</param>
        /// <param name="nomeGrupoMonitoria">Nome do Grupo de Monitoria</param>
        /// <param name="dataInclusao">Data de Inclusão</param>
        /// <param name="operacao">Operação</param>
        public void IncluirGrupoMonitoriaLog(Int32 idGrupoMonitoria,
                                             Int32 codigoOrganizacao,
                                             String nomeGrupoMonitoria,
                                             DateTime dataInclusao,
                                             String operacao)
        {
            Incluir(idGrupoMonitoria, codigoOrganizacao, nomeGrupoMonitoria, dataInclusao, operacao);
        }

        /// <summary>
        /// Incluir Log do Grupo de Monitoria
        /// </summary>
        /// <param name="idGrupoMonitoria">Id do Grupo de Monitoria</param>
        /// <param name="codigoOrganizacao">Código da ORG</param>
        /// <param name="nomeGrupoMonitoria">Nome do Grupo de Monitoria</param>
        /// <param name="dataInclusao">Data de Inclusão</param>
        /// <param name="operacao">Operação</param>
        private void Incluir(Int32 idGrupoMonitoria,
                            Int32 codigoOrganizacao,
                            String nomeGrupoMonitoria,
                            DateTime dataInclusao,
                            String operacao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPMON_ID", idGrupoMonitoria);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGGRPMONLOG_NM", nomeGrupoMonitoria.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGGRPMONLOG_DH_USU_INS", dataInclusao);
                acessoDadosBase.AddParameter("@MGGRPMONLOG_OPERACAO", operacao);

                acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGGRPMONLOG_INS");

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
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                IdGrupoMonitoriaLog = Convert.ToInt32(dt.Rows[0]["MGGRPMONLOG_ID"]);
                IdGrupoMonitoria = Convert.ToInt32(dt.Rows[0]["MGGRPMON_ID"]);
                CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                NomeGrupoMonitoria = dt.Rows[0]["MGGRPMONLOG_NM"].ToString();
                IdUsuarioInclusao = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                DataInclusao = Convert.ToDateTime(dt.Rows[0]["MGGRPMONLOG_DH_USU_INS"]);
                Operacao = dt.Rows[0]["MGGRPMONLOG_OPERACAO"].ToString();

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("MGGRPMONLOG_ID")) { dt.Columns["MGGRPMONLOG_ID"].ColumnName = "IdGrupoMonitoriaLog"; }
            if (dt.Columns.Contains("MGGRPMON_ID")) { dt.Columns["MGGRPMON_ID"].ColumnName = "IdGrupoMonitoria"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("MGGRPMONLOG_NM")) { dt.Columns["MGGRPMONLOG_NM"].ColumnName = "NomeGrupoMonitoria"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            if (dt.Columns.Contains("USU_INC_NM")) { dt.Columns["USU_INC_NM"].ColumnName = "UsuarioInclusao"; }
            if (dt.Columns.Contains("MGGRPMONLOG_DH_USU_INS")) { dt.Columns["MGGRPMONLOG_DH_USU_INS"].ColumnName = "DataInclusao"; }
            if (dt.Columns.Contains("MGGRPMONLOG_OPERACAO")) { dt.Columns["MGGRPMONLOG_OPERACAO"].ColumnName = "Operacao"; }
            if (dt.Columns.Contains("QTD_LOG_DEST")) { dt.Columns["QTD_LOG_DEST"].ColumnName = "TotalLogDestinatario"; }
            if (dt.Columns.Contains("ULT_ID_LOG_GRUPO")) { dt.Columns["ULT_ID_LOG_GRUPO"].ColumnName = "UltimoIdLogGrupoMonitoria"; }
        }
        #endregion Métodos
    }
}