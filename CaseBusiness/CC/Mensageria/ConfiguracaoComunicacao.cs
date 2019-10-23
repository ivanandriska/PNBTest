#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
using System.Runtime.Caching;
#endregion Using

namespace CaseBusiness.CC.Mensageria
{
    public class ConfiguracaoComunicacao : BusinessBase
    {
        #region Atributos
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private Int32 _idCanal = Int32.MinValue;
        private Int32 _codigoConfiguracaoComunicacao = Int32.MinValue;
        private String _nomeConfiguracaoComunicacao = String.Empty;
        private String _codigoNomeConfiguracaoComunicacao = String.Empty;
        private String _descricaoConfiguracaoComunicacao = String.Empty;
        private String _textoConfiguracaoComunicacaoCliente = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private String _nomeUsuarioInclusao = String.Empty;
        private DateTime _dataHoraUsarioInclusao = DateTime.MinValue;
        private Int32 _idUsuarioAlteracao = Int32.MinValue;
        private String _nomeUsuarioAlteracao = String.Empty;
        private DateTime _dataHoraUsarioAlteracao = DateTime.MinValue;
        private DataTable _dtRestricoesExclusao = null;
        private DataTable _dtRestricoesAlteracao = null;

        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 60;  // 60 minutos * 24 horas
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 10;  // 60 minutos * 24 horas
        #endregion Atributos

        #region Propriedades
        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public Int32 IdCanal
        {
            get { return _idCanal; }
            set { _idCanal = value; }
        }

        public Int32 CodigoConfiguracaoComunicacao
        {
            get { return _codigoConfiguracaoComunicacao; }
            set { _codigoConfiguracaoComunicacao = value; }
        }

        public String NomeConfiguracaoComunicacao
        {
            get { return _nomeConfiguracaoComunicacao; }
            set { _nomeConfiguracaoComunicacao = value; }
        }

        public String CodigoNomeConfiguracaoComunicacao
        {
            get { return _codigoNomeConfiguracaoComunicacao; }
            set { _codigoNomeConfiguracaoComunicacao = value; }
        }

        public String DescricaoConfiguracaoComunicacao
        {
            get { return _descricaoConfiguracaoComunicacao; }
            set { _descricaoConfiguracaoComunicacao = value; }
        }

        public String TextoConfiguracaoComunicacaoCliente
        {
            get { return _textoConfiguracaoComunicacaoCliente; }
            set { _textoConfiguracaoComunicacaoCliente = value; }
        }

        public Int32 IdUsuarioInclusao
        {
            get { return _idUsuarioInclusao; }
            set { _idUsuarioInclusao = value; }
        }

        public String NomeUsuarioInclusao
        {
            get { return _nomeUsuarioInclusao; }
            set { _nomeUsuarioInclusao = value; }
        }

        public DateTime DataHoraUsarioInclusao
        {
            get { return _dataHoraUsarioInclusao; }
            set { _dataHoraUsarioInclusao = value; }
        }

        public Int32 IdUsuarioAlteracao
        {
            get { return _idUsuarioAlteracao; }
            set { _idUsuarioAlteracao = value; }
        }

        public String NomeUsuarioAlteracao
        {
            get { return _nomeUsuarioAlteracao; }
            set { _nomeUsuarioAlteracao = value; }
        }

        public DateTime DataHoraUsarioAlteracao
        {
            get { return _dataHoraUsarioAlteracao; }
            set { _dataHoraUsarioAlteracao = value; }
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
        /// Construtor classe ConfiguracaoComunicacao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public ConfiguracaoComunicacao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe ConfiguracaoComunicacao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ConfiguracaoComunicacao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe ConfiguracaoComunicacao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public ConfiguracaoComunicacao(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe ConfiguracaoComunicacao e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoGrupo">Código do ConfiguracaoComunicao</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ConfiguracaoComunicacao(Int32 codigoConfiguracaoComunicacao, 
                                       Int32 codigoOrganizacao, 
                                       Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(codigoConfiguracaoComunicacao, codigoOrganizacao);
        }

        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar um Configuração da Comunicação por ID
        /// </summary>
        /// <param name="codigoConfiguracaoComunicacao">Código da Configuração da Comunicação</param>
        /// <returns></returns>
        public DataTable Buscar(Int32 codigoConfiguracaoComunicacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGCONFCOM_SEL_CONSULTAR").Tables[0];

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
        /// Buscar Configuração da Comunicação
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">idCanal</param>
        /// <param name="codigoConfiguracaoComunicacao">Código da Configuração da Comunicação</param>
        /// <param name="nomeConfiguracaoComunicacao">Nome da Configuração da Cominica;'ao</param>
        /// <param name="destinatario">Destinatário</param>
        /// <returns></returns>
        public DataTable Buscar(Int32 codigoOrganizacao,
                                Int32 idCanal,
                                Int32 codigoConfiguracaoComunicacao,
                                String nomeConfiguracaoComunicacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);
                acessoDadosBase.AddParameter("@MGCONFCOM_NM", nomeConfiguracaoComunicacao.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGCONFCOM_SEL_BUSCAR").Tables[0];

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
        /// Listar Configuração da Comunicação
        /// </summary>
        /// <param name="codigoOrganizacao">Codigo Org</param>Comunicaçãop</param>
        public DataTable Listar(Int32 codigoOrganizacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGCONFCOM_SEL_LISTAR").Tables[0];

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
        /// Consultar um Configuração da Comunição
        /// </summary>
        /// <param name="codigoConfiguracaoComunicacao">Codigo da Configuração da Comunicação</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        private void Consultar(Int32 codigoConfiguracaoComunicacao, 
                               Int32 codigoOrganizacao)
        {
            try
            {
                DataTable dt;
                String kCacheKey = "ConfiguracaoComunicacao" + codigoConfiguracaoComunicacao.ToString() + codigoOrganizacao.ToString();

                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dt = MemoryCache.Default[kCacheKey] as DataTable;
                }
                else
                {
                    if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                    acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);
                    acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                        "prMGCONFCOM_SEL_CONSULTAR").Tables[0];
                }
                //// Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                    IdCanal = Convert.ToInt32(dt.Rows[0]["COMCNL_ID"]);
                    CodigoConfiguracaoComunicacao = Convert.ToInt32(dt.Rows[0]["MGCONFCOM_CD"]);
                    NomeConfiguracaoComunicacao = dt.Rows[0]["MGCONFCOM_NM"].ToString();
                    DescricaoConfiguracaoComunicacao = dt.Rows[0]["MGCONFCOM_DS"].ToString();
                    TextoConfiguracaoComunicacaoCliente = dt.Rows[0]["MGCONFCOM_TX_CLIENTE"].ToString();
                    IdUsuarioInclusao = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                    NomeUsuarioInclusao = dt.Rows[0]["USU_NM_INS"].ToString();
                    DataHoraUsarioInclusao = Convert.ToDateTime(dt.Rows[0]["MGCONFCOM_DH_USUARIO_INS"]);
                    CodigoNomeConfiguracaoComunicacao = dt.Rows[0]["MGCONFCOM_CD_NM"].ToString();

                    if (dt.Rows[0]["USU_ID_UPD"] != DBNull.Value)
                    {
                        IdUsuarioAlteracao = Convert.ToInt32(dt.Rows[0]["USU_ID_UPD"]);
                        NomeUsuarioAlteracao = dt.Rows[0]["USU_NM_UPD"].ToString();
                        DataHoraUsarioAlteracao = Convert.ToDateTime(dt.Rows[0]["MGCONFCOM_DH_USUARIO_UPD"]);
                    }

                    __blnIsLoaded = true;

                    MemoryCache.Default.Set(kCacheKey, dt,
                    new CacheItemPolicy()
                    {
                        SlidingExpiration = new TimeSpan(DateTime.Now.AddMinutes(kCache_SLIDINGEXPIRATION_MINUTES).Ticks - DateTime.Now.Ticks)
                    });
                }


            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Inclui uma Configuração de Comunicação
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="codigoConfiguracaoComunicacao">Código da Configuração da Comunicação</param>
        /// <param name="nomeConfiguracaoComunicacao">Nome da Configuração da Comunicação</param>
        /// <param name="textoConfiguracaoComunicacaoCliente">Texto de Configuraçãoo de Comunicação do Cliente</param>
        /// <param name="descricaoConfiguracaoComunicacao">Descrição da Configuração da Comunicação</param>
        /// <param name="dhInclusao">Data Hora Inclusao</param>
        public void Incluir(Int32 codigoOrganizacao,
                            Int32 idCanal,
                            Int32 codigoConfiguracaoComunicacao,
                            String nomeConfiguracaoComunicacao,
                            String textoConfiguracaoComunicacaoCliente,
                            String descricaoConfiguracaoComunicacao,
                            DateTime dhInclusao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);
                acessoDadosBase.AddParameter("@MGCONFCOM_NM", nomeConfiguracaoComunicacao.Trim());
                acessoDadosBase.AddParameter("@MGCONFCOM_TX_CLIENTE", textoConfiguracaoComunicacaoCliente.Trim());
                acessoDadosBase.AddParameter("@MGCONFCOM_DS", descricaoConfiguracaoComunicacao.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGCONFCOM_DH_USUARIO_INS", dhInclusao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prMGCONFCOM_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Alterar uma Configuração de Comunicação
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="codigoConfiguracaoComunicacao">Código da Confioguração da Comunicação</param>
        /// <param name="nomeConfiguracaoComunicacao">Nome Configuração da Comunicação</param>
        /// <param name="textoConfiguracaoComunicacaoCliente">Texto de Configuraçãoo de Comunicação do Cliente</param>
        /// /// <param name="descricaoConfiguracaoComunicacao">Descrição da Configuração da Comunicação</param>
        /// <param name="dataHoraUsarioAlteracao">Data Hora Alteração</param>
        public void Alterar(Int32 codigoOrganizacao,
                            Int32 idCanal,
                            Int32 codigoConfiguracaoComunicacao,
                            String nomeConfiguracaoComunicacao,
                            String textoConfiguracaoComunicacaoCliente,
                            String descricaoConfiguracaoComunicacao,
                            DateTime dataHoraUsarioAlteracao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);
                acessoDadosBase.AddParameter("@MGCONFCOM_NM", nomeConfiguracaoComunicacao.Trim());
                acessoDadosBase.AddParameter("@MGCONFCOM_TX_CLIENTE", textoConfiguracaoComunicacaoCliente.Trim());
                acessoDadosBase.AddParameter("@MGCONFCOM_DS", descricaoConfiguracaoComunicacao.Trim());
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGCONFCOM_DH_USUARIO_UPD", dataHoraUsarioAlteracao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prMGCONFCOM_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Exclui um Configuração de Comunicação 
        /// </summary>
        /// <param name="codigoConfiguracaoComunicacao">Código da Configuração Comunicação</param>
        public void Excluir(Int32 codigoConfiguracaoComunicacao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prMGCONFCOM_DEL");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Obtem as Restrições de Exclusão da Regra carregada
        /// </summary>
        private DataTable ObterRestricoesExclusao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGCONFCOM_CD", CodigoConfiguracaoComunicacao);

                dt = new DataTable();
                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGCONFCOM_SEL_RESTRIC_DEL").Tables[0];

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
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "IdCanal"; }
            if (dt.Columns.Contains("COMCNL_NM")) { dt.Columns["COMCNL_NM"].ColumnName = "NomeCanal"; }
            if (dt.Columns.Contains("MGCONFCOM_CD")) { dt.Columns["MGCONFCOM_CD"].ColumnName = "CodigoConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("MGCONFCOM_NM")) { dt.Columns["MGCONFCOM_NM"].ColumnName = "NomeConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("MGCONFCOM_NM")) { dt.Columns["MGCONFCOM_NM"].ColumnName = "NomeConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("MGCONFCOM_DS")) { dt.Columns["MGCONFCOM_DS"].ColumnName = "DescricaoConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("MGCONFCOM_LIST")) { dt.Columns["MGCONFCOM_LIST"].ColumnName = "DropDownListConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("MGCONFCOM_TX_CLIENTE")) { dt.Columns["MGCONFCOM_TX_CLIENTE"].ColumnName = "TextoConfiguracaoComunicacaoCliente"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            if (dt.Columns.Contains("USU_NM_INS")) { dt.Columns["USU_NM_INS"].ColumnName = "NomeUsuarioInclusao"; }
            if (dt.Columns.Contains("MGCONFCOM_DH_USUARIO_INS")) { dt.Columns["MGCONFCOM_DH_USUARIO_INS"].ColumnName = "DataHoraUsarioInclusao"; }
            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "IdUsuarioAlteracao"; }
            if (dt.Columns.Contains("USU_NM_UPD")) { dt.Columns["USU_NM_UPD"].ColumnName = "NomeUsuarioAlteracao"; }
            if (dt.Columns.Contains("MGCONFCOM_DH_USUARIO_UPD")) { dt.Columns["MGCONFCOM_DH_USUARIO_UPD"].ColumnName = "DataHoraUsarioAlteracao"; }

            if (dt.Columns.Contains("ROWS_COUNT_DEL")) { dt.Columns["ROWS_COUNT_DEL"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA_DEL")) { dt.Columns["RESTRICAO_TABELA_DEL"].ColumnName = "RestricoesExclusaoTabela"; }
        }
        #endregion Métodos
    }
}
