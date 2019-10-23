#region Using
using CaseBusiness.Framework.BancoDados;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
#endregion Using;

namespace CaseBusiness.CC.Global
{
    public class Organizacao : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Global_Organizacao_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private String _nome = String.Empty;
        private String _sigla = String.Empty;
        private StatusOrganizacao.enumStatus _statusOrganizacao;
        //private CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem _codigoEstrategiaAnaliseChecagem = ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem.EMPTY;
        private Int32 _tempoTransacaoExpiracao = Int32.MinValue;
        private Int32 _tempoTransacaoExibicao = Int32.MinValue;
        private Int32 _qtdeTransacoesHistorico = Int32.MinValue;
        private Int32 _tempoMensagemExibicao = Int32.MinValue;
        private Int32 _filaCartaoQtdeAlertasFaixa1 = Int32.MinValue;
        private Int32 _filaCartaoQtdeAlertasFaixa2 = Int32.MinValue;
        //private CaseBusiness.CC.Concentrador.Cartao.enmDigitosQtde _cartaoNumeroDigitosQtde = Concentrador.Cartao.enmDigitosQtde.EMPTY;

        //private Int32 _idOrganizacaoImagemParametro_Ultima = Int32.MinValue;

        private DataTable _dtRestricoesExclusao;

        public const String kCodigoOrganizacao_Formato = @"0000";
        public const Int32 kTempoTransacaoExpiracaoMinima = 1;
        public const Int32 kTempoTransacaoExpiracaoMaxima = 8760;
        public const Int32 kTempoTransacaoExibicaoMinima = 1;
        public const Int32 kTempoTransacaoExibicaoMaxima = 17520;
        public const Int32 kQtdeTransacoesHistoricoMinima = 1;
        public const Int32 kQtdeTransacoesHistoricoMaxima = 100;
        public const Int32 kTempoMensagemExibicaoMinima = 1;
        public const Int32 kTempoMensagemExibicaoMaxima = 9999;
        public const Int32 kFilaCartaoQtdeAlertasFaixa1Default = 5;
        public const Int32 kFilaCartaoQtdeAlertasFaixa2Default = 10;

        #endregion Atributos

        #region Propriedades
        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public String Sigla
        {
            get { return _sigla; }
            set { _sigla = value; }
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

        public Int32 FilaCartaoQtdeAlertasFaixa1
        {
            get { return _filaCartaoQtdeAlertasFaixa1; }
            set { _filaCartaoQtdeAlertasFaixa1 = value; }
        }

        public Int32 FilaCartaoQtdeAlertasFaixa2
        {
            get { return _filaCartaoQtdeAlertasFaixa2; }
            set { _filaCartaoQtdeAlertasFaixa2 = value; }
        }

        //public CaseBusiness.CC.Concentrador.Cartao.enmDigitosQtde CartaoNumeroDigitosQtde
        //{
        //    get { return _cartaoNumeroDigitosQtde; }
        //    set { _cartaoNumeroDigitosQtde = value; }
        //}

        //public Int32 IdOrganizacaoImagemParametro_Ultima
        //{
        //    get { return _idOrganizacaoImagemParametro_Ultima; }
        //    set { _idOrganizacaoImagemParametro_Ultima = value; }
        //}

        public String TextoFormatado_CodigoNome
        {
            get
            {
                if (!IsLoaded) { return String.Empty; }

                return FormatarCodigoOrganizacao(CodigoOrganizacao)
                       + " - "
                       + Nome.Trim();
            }
        }

        public String TextoFormatado_CodigoSigla
        {
            get
            {
                if (!IsLoaded) { return String.Empty; }

                return FormatarCodigoOrganizacao(CodigoOrganizacao)
                       + " - "
                       + Sigla.Trim();
            }
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

                    //
                    // Se nao houver Restricao, ainda será necessário checar se a 
                    //futura exclusao da Fila Padrao poderá ser efetivada sem erro.
                    //
                    if (_dtRestricoesExclusao == null || _dtRestricoesExclusao.Rows.Count == 0)
                    {
                        //CaseBusiness.CC.ALM.Fila busFila = new ALM.Fila(CodigoOrganizacao,
                        //                                                CaseBusiness.CC.ALM.Fila.ID_FILA_PADRAO,
                        //                                                UsuarioManutencao.ID);

                        //_dtRestricoesExclusao = busFila.RestricoesExclusao;

                        //busFila = null;
                    }
                }

                return _dtRestricoesExclusao;
            }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Organizacao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Organizacao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Organizacao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Organizacao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Organizacao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Organizacao(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Organizacao e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Organizacao(Int32 codigoOrganizacao,
                           Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(codigoOrganizacao);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Inclui uma Organização
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="nome">Nome da Organização</param>
        /// <param name="sigla">Sigla da Organização</param>
        /// <param name="statusOrganizacao">Status da Organização</param>
        /// <param name="codigoEstrategiaAnaliseChecagem">Código da Estratégia de Checagem</param>
        /// <param name="tempoTransacaoExpiracao">Tempo de Expiração de uma Transação nas Filas</param>
        /// <param name="tempoTransacaoExibicao">Tempo que a Transação, após Expirada, será mostrada na respectiva Fila classificada</param>
        /// <param name="qtdeTransacoesHistorico">Qtde de Transações a adicionar como Histórico no Envelope de Análise</param>
        /// <param name="tempoMensagemExibicao">Qtde de Tempo (em minutos) para Exibir a mensagem (SMS)</param>
        /// <param name="filaCartaoQtdeAlertasFaixa1">Qtde de Alertas de um Cartão em uma Fila - Faixa 1</param>
        /// <param name="filaCartaoQtdeAlertasFaixa2">Qtde de Alertas de um Cartão em uma Fila - Faixa 2</param>
        public void Incluir(Int32 codigoOrganizacao,
                            String nome,
                            String sigla,
                            StatusOrganizacao.enumStatus statusOrganizacao,
                            //CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem codigoEstrategiaAnaliseChecagem,
                            Int32 tempoTransacaoExpiracao,
                            Int32 tempoTransacaoExibicao,
                            Int32 qtdeTransacoesHistorico,
                            Int32 tempoMensagemExibicao,
                            Int32 filaCartaoQtdeAlertasFaixa1,
                            Int32 filaCartaoQtdeAlertasFaixa2)
        {
            try
            {
                DateTime dataImagem = DateTime.Now;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organizacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@ORG_NM", nome.Trim());
                acessoDadosBase.AddParameter("@ORG_SG", sigla.Trim());
                acessoDadosBase.AddParameter("@ORG_ST", CaseBusiness.CC.Global.StatusOrganizacao.ObterDBValue(statusOrganizacao));
                acessoDadosBase.AddParameter("@ORG_ESACHEC_CD", "XPTO");
                acessoDadosBase.AddParameter("@ORG_TRS_TEMPO_EXPIRACAO", tempoTransacaoExpiracao);
                acessoDadosBase.AddParameter("@ORG_TRS_TEMPO_EXIBICAO", tempoTransacaoExibicao);
                acessoDadosBase.AddParameter("@ORG_QTDE_TRS_HIST", qtdeTransacoesHistorico);
                acessoDadosBase.AddParameter("@ORG_MSG_TEMPO_EXIBICAO", tempoMensagemExibicao);
                acessoDadosBase.AddParameter("@ORG_FLA_CRT_QTDE_FAIXA1", filaCartaoQtdeAlertasFaixa1);
                acessoDadosBase.AddParameter("@ORG_FLA_CRT_QTDE_FAIXA2", filaCartaoQtdeAlertasFaixa2);
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@ORG_DH_USUARIO_INS", dataImagem);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prORG_INS");

                OrganizacaoImagemParametro obj_OrganizacaoImgParametro = new OrganizacaoImagemParametro(UsuarioManutencao.ID);
                //Int32 idImagemGeradaCodigoOrganizacao = Int32.MinValue;
                //obj_OrganizacaoImgParametro.Gerar(codigoOrganizacao, statusOrganizacao, codigoEstrategiaAnaliseChecagem, tempoTransacaoExpiracao, tempoTransacaoExibicao,
                //                                  qtdeTransacoesHistorico, tempoMensagemExibicao, dataImagem, ref idImagemGeradaCodigoOrganizacao);


                //
                // Para toda Organização criada será criada uma Fila Padrão.
                //
                //CaseBusiness.CC.ALM.Fila objFila = new CaseBusiness.CC.ALM.Fila(UsuarioManutencao.ID);
                //Int32 idFila = Int32.MinValue;
                //objFila.IncluirFilaPadrao(codigoOrganizacao, ref idFila);
                //-------------------------------------------------------------------------------------------------


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
        /// Altera uma Organização
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="nome">Nome da Organização</param>
        /// <param name="sigla">Sigla da Organização</param>
        /// <param name="statusOrganizacao">Status da Organização</param>
        /// <param name="codigoEstrategiaAnaliseChecagem">Código da Estratégia de Checagem</param>
        /// <param name="tempoTransacaoExpiracao">Tempo de Expiração de uma Transação nas Filas</param>
        /// <param name="tempoTransacaoExibicao">Tempo que a Transação, após Expirada, será mostrada na respectiva Fila classificada</param>
        /// <param name="qtdeTransacoesHistorico">Qtde de Transações a adicionar como Histórico no Envelope de Análise</param>
        /// <param name="tempoMensagemExibicao">Qtde de Tempo (em minutos) para Exibir a mensagem (SMS)</param>
        /// <param name="filaCartaoQtdeAlertasFaixa1">Qtde de Alertas de um Cartão em uma Fila - Faixa 1</param>
        /// <param name="filaCartaoQtdeAlertasFaixa2">Qtde de Alertas de um Cartão em uma Fila - Faixa 2</param>
        public void Alterar(Int32 codigoOrganizacao,
                            String nome,
                            String sigla,
                            StatusOrganizacao.enumStatus statusOrganizacao,
                            //CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem codigoEstrategiaAnaliseChecagem,
                            Int32 tempoTransacaoExpiracao,
                            Int32 tempoTransacaoExibicao,
                            Int32 qtdeTransacoesHistorico,
                            Int32 tempoMensagemExibicao,
                            Int32 filaCartaoQtdeAlertasFaixa1,
                            Int32 filaCartaoQtdeAlertasFaixa2)
        {
            try
            {
                DateTime dataImagem = DateTime.Now;
                //        Int32 cdorganizacaoImg = Int32.MinValue;
                //        CaseBusiness.CC.ALM.OrganizacaoImagem objBUS_OrganizacaoImagem = new CaseBusiness.CC.ALM.OrganizacaoImagem(UsuarioManutencao.ID); 

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organizacao está operando em Modo Entidade Only"); }

                Boolean gerarImagem = false;

                CaseBusiness.CC.Global.Organizacao objBUS_OrganizacaoPreenchida = new CaseBusiness.CC.Global.Organizacao(codigoOrganizacao, UsuarioManutencao.ID);


                if (!objBUS_OrganizacaoPreenchida.StatusOrganizacao.Equals(statusOrganizacao) ||
                    //!objBUS_OrganizacaoPreenchida.CodigoEstrategiaAnaliseChecagem.Equals(codigoEstrategiaAnaliseChecagem) ||
                    !objBUS_OrganizacaoPreenchida.TempoTransacaoExpiracao.Equals(tempoTransacaoExpiracao) ||
                    !objBUS_OrganizacaoPreenchida.TempoTransacaoExibicao.Equals(TempoTransacaoExibicao) ||
                    !objBUS_OrganizacaoPreenchida.QtdeTransacoesHistorico.Equals(qtdeTransacoesHistorico) ||
                    !objBUS_OrganizacaoPreenchida.TempoMensagemExibicao.Equals(tempoMensagemExibicao))
                {
                    gerarImagem = true;
                }


                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@ORG_NM", nome.Trim());
                acessoDadosBase.AddParameter("@ORG_SG", sigla.Trim());
                acessoDadosBase.AddParameter("@ORG_ST", CaseBusiness.CC.Global.StatusOrganizacao.ObterDBValue(statusOrganizacao));
                acessoDadosBase.AddParameter("@ORG_ESACHEC_CD", "XPTO");
                acessoDadosBase.AddParameter("@ORG_TRS_TEMPO_EXPIRACAO", tempoTransacaoExpiracao);
                acessoDadosBase.AddParameter("@ORG_TRS_TEMPO_EXIBICAO", tempoTransacaoExibicao);
                acessoDadosBase.AddParameter("@ORG_QTDE_TRS_HIST", qtdeTransacoesHistorico);
                acessoDadosBase.AddParameter("@ORG_MSG_TEMPO_EXIBICAO", tempoMensagemExibicao);
                acessoDadosBase.AddParameter("@ORG_FLA_CRT_QTDE_FAIXA1", filaCartaoQtdeAlertasFaixa1);
                acessoDadosBase.AddParameter("@ORG_FLA_CRT_QTDE_FAIXA2", filaCartaoQtdeAlertasFaixa2);
                acessoDadosBase.AddParameter("@USU_ID_UPD", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@ORG_DH_USUARIO_UPD", dataImagem);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prORG_UPD");


                if (gerarImagem)
                {
                    OrganizacaoImagemParametro obj_OrganizacaoImgParametro = new OrganizacaoImagemParametro(UsuarioManutencao.ID);
                    //Int32 idImagemGeradaCodigoOrganizacao = Int32.MinValue;
                    //obj_OrganizacaoImgParametro.Gerar(codigoOrganizacao, statusOrganizacao, codigoEstrategiaAnaliseChecagem, tempoTransacaoExpiracao, tempoTransacaoExibicao,
                    //                                  qtdeTransacoesHistorico, tempoMensagemExibicao, dataImagem, ref idImagemGeradaCodigoOrganizacao);
                }

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
        /// Exclui uma Organização e todas suas imagens parâmetros
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <returns>OK = Exclusão Permitida e Efetuada
        ///            Mensagem = Motivo que não permitiu a Exclusão</returns>
        public void Excluir(Int32 codigoOrganizacao)
        {

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organizacao está operando em Modo Entidade Only"); }


                //
                // Excluir Fila Padrão sempre que a organização for excluída. 
                //
                //CaseBusiness.CC.ALM.Fila objFila = new CaseBusiness.CC.ALM.Fila(UsuarioManutencao.ID);
                //Int32 idFila = Int32.MinValue;
                //objFila.Excluir(codigoOrganizacao, CaseBusiness.CC.ALM.Fila.ID_FILA_PADRAO);


                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prORG_DEL");

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
        /// Buscar Organização
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="nomeOrganizacao">Nome da Organização</param>
        public DataTable Buscar(Int32 codigoOrganizacao,
                                String nomeOrganizacao,
                                String siglaOrganizacao,
                                CaseBusiness.CC.Global.StatusOrganizacao.enumStatus statusOrganizacao)//,
                                //CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.enumEstrategiaAnaliseChecagem estrategiaChecagem)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organização está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@ORG_NM", nomeOrganizacao.Trim());
                acessoDadosBase.AddParameter("@ORG_SG", siglaOrganizacao.Trim());
                acessoDadosBase.AddParameter("@ORG_ST", CaseBusiness.CC.Global.StatusOrganizacao.ObterDBValue(statusOrganizacao));
                //acessoDadosBase.AddParameter("@ORG_ESACHEC_CD", CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.ObterDBValue(estrategiaChecagem));

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prORG_SEL_BUSCAR").Tables[0];
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
        /// Consulta uma Organizacao
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        private void Consultar(Int32 codigoOrganizacao)
        {
            try
            {
                //DataTable dtOriginal = null;
                DataView dv = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organizacao está operando em Modo Entidade Only"); }

                // Fill Object
                __blnIsLoaded = false;


                //// O uso do dataview na seleção de uma Organizacao foi desativada pois,
                //// após um Rowfilter, numa SEGUNDA utilizacao por outra classe, o MemoryCache fica com a seleção desta Organização vinculada (pasmen!)
                //// Então, é necessária a criação de uma "deep" cópia antes da seleção da desejada Organizacao
                //// E, como o processo de cópia faria um laço com TODOS os registros, este foi utilizado pra localizar a Organização.
                //dtOriginal = Listar();

                //DataTable dtClone = dtOriginal.Clone();

                //foreach (DataRow dr in dtOriginal.Rows)
                //{
                //    //    clone.ImportRow(dr);

                //    if (Convert.ToInt32(dr["CodigoOrganizacao"]) == codigoOrganizacao)
                //    {
                //        CodigoOrganizacao = Convert.ToInt32(dr["CodigoOrganizacao"]);
                //        Nome = Convert.ToString(dr["OrganizacaoNome"]);
                //        Sigla = Convert.ToString(dr["OrganizacaoSigla"]);
                //        StatusOrganizacao = CaseBusiness.CC.Global.StatusOrganizacao.ObterEnum(Convert.ToString(dr["StatusOrganizacao"]));
                //        CodigoEstrategiaAnaliseChecagem = CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.ObterEnum(Convert.ToString(dr["CodigoEstrategiaAnaliseChecagem"]));
                //        TempoTransacaoExpiracao = Convert.ToInt32(dr["TempoTransacaoExpiracao"]);
                //        QtdeTransacoesHistorico = Convert.ToInt32(dr["QtdeTransacoesHistorico"]);
                //        IdOrganizacaoImagemParametro_Ultima = Convert.ToInt32(dr["IdOrganizacaoImagemParametro_Ultima"]);

                //        __blnIsLoaded = true;

                //        dtOriginal = null;
                //        dtClone = null;

                //        return;
                //    }
                //}

                dv = Listar().DefaultView;
                dv.RowFilter = "CodigoOrganizacao = " + codigoOrganizacao.ToString();

                if (dv.Count > 0)
                {
                    CodigoOrganizacao = Convert.ToInt32(dv[0]["CodigoOrganizacao"]);
                    Nome = Convert.ToString(dv[0]["OrganizacaoNome"]);
                    Sigla = Convert.ToString(dv[0]["OrganizacaoSigla"]);
                    StatusOrganizacao = CaseBusiness.CC.Global.StatusOrganizacao.ObterEnum(Convert.ToString(dv[0]["StatusOrganizacao"]));
                    //CodigoEstrategiaAnaliseChecagem = CaseBusiness.CC.ALM.EstrategiaAnaliseChecagem.ObterEnum(Convert.ToString(dv[0]["CodigoEstrategiaAnaliseChecagem"]));
                    TempoTransacaoExpiracao = Convert.ToInt32(dv[0]["TempoTransacaoExpiracao"]);
                    TempoTransacaoExibicao = Convert.ToInt32(dv[0]["TempoTransacaoExibicao"]);
                    QtdeTransacoesHistorico = Convert.ToInt32(dv[0]["QtdeTransacoesHistorico"]);
                    TempoMensagemExibicao = Convert.ToInt32(dv[0]["TempoMensagemExibicao"]);
                    FilaCartaoQtdeAlertasFaixa1 = Convert.ToInt32(dv[0]["FilaCartaoQtdeAlertasFaixa1"]);
                    FilaCartaoQtdeAlertasFaixa2 = Convert.ToInt32(dv[0]["FilaCartaoQtdeAlertasFaixa2"]);
                    //IdOrganizacaoImagemParametro_Ultima = Convert.ToInt32(dv[0]["IdOrganizacaoImagemParametro_Ultima"]);

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
        /// Listar as Organizações
        /// </summary>
        public DataTable Listar()
        {
            DataTable dtOriginal = null;
            DataTable dtDeepCopy = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organizacao está operando em Modo Entidade Only"); }

                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dtOriginal = MemoryCache.Default[kCacheKey] as DataTable;
                }
                else
                {
                    dtOriginal = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                                "prORG_SEL_LISTAR").Tables[0];

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
        /// Obtem as Restrições de Exclusão da Organizacao carregada
        /// </summary>
        private void ObterRestricoesExclusao()
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organização está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", CodigoOrganizacao);

                _dtRestricoesExclusao = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prORG_SEL_RESTRIC_DEL").Tables[0];

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
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "OrganizacaoNome"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "OrganizacaoCodigoNome"; }
            if (dt.Columns.Contains("ORG_SG")) { dt.Columns["ORG_SG"].ColumnName = "OrganizacaoSigla"; }
            if (dt.Columns.Contains("ORG_ST")) { dt.Columns["ORG_ST"].ColumnName = "StatusOrganizacao"; }
            if (dt.Columns.Contains("ORG_ESACHEC_CD")) { dt.Columns["ORG_ESACHEC_CD"].ColumnName = "CodigoEstrategiaAnaliseChecagem"; }
            if (dt.Columns.Contains("ORG_TRS_TEMPO_EXPIRACAO")) { dt.Columns["ORG_TRS_TEMPO_EXPIRACAO"].ColumnName = "TempoTransacaoExpiracao"; }
            if (dt.Columns.Contains("ORG_TRS_TEMPO_EXIBICAO")) { dt.Columns["ORG_TRS_TEMPO_EXIBICAO"].ColumnName = "TempoTransacaoExibicao"; }
            if (dt.Columns.Contains("ORG_QTDE_TRS_HIST")) { dt.Columns["ORG_QTDE_TRS_HIST"].ColumnName = "QtdeTransacoesHistorico"; }
            if (dt.Columns.Contains("ORG_MSG_TEMPO_EXIBICAO")) { dt.Columns["ORG_MSG_TEMPO_EXIBICAO"].ColumnName = "TempoMensagemExibicao"; }

            if (dt.Columns.Contains("ORG_FLA_CRT_QTDE_FAIXA1")) { dt.Columns["ORG_FLA_CRT_QTDE_FAIXA1"].ColumnName = "FilaCartaoQtdeAlertasFaixa1"; }
            if (dt.Columns.Contains("ORG_FLA_CRT_QTDE_FAIXA2")) { dt.Columns["ORG_FLA_CRT_QTDE_FAIXA2"].ColumnName = "FilaCartaoQtdeAlertasFaixa2"; }

            //if (dt.Columns.Contains("MAX_OGIMGPARM_ID")) { dt.Columns["MAX_OGIMGPARM_ID"].ColumnName = "IdOrganizacaoImagemParametro_Ultima"; }

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }


        /// <summary>
        /// Formata um Código de Organização
        /// </summary>
        /// <param name="codigoOrganizacao">Código de Organização</param>
        public static String FormatarCodigoOrganizacao(Int32 codigoOrganizacao)
        {
            return String.Format("{0:" + kCodigoOrganizacao_Formato + "}", codigoOrganizacao);
        }
        #endregion Métodos
    }
}
