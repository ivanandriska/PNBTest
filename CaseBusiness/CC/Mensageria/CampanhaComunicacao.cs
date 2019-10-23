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
    public class CampanhaComunicacao : BusinessBase
    {

        #region Atributos
        private Int32 _idCampanha = Int32.MinValue;
        private Int32 _codigoConfiguracaoComunicacao = Int32.MinValue;
        private Int32 _quantidadeMaxAudiencia = Int32.MinValue;
        private Int32 _idFornecedora = Int32.MinValue;
        private Int32 _idSeedList = Int32.MinValue;
        private String _codigoCampanhaExterno = String.Empty;
        private Int32 _idCanal = Int32.MinValue;
        private Int32 _idGrupoTeste = Int32.MinValue;

        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 120;  // 60 minutos * 24 horas
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 30;  // 60 minutos * 24 horas

        #endregion Atributos

        #region Propriedades
        public Int32 IdCampanha
        {
            get { return _idCampanha; }
            set { _idCampanha = value; }
        }
        public Int32 CodigoConfiguracaoComunicacao
        {
            get { return _codigoConfiguracaoComunicacao; }
            set { _codigoConfiguracaoComunicacao = value; }
        }
        public Int32 QuantidadeMaxAudiencia
        {
            get { return _quantidadeMaxAudiencia; }
            set { _quantidadeMaxAudiencia = value; }
        }
        public Int32 IdFornecedora
        {
            get { return _idFornecedora; }
            set { _idFornecedora = value; }
        }
        public Int32 IdSeedList
        {
            get { return _idSeedList; }
            set { _idSeedList = value; }
        }
        public String CodigoCampanhaExterno
        {
            get { return _codigoCampanhaExterno; }
            set { _codigoCampanhaExterno = value; }
        }
        public Int32 IdCanal
        {
            get { return _idCanal; }
            set { _idCanal = value; }
        }
        public Int32 IdGrupoTeste
        {
            get { return _idGrupoTeste; }
            set { _idGrupoTeste = value; }
        }

        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Configuracao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public CampanhaComunicacao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Configuracao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public CampanhaComunicacao()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public CampanhaComunicacao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public CampanhaComunicacao(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Campanhas e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idCampanhas">ID Restrição</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public CampanhaComunicacao(Int32 idCampanhas, Int32 idUsuarioManutencao)
           : this(idUsuarioManutencao)
        {
            Consultar(idCampanhas);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar Configuração de Envio
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id da Canal</param>
        public DataTable Buscar(Int32 idCampanha,
                                Int32 idCanal,
                                Int32 cdConfComunicacao)
        {
            DataTable dt = null;
            String kCacheKey = "CampanhaComunicacao_" + idCampanha.ToString() + idCanal.ToString() + cdConfComunicacao.ToString();

            try
            {
                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dt = MemoryCache.Default[kCacheKey] as DataTable;
                    return dt;
                }


                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCONFCOM_CD", cdConfComunicacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "PRMGCMPCOM_SEL_BUSCAR").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                MemoryCache.Default.Set(kCacheKey, dt,
                new CacheItemPolicy()
                {
                    SlidingExpiration = new TimeSpan(DateTime.Now.AddMinutes(kCache_SLIDINGEXPIRATION_MINUTES).Ticks - DateTime.Now.Ticks)
                });
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Inclui uma Configuracao
        /// </summary>
        /// <param name="idCampanha">Id Campanha</param>
        /// <param name="cdConfComuicacao">Cod. Configurcao Comunicação</param>
        /// <param name="qtMaximaAudiencia">Qtde Maxima Audiência</param>
        /// <param name="IdForncedor">Id Forncedor</param>
        /// <param name="IdSeed">Id Seed</param>
        /// <param name="idCanal">id Canal</param>
        /// <param name="idTeste">id Teste</param>
        public void Incluir(Int32 idCampanha,
                            Int32 cdConfComuicacao,
                            Int32 qtMaximaAudiencia,
                            Int32 IdForncedor,
                            Int32 IdSeed,
                            //Int32 cdCampanhaExterna,
                            Int32 idCanal,
                            Int32 idTeste)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@MGCONFCOM_CD", cdConfComuicacao);
                acessoDadosBase.AddParameter("@CMPCOM_QT_MAX_AUDIENCIA", qtMaximaAudiencia);
                acessoDadosBase.AddParameter("@COMFORN_ID", IdForncedor);
                acessoDadosBase.AddParameter("@MGGRPTST_ID_SEED", IdSeed);
                //acessoDadosBase.AddParameter("@CMPCOM_CD_CAMPANHA_EXT", cdCampanhaExterna);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGGRPTST_ID_TESTE", idTeste);


                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPCOM_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Inclui uma Configuracao
        /// </summary>
        /// <param name="idCampanha">Id Campanha</param>
        /// <param name="cdConfComuicacao">Cod. Configurcao Comunicação</param>
        /// <param name="qtMaximaAudiencia">Qtde Maxima Audiência</param>
        /// <param name="IdForncedor">Id Forncedor</param>
        /// <param name="IdSeed">Id Seed</param>
        /// <param name="idCanal">id Canal</param>
        /// <param name="idTeste">id Teste</param>
        public void Alterar(Int32 idCampanha,
                            Int32 cdConfComuicacao,
                            Int32 qtMaximaAudiencia,
                            Int32 IdForncedor,
                            Int32 IdSeed,
                            //Int32 cdCampanhaExterna,
                            Int32 idCanal,
                            Int32 idTeste)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ConfiguracaoComunicacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@MGCONFCOM_CD", cdConfComuicacao);
                acessoDadosBase.AddParameter("@CMPCOM_QT_MAX_AUDIENCIA", qtMaximaAudiencia);
                acessoDadosBase.AddParameter("@COMFORN_ID", IdForncedor);
                acessoDadosBase.AddParameter("@MGGRPTST_ID_SEED", IdSeed);
                //acessoDadosBase.AddParameter("@CMPCOM_CD_CAMPANHA_EXT", cdCampanhaExterna);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGGRPTST_ID_TESTE", idTeste);


                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPCOM_UPD");

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        // <summary>
        /// Consultar Campanha Comunica~ção
        /// </summary>
        /// <param name="idCampanha">Id Campanha</param>        
        private void Consultar(Int32 idCampanhas)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Campanhas está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanhas);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCMPCOM_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdCampanha = Convert.ToInt32(dt.Rows[0]["CMP_ID"]);
                    CodigoConfiguracaoComunicacao = Convert.ToInt32(dt.Rows[0]["MGCONFCOM_CD"]);

                    if (dt.Rows[0]["CMPCOM_QT_MAX_AUDIENCIA"] != DBNull.Value)
                    {
                        QuantidadeMaxAudiencia = Convert.ToInt32(dt.Rows[0]["CMPCOM_QT_MAX_AUDIENCIA"]);
                    }

                    IdFornecedora = Convert.ToInt32(dt.Rows[0]["COMFORN_ID"]);

                    if (dt.Rows[0]["MGGRPTST_ID_SEED"] != DBNull.Value)
                    {
                        IdSeedList = Convert.ToInt32(dt.Rows[0]["MGGRPTST_ID_SEED"]);
                    }

                    CodigoCampanhaExterno = dt.Rows[0]["CMPCOM_CD_CAMPANHA_EXT"].ToString();

                    IdCanal = Convert.ToInt32(dt.Rows[0]["COMCNL_ID"].ToString());

                    if (dt.Rows[0]["MGGRPTST_ID_TESTE"] != DBNull.Value)
                    {
                        IdGrupoTeste = Convert.ToInt32(dt.Rows[0]["MGGRPTST_ID_TESTE"].ToString());
                    }

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
        /// Excluir uma Campanha Comunicação
        /// </summary>        
        /// <param name="idCampanha">Id Campanha</param>
        public void Excluir(Int32 idCampanha)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe QualificadorDefinicao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPCOM_DEL");
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
            if (dt.Columns.Contains("CMP_ID")) { dt.Columns["CMP_ID"].ColumnName = "idCampanha"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("MGCONFCOM_CD")) { dt.Columns["MGCONFCOM_CD"].ColumnName = "codigoConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("CMPCOM_QT_MAX_AUDIENCIA")) { dt.Columns["CMPCOM_QT_MAX_AUDIENCIA"].ColumnName = "quantidadeMaxAudiencia"; }
            if (dt.Columns.Contains("COMFORN_ID")) { dt.Columns["COMFORN_ID"].ColumnName = "idFornecedora"; }
            if (dt.Columns.Contains("CMPCOM_CD_CAMPANHA_EXT")) { dt.Columns["CMPCOM_CD_CAMPANHA_EXT"].ColumnName = "codigoCampanhaExterno"; }

            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "idCanal"; }
            if (dt.Columns.Contains("MGGRPTST_ID_TESTE")) { dt.Columns["MGGRPTST_ID_TESTE"].ColumnName = "idGrupoTeste"; }

            if (dt.Columns.Contains("MGGRPTST_ID_SEED")) { dt.Columns["MGGRPTST_ID_SEED"].ColumnName = "idSeedList"; }
        }
        #endregion Métodos
    }
}
