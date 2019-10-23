#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Caching;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Global
{
    public class OrganizacaoImagemParametro : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Global_OrganizacaoImagemParametro_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private Int32 _idOrganizacaoImagemParametro = Int32.MinValue;
        private StatusOrganizacao.enumStatus _statusOrganizacao;
        //private CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem _codigoEstrategiaAnaliseChecagem = ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem.EMPTY;
        private Int32 _tempoTransacaoExpiracao = Int32.MinValue;
        private Int32 _tempoTransacaoExibicao = Int32.MinValue;
        private Int32 _qtdeTransacoesHistorico = Int32.MinValue;
        private Int32 _tempoMensagemExibicao = Int32.MinValue;
        private Int32 _idUsuarioIns = Int32.MinValue;
        private DateTime _dhGeracao = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public Int32 IdOrganizacaoImagemParametro
        {
            get { return _idOrganizacaoImagemParametro; }
            set { _idOrganizacaoImagemParametro = value; }
        }

        public StatusOrganizacao.enumStatus StatusOrganizacao
        {
            get { return _statusOrganizacao; }
            set { _statusOrganizacao = value; }
        }

        //public CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem CodigoEstrategiaAnaliseChecagem
        //{
        //    get { return _codigoEstrategiaAnaliseChecagem; }
        //    set { _codigoEstrategiaAnaliseChecagem = value; }
        //}

        public Int32 TempoTransacaoExpiracao
        {
            get { return _tempoTransacaoExpiracao; }
            set { _tempoTransacaoExpiracao = value; }
        }

        public Int32 TempoTransacaoExibicao
        {
            get { return _tempoTransacaoExibicao; }
            set { _tempoTransacaoExibicao = value; }
        }

        public Int32 QtdeTransacoesHistorico
        {
            get { return _qtdeTransacoesHistorico; }
            set { _qtdeTransacoesHistorico = value; }
        }

        public Int32 TempoMensagemExibicao
        {
            get { return _tempoMensagemExibicao; }
            set { _tempoMensagemExibicao = value; }
        }

        public Int32 IdUsuarioIns
        {
            get { return _idUsuarioIns; }
            set { _idUsuarioIns = value; }
        }

        public DateTime DataHoraGeracao
        {
            get { return _dhGeracao; }
            set { _dhGeracao = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe OrganizacaoImgParm - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public OrganizacaoImagemParametro(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe OrganizacaoImgParm
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public OrganizacaoImagemParametro(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe OrganizacaoImgParm utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public OrganizacaoImagemParametro(Int32 idUsuarioManutencao,
                                     CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe OrganizacaoImgParm e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idOrganizacaoImagemParametro">ID da Imagem de Parâmetros da Organização</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public OrganizacaoImagemParametro(Int32 codigoOrganizacao,
                                          Int32 idOrganizacaoImagemParametro,
                                          Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(codigoOrganizacao, idOrganizacaoImagemParametro);
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Inclui uma Imagem do parâmetro da Organização
        /// </summary>
        public void Gerar(Int32 codigoOrganizacao,
                            StatusOrganizacao.enumStatus statusOrganizacao,
                            //CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem codigoEstrategiaAnaliseChecagem,
                            Int32 tempoTransacaoExpiracao,
                            Int32 tempoTransacaoExibicao,
                            Int32 qtdeTransacoesHistorico, 
                            Int32 tempoMensagemExibicao, 
                            DateTime dataImagem,
                            ref Int32 idOrganizacaoImg)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe OrganizacaoImgParm está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@ORGIMG_ST", CaseBusiness.CC.Global.StatusOrganizacao.ObterDBValue(statusOrganizacao));
                //acessoDadosBase.AddParameter("@ORGIMG_ESACHEC_CD", CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.ObterDBValue(codigoEstrategiaAnaliseChecagem));
                acessoDadosBase.AddParameter("@ORGIMG_TRS_TEMPO_EXPIRACAO", tempoTransacaoExpiracao);
                acessoDadosBase.AddParameter("@ORGIMG_TRS_TEMPO_EXIBICAO", tempoTransacaoExibicao);
                acessoDadosBase.AddParameter("@ORGIMG_QTDE_TRS_HIST", qtdeTransacoesHistorico);
                acessoDadosBase.AddParameter("@ORGIMG_MSG_TEMPO_EXIBICAO", tempoMensagemExibicao);
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@ORGIMG_DH_GERACAO", dataImagem);
                acessoDadosBase.AddParameter("@ORGIMG_ID", Int32.MaxValue, ParameterDirection.InputOutput);

                idOrganizacaoImg = Convert.ToInt32(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure,
                                                                                             "prORGIMG_INS")[0]);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consulta uma Imagem de Parâmentro de uma Organizacao
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idOrganizacaoImagemParametro">ID da Imagem de Parâmetros da Organização</param>
        private void Consultar(Int32 codigoOrganizacao,
                               Int32 idOrganizacaoImagemParametro)
        {
            try
            {
                DataTable dt;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Grupo está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@ORGIMG_ID", idOrganizacaoImagemParametro);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prORGIMG_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;


                if (dt.Rows.Count > 0)
                {
                    CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["CodigoOrganizacao"]);
                    IdOrganizacaoImagemParametro = Convert.ToInt32(dt.Rows[0]["IdOrganizacaoImagemParametro"]);
                    StatusOrganizacao = CaseBusiness.CC.Global.StatusOrganizacao.ObterEnum(Convert.ToString(dt.Rows[0]["StatusOrganizacao"]));
                    //CodigoEstrategiaAnaliseChecagem = CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.ObterEnum(Convert.ToString(dt.Rows[0]["CodigoEstrategiaAnaliseChecagem"]));
                    TempoTransacaoExpiracao = Convert.ToInt32(dt.Rows[0]["tempoTransacaoExpiracao"]);
                    TempoTransacaoExibicao = Convert.ToInt32(dt.Rows[0]["tempoTransacaoExibicao"]);
                    QtdeTransacoesHistorico = Convert.ToInt32(dt.Rows[0]["QtdeTransacoesHistorico"]);
                    TempoMensagemExibicao = Convert.ToInt32(dt.Rows[0]["TempoMensagemExibicao"]);
                    IdUsuarioIns = Convert.ToInt32(dt.Rows[0]["IdUsuarioIns"]);
                    DataHoraGeracao = Convert.ToDateTime(dt.Rows[0]["DataHoraGeracao"]);

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
        /// Buscar Organização
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public DataTable Buscar(Int32 codigoOrganizacao)
        {
            DataTable dt = null;
            //DataView dv = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organização está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                          "prORGIMG_SEL_BUSCAR").Tables[0];
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
        /// Remove do Cache as Organizações
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
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORGIMG_ID")) { dt.Columns["ORGIMG_ID"].ColumnName = "IdOrganizacaoImagemParametro"; }
            if (dt.Columns.Contains("ORGIMG_ST")) { dt.Columns["ORGIMG_ST"].ColumnName = "StatusOrganizacao"; }
            if (dt.Columns.Contains("ORGIMG_ESACHEC_CD")) { dt.Columns["ORGIMG_ESACHEC_CD"].ColumnName = "CodigoEstrategiaAnaliseChecagem"; }
            if (dt.Columns.Contains("ORGIMG_TRS_TEMPO_EXPIRACAO")) { dt.Columns["ORGIMG_TRS_TEMPO_EXPIRACAO"].ColumnName = "tempoTransacaoExpiracao"; }
            if (dt.Columns.Contains("ORGIMG_TRS_TEMPO_EXIBICAO")) { dt.Columns["ORGIMG_TRS_TEMPO_EXIBICAO"].ColumnName = "tempoTransacaoExibicao"; }            
            if (dt.Columns.Contains("ORGIMG_QTDE_TRS_HIST")) { dt.Columns["ORGIMG_QTDE_TRS_HIST"].ColumnName = "QtdeTransacoesHistorico"; }
            if (dt.Columns.Contains("ORGIMG_MSG_TEMPO_EXIBICAO")) { dt.Columns["ORGIMG_MSG_TEMPO_EXIBICAO"].ColumnName = "TempoMensagemExibicao"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioIns"; }
            if (dt.Columns.Contains("ORGIMG_DH_GERACAO")) { dt.Columns["ORGIMG_DH_GERACAO"].ColumnName = "DataHoraGeracao"; }
        }
        #endregion Métodos
    }
}
