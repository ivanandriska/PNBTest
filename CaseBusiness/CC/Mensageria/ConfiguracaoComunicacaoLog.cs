#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Mensageria
{
    public class ConfiguracaoComunicacaoLog : BusinessBase
    {
        #region Atributos
        private Int32 _idConfiguracaoComunicacaoLog = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private Int32 _idCanal = Int32.MinValue;
        private Int32 _codigoConfiguracaoComunicacao = Int32.MinValue;
        private String _nomeConfiguracaoComunicacao = String.Empty;
        private String _textoConfiguracaoComunicacaoCliente = String.Empty;
        private String _descricaoConfiguracaoComunicacao = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private String _nomeUsuarioInclusao = String.Empty;
        private DateTime _dataHoraInclusao = DateTime.MinValue;
        private String _codigoOperacao = String.Empty;
        #endregion Atributos

        #region Propriedades
        public Int32 IdConfiguracaoComunicacaoLog
        {
            get { return _idConfiguracaoComunicacaoLog; }
            set { _idConfiguracaoComunicacaoLog = value; }
        }

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

        public String TextoConfiguracaoComunicacaoCliente
        {
            get { return _textoConfiguracaoComunicacaoCliente; }
            set { _textoConfiguracaoComunicacaoCliente = value; }
        }

        public String DescricaoConfiguracaoComunicacao
        {
            get { return _descricaoConfiguracaoComunicacao; }
            set { _descricaoConfiguracaoComunicacao = value; }
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

        public DateTime DataHoraInclusao
        {
            get { return _dataHoraInclusao; }
            set { _dataHoraInclusao = value; }
        }

        public String CodigoOperacao
        {
            get { return _codigoOperacao; }
            set { _codigoOperacao = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe ConfiguracaoComunicacaoLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public ConfiguracaoComunicacaoLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe ConfiguracaoComunicacaoLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public ConfiguracaoComunicacaoLog(Int32 idUsuarioManutencao,
                                       CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe ConfiguracaoComunicacaoLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ConfiguracaoComunicacaoLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe ConfiguracaoComunicacaoLog e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idConfiguracaoComunicacaoLog">Id do Log Configuração da Comunicação</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public ConfiguracaoComunicacaoLog(Int32 idConfiguracaoComunicacaoLog,
                                       Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idConfiguracaoComunicacaoLog);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar ConfiguracaoComunicacaoLog
        /// </summary>
        /// <param name="codigoOrganizacao">Código Org</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="codigoConfiguracaoComunicacao">Código da Configuração da Comunicação</param>
        /// <param name="nomeConfiguracaoComunicacao">Nome Configuração da Comunicação</param>
        public DataTable Buscar(Int32 codigoOrganizacao,
                                Int32 idCanal,
                                Int32 codigoConfiguracaoComunicacao,
                                String nomeConfiguracaoComunicacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacaoLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);
                acessoDadosBase.AddParameter("@MGCONFCOMLOG_NM", nomeConfiguracaoComunicacao.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGCONFMSGLOG_SEL_BUSCAR").Tables[0];

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
        /// Inclui Log da Configuração da Mensagem
        /// </summary>
        /// <param name="codigoConfiguracaoComunicacao">Código da Configuração da Comunicação</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="nomeConfiguracaoComunicacao">Nome Configuração da Comunicação</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="textoConfiguracaoComunicacaoCliente">Texto da Configuração da Comunicação do Cliente</param>
        /// <param name="descricaoConfiguracaoComunicacao">Descrição Configuracao da Comunicação</param>
        /// <param name="codigoOperacao">Operação (ALT = ALTteração, INC = INClusão, EXC = EXClusão</param>
        /// <param name="dataHoraInclusao">Data e Hora da Manutenção</param>
        /// <param name="idConfiguracaoComunicacaoLog">Id do Log da Configuração da Mensagem</param>
        /// <returns>Retorna o Id do Log da Configuração de Mensagem</returns>
        public void Incluir(Int32 codigoConfiguracaoComunicacao, 
                            Int32 codigoOrganizacao, 
                            String nomeConfiguracaoComunicacao, 
                            Int32 idCanal, 
                            String textoConfiguracaoComunicacaoCliente, 
                            String descricaoConfiguracaoComunicacao,
                            String codigoOperacao, 
                            DateTime dataHoraInclusao,
                            ref Int32 idConfiguracaoComunicacaoLog)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacaoLog está operando em Modo Entidade Only"); }

                idConfiguracaoComunicacaoLog = Int32.MinValue;

                acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGCONFCOMLOG_NM", nomeConfiguracaoComunicacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCONFCOMLOG_TX_CLIENTE", textoConfiguracaoComunicacaoCliente);
                acessoDadosBase.AddParameter("@MGCONFCOMLOG_DS", descricaoConfiguracaoComunicacao);
                acessoDadosBase.AddParameter("@MGCONFCOMLOG_OPERACAO", codigoOperacao);
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGCONFCOMLOG_DH_USUARIO_INS", dataHoraInclusao);
                acessoDadosBase.AddParameter("@MGCONFCOMLOG_ID", Int32.MaxValue, ParameterDirection.InputOutput);

                idConfiguracaoComunicacaoLog = Convert.ToInt32(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGCONFCOMLOG_INS")[0]);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consultar um ConfiguracaoComunicacaoLog
        /// </summary>
        /// <param name="idConfiguracaoComunicacaoLog">Id do Log da Configuração da Comunicação</param>
        private void Consultar(Int32 idConfiguracaoComunicacaoLog)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacaoLog está operando em Modo Entidade Only"); }

                DataTable dt;

                acessoDadosBase.AddParameter("@MGCONFCOMLOG_ID", idConfiguracaoComunicacaoLog);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGCONFMSGLOG_SEL_CONSULTAR").Tables[0];

                //// Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                    IdCanal = Convert.ToInt32(dt.Rows[0]["COMCNL_ID"]);
                    CodigoConfiguracaoComunicacao = Convert.ToInt32(dt.Rows[0]["MGCONFCOM_CD"]);
                    NomeConfiguracaoComunicacao = dt.Rows[0]["MGCONFCOMLOG_NM"].ToString();
                    TextoConfiguracaoComunicacaoCliente = dt.Rows[0]["MGCONFCOMLOG_TX_CLIENTE"].ToString();
                    DescricaoConfiguracaoComunicacao = dt.Rows[0]["MGCONFCOMLOG_DS"].ToString();
                    NomeUsuarioInclusao = dt.Rows[0]["USU_NM"].ToString();
                    DataHoraInclusao = Convert.ToDateTime(dt.Rows[0]["MGCONFCOMLOG_DH_USUARIO_INS"].ToString());
                    CodigoOperacao = dt.Rows[0]["MGCONFCOMLOG_OPERACAO"].ToString();

                    __blnIsLoaded = true; ;
                }
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
            if (dt.Columns.Contains("MGCONFCOMLOG_ID")) { dt.Columns["MGCONFCOMLOG_ID"].ColumnName = "IdConfiguracaoComunicacaoLog"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "IdCanal"; }
            if (dt.Columns.Contains("COMCNL_NM")) { dt.Columns["COMCNL_NM"].ColumnName = "NomeCanal"; }
            if (dt.Columns.Contains("MGCONFCOM_CD")) { dt.Columns["MGCONFCOM_CD"].ColumnName = "CodigoConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("MGCONFCOMLOG_NM")) { dt.Columns["MGCONFCOMLOG_NM"].ColumnName = "NomeConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("MGCONFCOMLOG_TX_CLIENTE")) { dt.Columns["MGCONFCOMLOG_TX_CLIENTE"].ColumnName = "TextoConfiguracaoComunicacaoCliente"; }
            if (dt.Columns.Contains("MGCONFCOMLOG_DS")) { dt.Columns["MGCONFCOMLOG_DS"].ColumnName = "DescricaoConfiguracaoComunicacao"; }

            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            if (dt.Columns.Contains("MGCONFCOMLOG_DH_USUARIO_INS")) { dt.Columns["MGCONFCOMLOG_DH_USUARIO_INS"].ColumnName = "DataHoraInclusao"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioResponsavel"; }
            if (dt.Columns.Contains("MGCONFCOMLOG_OPERACAO")) { dt.Columns["MGCONFCOMLOG_OPERACAO"].ColumnName = "CodigoOperacao"; }

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }

        #endregion Métodos
    }
}