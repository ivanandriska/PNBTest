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
    public class GrupoTesteDestinatarioLog : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_GrupoTesteDestinatarioLog_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idGrupoMonitoriaDestinatarioLog = Int32.MinValue;
        private Int32 _idGrupoMonitoriaDestinatario = Int32.MinValue;
        private Int32 _idGrupoMonitoria = Int32.MinValue;
        private String _nomeDestinatario = String.Empty;
        private String _destinatario = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private DateTime _dataInclusao = DateTime.MinValue;
        private String _operacao = String.Empty;
        #endregion Atributos

        #region Propriedades
        public Int32 IdGrupoMonitoriaDestinatarioLog
        {
            get { return _idGrupoMonitoriaDestinatarioLog; }
            set { _idGrupoMonitoriaDestinatarioLog = value; }
        }

        public Int32 IdGrupoMonitoriaDestinatario
        {
            get { return _idGrupoMonitoriaDestinatario; }
            set { _idGrupoMonitoriaDestinatario = value; }
        }

        public Int32 IdGrupoMonitoria
        {
            get { return _idGrupoMonitoria; }
            set { _idGrupoMonitoria = value; }
        }

        public String NomeDestinatario
        {
            get { return _nomeDestinatario; }
            set { _nomeDestinatario = value; }
        }

        public String Destinatario
        {
            get { return _destinatario; }
            set { _destinatario = value; }
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
        /// Construtor classe GrupoTesteDestinatarioLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public GrupoTesteDestinatarioLog(Int32 idUsuarioManutencao,
                                             Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe GrupoTesteDestinatarioLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public GrupoTesteDestinatarioLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe GrupoTesteDestinatarioLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public GrupoTesteDestinatarioLog(Int32 idUsuarioManutencao,
                                             CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe GrupoTesteDestinatarioLog e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idGrupoMonitoriaDestinatarioLog">Id do Log Destinatário do Grupo de Monitoria</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public GrupoTesteDestinatarioLog(Int32 idGrupoMonitoriaDestinatarioLog,
                                             Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idGrupoMonitoriaDestinatarioLog);
        }
        #endregion Construtores

        #region Metodos
        /// <summary>
        /// Incluir Log do Destinatário do Grupo de Teste 
        /// </summary>
        /// <param name="idGrupoMonitoriaDestinatario">Id do Destinatário do Grupo de Monitoria</param>
        /// <param name="idGrupoMonitoria">Id do Grupo de Monitoria</param>
        /// <param name="nomeDestinatario">Nome do Destinatário</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="dataInclusao">Data Inclusão</param>
        /// <param name="operacao">Operação</param>
        public void IncluirGrupoMonitoriaDestinatario(Int32 idGrupoMonitoriaDestinatario,
                                                      Int32 idGrupoMonitoria,
                                                      String nomeDestinatario,
                                                      String destinatario,
                                                      DateTime dataInclusao,
                                                      String operacao)
        {
            Incluir(idGrupoMonitoriaDestinatario, idGrupoMonitoria, nomeDestinatario, destinatario, dataInclusao, operacao);
        }

        /// <summary>
        /// Incluir Log do Destinatário do Grupo de Teste 
        /// </summary>
        /// <param name="idGrupoMonitoriaDestinatario">Id do Destinatário do Grupo de Monitoria</param>
        /// <param name="idGrupoMonitoria">Id do Grupo de Monitoria</param>
        /// <param name="nomeDestinatario">Nome do Destinatário</param>
        /// <param name="destinatario">Destinatário</param>
        /// <param name="dataInclusao">Data Inclusão</param>
        /// <param name="operacao">Operação</param>
        private void Incluir(Int32 idGrupoMonitoriaDestinatario,
                             Int32 idGrupoMonitoria,
                             String nomeDestinatario,
                             String destinatario,
                             DateTime dataInclusao,
                             String operacao)
        {
            try
            {
                // Obs.: No momento o nomeDestinatario esta recebendo somente o ddd e número de celular, quando for outro tipo fazer o devido tratamento

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteDestinatarioLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPMONDEST_ID", idGrupoMonitoriaDestinatario);
                acessoDadosBase.AddParameter("@MGGRPMON_ID", idGrupoMonitoria);
                acessoDadosBase.AddParameter("@MGGRPMONDESTLOG_NM", Util.RemoveFormat(nomeDestinatario.Trim()));
                acessoDadosBase.AddParameter("@MGGRPMONDESTLOG_DS", destinatario);
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGGRPMONDESTLOG_DH_USU_INS", dataInclusao);
                acessoDadosBase.AddParameter("@MGGRPMONDESTLOG_OPERACAO", operacao);

                acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGGRPMONDESTLOG_INS");

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

        public DataTable ListarDestinatarios(Int32 idGrupoMonitoria)
        {
            DataTable dt = null;

            //    try
            //    {
            //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteDestinatarioLog está operando em Modo Entidade Only"); }

            //        //acessoDadosBase.AddParameter("@GRP_CD", codigoGrupo);
            //        //acessoDadosBase.AddParameter("@REGRHEAD_CD", codigoRegra);


            //        dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
            //                                            "").Tables[0];

            //        // Renomear Colunas
            //        //RenomearColunas(ref dt);
            //    }
            //    catch (Exception ex)
            //    {
            //        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            //        throw;
            //    }

            return dt;
        }

        /// <summary>
        /// Buscar Todos os Destinatário do Grupo de Teste
        /// </summary>
        /// <param name="idGrupoMonitoria">Id Grupo de Monitoria</param>
        public DataTable Buscar(Int32 idGrupoMonitoria)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteDestinatarioLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPMON_ID", idGrupoMonitoria);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPMONDESTLOG_SEL_BUSCAR").Tables[0];

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
        /// Consultar uma Destinatário por ID
        /// </summary>
        /// <param name="idGrupoMonitoriaDestinatarioLog">Id do Log do Destinatário do Grupo de Monitoria</param>
        private void Consultar(Int32 idGrupoMonitoriaDestinatarioLog)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteDestinatarioLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPMONDESTLOG_ID", idGrupoMonitoriaDestinatarioLog);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPMONDESTLOG_SEL_CON_ID").Tables[0];

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
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                IdGrupoMonitoriaDestinatarioLog = Convert.ToInt32(dt.Rows[0]["MGGRPMONDESTLOG_ID"]);
                IdGrupoMonitoriaDestinatario = Convert.ToInt32(dt.Rows[0]["MGGRPMONDEST_ID"]);
                IdGrupoMonitoria = Convert.ToInt32(dt.Rows[0]["MGGRPMON_ID"]);
                NomeDestinatario = (String)dt.Rows[0]["MGGRPMONDESTLOG_NM"];
                Destinatario = (String)dt.Rows[0]["MGGRPMONDESTLOG_DS"];
                IdUsuarioInclusao = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                DataInclusao = Convert.ToDateTime(dt.Rows[0]["MGGRPMONDESTLOG_DH_USU_INS"]);
                Operacao = (String)dt.Rows[0]["MGGRPMONDESTLOG_OPERACAO"];

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("MGGRPMONLOG_NM")) { dt.Columns["MGGRPMONLOG_NM"].ColumnName = "NomeGrupoMonitoria"; }
            if (dt.Columns.Contains("MGGRPMONDESTLOG_ID")) { dt.Columns["MGGRPMONDESTLOG_ID"].ColumnName = "IdGrupoMonitoriaDestinatarioLog"; }
            if (dt.Columns.Contains("MGGRPMONDEST_ID")) { dt.Columns["MGGRPMONDEST_ID"].ColumnName = "IdGrupoMonitoriaDestinatario"; }
            if (dt.Columns.Contains("MGGRPMON_ID")) { dt.Columns["MGGRPMON_ID"].ColumnName = "IdGrupoMonitoria"; }
            if (dt.Columns.Contains("MGGRPMONDESTLOG_NM")) { dt.Columns["MGGRPMONDESTLOG_NM"].ColumnName = "NomeDestinatario"; }
            if (dt.Columns.Contains("MGGRPMONDESTLOG_DS")) { dt.Columns["MGGRPMONDESTLOG_DS"].ColumnName = "Destinatario"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioCadastro"; }
            if (dt.Columns.Contains("MGGRPMONDESTLOG_DH_USU_INS")) { dt.Columns["MGGRPMONDESTLOG_DH_USU_INS"].ColumnName = "DataInclusao"; }
            if (dt.Columns.Contains("MGGRPMONDESTLOG_OPERACAO")) { dt.Columns["MGGRPMONDESTLOG_OPERACAO"].ColumnName = "Operacao"; }
        }
        #endregion
    }
}