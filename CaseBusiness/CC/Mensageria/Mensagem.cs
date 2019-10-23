using System;
using System.Data;
using System.Collections.Generic;
using CaseBusiness.Framework.BancoDados;
using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.Caching;

namespace CaseBusiness.CC.Mensageria
{
    public class Mensagem : BusinessBase
    {
        #region MemoryCache
        public static String kCacheKeyMensagemCampanhaLoteTipoDestinatario = "CaseBusiness_MensagemCampanhaLoteTipoDestinatario_Collection";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 120;
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 10;
        #endregion MemoryCache

        #region Atributos
        private Int32 _idCampanha = Int32.MinValue;
        private Int32 _numeroLote = Int32.MinValue;
        private Int32 _idFornecedora = Int32.MinValue;
        private Int32 _codigoConfiguracaoComunicacao = Int32.MinValue;
        private Int32 _codigoGrupoTeste = Int32.MinValue;
        private DateTime _dataHoraLiberadoEnvio = DateTime.MinValue;
        private DateTime _dataHoraInclusao = DateTime.MinValue;
        private DateTime _dataHoraStatusMensagem = DateTime.MinValue;
        private String _destinatario = String.Empty;
        private String _idMensagemSMS = String.Empty;
        private String _idCliente = "";
        private String _idMensagemSMSExterno = String.Empty;
        private String _nomeCliente = String.Empty;
        private String _numeroCPF = String.Empty;
        private String _tipoDestinatario = String.Empty;
        private String _textoMensagem = String.Empty;
        private String _statusMensagem = String.Empty;
        private Int32 _idUsuarioIns = Int32.MinValue;
        private Int32 _idUsuarioUpd = Int32.MinValue;

        private Int32 _codigoOrganizacao = Int32.MinValue;
        private Int32 _idCanal = Int32.MinValue;
        private String _nomeCanal = String.Empty;
        private String _descricaoStatusMensagem = String.Empty;
        private DateTime _dhUsarioIns = DateTime.MinValue;
        private String _nomeUsuarioIns = String.Empty;
        private String _nomeGrupoTeste = String.Empty;
        private DateTime? _dataHoraMensagem = null;
        private DateTime? _dataHoraResposta = null;
        private String _statusResposta = String.Empty;
        private String _descricaoMensagemLog = String.Empty;

        //private Int32 _idMensagemConfiguracao = Int32.MinValue;         
        //private DateTime _dhUsuarioUpd = DateTime.MinValue; // MGMSG_DH_USU_UPD
        //private String _codigoSegmento = String.Empty;  // MGMSGCMPLTRS_CD_SEGMENTO
        //private DataTable _dtRestricoesExclusao = null;
        #endregion Atributos

        #region Propriedades
        public Int32 IdCampanha
        {
            get { return _idCampanha; }
            set { _idCampanha = value; }
        }

        public Int32 NumeroLote
        {
            get { return _numeroLote; }
            set { _numeroLote = value; }
        }

        public Int32 IdFornecedora
        {
            get { return _idFornecedora; }
            set { _idFornecedora = value; }
        }

        public Int32 CodigoConfiguracaoComunicacao
        {
            get { return _codigoConfiguracaoComunicacao; }
            set { _codigoConfiguracaoComunicacao = value; }
        }

        public Int32 CodigoGrupoTeste
        {
            get { return _codigoGrupoTeste; }
            set { _codigoGrupoTeste = value; }
        }

        public DateTime DataHoraLiberadoEnvio
        {
            get { return _dataHoraLiberadoEnvio; }
            set { _dataHoraLiberadoEnvio = value; }
        }

        public DateTime DataHoraInclusao
        {
            get { return _dataHoraInclusao; }
            set { _dataHoraInclusao = value; }
        }

        public DateTime DataHoraStatusMensagem
        {
            get { return _dataHoraStatusMensagem; }
            set { _dataHoraStatusMensagem = value; }
        }

        public String Destinatario
        {
            get { return _destinatario; }
            set { _destinatario = value; }
        }

        public String IdMensagemSMS
        {
            get { return _idMensagemSMS; }
            set { _idMensagemSMS = value; }
        }

        public String IdCliente
        {
            get { return _idCliente; }
            set { _idCliente = value; }
        }

        public String IdMensagemSMSExterno
        {
            get { return _idMensagemSMSExterno; }
            set { _idMensagemSMSExterno = value; }
        }

        public String NomeCliente
        {
            get { return _nomeCliente; }
            set { _nomeCliente = value; }
        }

        public String NumeroCPF
        {
            get { return _numeroCPF; }
            set { _numeroCPF = value; }
        }

        public String TipoDestinatario
        {
            get { return _tipoDestinatario; }
            set { _tipoDestinatario = value; }
        }

        public String TextoMensagem
        {
            get { return _textoMensagem; }
            set { _textoMensagem = value; }
        }

        public String StatusMensagem
        {
            get { return _statusMensagem; }
            set { _statusMensagem = value; }
        }

        public Int32 IdUsuarioIns
        {
            get { return _idUsuarioIns; }
            set { _idUsuarioIns = value; }
        }

        public Int32 IdUsuarioUpd
        {
            get { return _idUsuarioUpd; }
            set { _idUsuarioUpd = value; }
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

        public String NomeCanal
        {
            get { return _nomeCanal; }
            set { _nomeCanal = value; }
        }

        public String DescricaoStatusMensagem
        {
            get { return _descricaoStatusMensagem; }
            set { _descricaoStatusMensagem = value; }
        }

        public DateTime DhUsarioIns
        {
            get { return _dhUsarioIns; }
            set { _dhUsarioIns = value; }
        }

        public String NomeUsuarioIns
        {
            get { return _nomeUsuarioIns; }
            set { _nomeUsuarioIns = value; }
        }

        public String NomeGrupoTeste
        {
            get { return _nomeGrupoTeste; }
            set { _nomeGrupoTeste = value; }
        }

        public DateTime? DataHoraMensagem
        {
            get { return _dataHoraMensagem; }
            set { _dataHoraMensagem = value; }
        }

        public DateTime? DataHoraResposta
        {
            get { return _dataHoraResposta; }
            set { _dataHoraResposta = value; }
        }

        public String StatusResposta
        {
            get { return _statusResposta; }
            set { _statusResposta = value; }
        }

        public String DescricaoMensagemLog
        {
            get { return _descricaoMensagemLog; }
            set { _descricaoMensagemLog = value; }
        }

        //public Int32 IdMensagemConfiguracao
        //{
        //    get { return _idMensagemConfiguracao; }
        //    set { _idMensagemConfiguracao = value; }
        //}

        //public Int32 CodigoGrupoTeste
        //{
        //    get { return _codigoGrupoTeste; }
        //    set { _codigoGrupoTeste = value; }
        //}

        //public DateTime DhUsuarioUpd
        //{
        //    get { return _dhUsuarioUpd; }
        //    set { _dhUsuarioUpd = value; }
        //}

        //public String CodigoSegmento
        //{
        //    get { return _codigoSegmento; }
        //    set { _codigoSegmento = value; }
        //}

        //public Boolean ExclusaoPermitida
        //{
        //    get
        //    {
        //        if (!IsLoaded)
        //        {
        //            _dtRestricoesExclusao = null;
        //            return false;
        //        }

        //        if (RestricoesExclusao == null)
        //        { return false; }
        //        else
        //        {
        //            if (_dtRestricoesExclusao.Rows.Count <= 0)
        //            { return true; }
        //            else
        //            { return false; }
        //        }
        //    }
        //}

        //public DataTable RestricoesExclusao
        //{
        //    get
        //    {
        //        if (_dtRestricoesExclusao == null)
        //        {
        //            _dtRestricoesExclusao = new DataTable();
        //            _dtRestricoesExclusao = ObterRestricoesExclusao();
        //        }

        //        return _dtRestricoesExclusao;
        //    }
        //}
        #endregion Propriedades

        #region Construtores

        public Mensagem()
        {
        }

        /// <summary>
        /// Construtor classe Mensagem - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Mensagem(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Mensagem
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Mensagem(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Mensagem utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Mensagem(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Mensagem e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoGrupo">Código da Mensagem</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Mensagem(String idMensagemSMS,
                        Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idMensagemSMS);
        }

        //TODO: revisar
        ///// <summary>
        ///// Construtor classe Mensagem e já preenche as propriedades com os dados da chave informada
        ///// <param name="statusTransacao">Status da Transação</param>
        ///// </summary>
        //public Mensagem(String statusTransacao,
        //                Int32 idUsuarioManutencao)
        //    : this(idUsuarioManutencao)
        //{
        //    Consultar(statusTransacao);
        //}

        #endregion Construtores

        #region Métodos

        public String RetornarCodigoEvento(String codRetorno)
        {
            switch (codRetorno.Trim().ToUpper())
            {
                case "E0":
                    return "ERRO_ENVIO";
                case "E6":
                    return "ERRO_ENTREGA";
                case "E1":
                case "E3":
                case "E4":
                case "E7":
                case "DP":
                    return "REJEITADO";
                case "OK":
                case "OP":
                    return "ENVIADO";
                case "CL":
                    return "RECEBIDO";
                case "":
                    return "RESPONDIDO";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Busca dados da mensagem enviada
        /// </summary>
        /// <param name="idMensagemSMS">Id da mensagem</param>
        /// <returns></returns>
        public DataTable Buscar(String idMensagemSMS)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagemSMS);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "PRMGSMS_SEL_BUSCARMENSAGEM").Tables[0];

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



        public DataTable BuscarCampanhaLoteTipoDestinatario(Int32 idCampanha,
                                Int32 nrLote,
                                String destinatarioTipo)
        {
            DataTable dt = null;

            try
            {

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@CMP_NR_LOTE", nrLote);
                acessoDadosBase.AddParameter("@MGSMS_TP_DEST", destinatarioTipo);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGSMS_SEL_BUSCAR_TP_DEST").Tables[0];

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

        #endregion Métodos

        /// <summary>
        /// Consulta uma Mensagem
        /// </summary>
        /// <param name="idMensagemSMS">Id da mensagem na qual os dados da transação está associado</param>
        private void Consultar(String idMensagemSMS)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                DataTable dt;

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagemSMS);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGSMS_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                if (dt.Rows.Count > 0)
                {
                    CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                    IdCanal = Convert.ToInt32(dt.Rows[0]["COMCNL_ID"]);
                    NomeCanal = dt.Rows[0]["COMCNL_NM"].ToString();
                    StatusMensagem = dt.Rows[0]["MGMSGST_ST"].ToString();
                    DescricaoStatusMensagem = dt.Rows[0]["MGMSGST_DS"].ToString();
                    idMensagemSMS = dt.Rows[0]["MGSMS_ID"].ToString();
                    ////Descriptografar o Número do CPF
                    NumeroCPF = String.IsNullOrEmpty(pci.Decodificar(Convert.ToString(dt.Rows[0]["MGSMS_NR_CPF"]))) ? "!!ERRO CRYPT!! " + dt.Rows[0]["MGSMS_NR_CPF"].ToString() : pci.Decodificar(dt.Rows[0]["MGSMS_NR_CPF"].ToString());
                    TextoMensagem = dt.Rows[0]["MGSMS_TX_MENSAGEM"].ToString();
                    CodigoGrupoTeste = dt.Rows[0]["MGGRPTST_ID"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["MGGRPTST_ID"]);
                    NomeGrupoTeste = dt.Rows[0]["MGGRPTST_NM"] == DBNull.Value ? "" : dt.Rows[0]["MGGRPTST_NM"].ToString();
                    TipoDestinatario = dt.Rows[0]["MGSMS_TP_DEST"].ToString();
                    Destinatario = dt.Rows[0]["MGSMS_DS_DEST"].ToString();

                    if (dt.Rows[0]["MGSMSLOG_DH"] != DBNull.Value)
                        DhUsarioIns = Convert.ToDateTime(dt.Rows[0]["MGSMSLOG_DH"]);

                    if (dt.Rows[0]["USU_ID_INS"] != DBNull.Value)
                        IdUsuarioIns = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);

                    if (dt.Rows[0]["USU_NM"] != DBNull.Value)
                        NomeUsuarioIns = dt.Rows[0]["USU_NM"].ToString();

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
        /// Recebe mensagem com a(s) TAG(s)
        /// </summary>
        /// <param name="strMensagem">Mensagem com a TAG</param>
        /// <param name="numeroCPF">Número do CPF do Cliente</param>
        /// <param name="nomeCliente">Nome do Cliente</param>
        /// <returns>Retorna Mensagem formatada conforme TAG(s) recebidas</returns>
        public String GerarTextoMensagem(String strMensagem,
                                         CaseBusiness.CC.Mensageria.MensagemRequisicao mensagem)
        {
            String textoMensagem = "";
            int pos;

            try
            {
                var msg = JsonConvert.DeserializeObject<System.Dynamic.ExpandoObject>(mensagem.TAGS.ToString()) as dynamic;

                foreach (var itemTags in msg)
                {
                    if (itemTags.Key.ToLower() == "nomecliente")
                    {
                        String nomeCliente = itemTags.Value.ToString();

                        //'NOME DO CLIENTE - Máximo 10 caracteres
                        if (!String.IsNullOrEmpty(nomeCliente.Trim()))
                        {
                            pos = nomeCliente.IndexOf(" ", 0);

                            if (pos > 10)
                                nomeCliente = nomeCliente.Substring(0, 10);
                            else
                                if (pos > 0)
                                nomeCliente = nomeCliente.Substring(0, pos);
                        }

                        // Monta mensagem
                        textoMensagem = strMensagem.Replace("##nomecliente##", nomeCliente);
                    }
                }
            }
            catch (Exception ex)
            {
                textoMensagem = "";
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            }

            return textoMensagem;
        }


        public String ChecarEnvioSeedList(Int32 idCampanha, Int32 nrLote, Int32 idSeedList)
        {
            String destinarioEnvio = "";
            String kCacheKeySeedListCampanhaLote = "Cache_" + idCampanha.ToString() + nrLote.ToString();
            Boolean? seedListCompleta = true;

            try
            {
                if (!MemoryCache.Default.Contains(kCacheKeySeedListCampanhaLote))
                    seedListCompleta = false;
                else
                    seedListCompleta = MemoryCache.Default[kCacheKeySeedListCampanhaLote] as Boolean?;

                if (seedListCompleta == false)
                {
                    DataTable dtGrupoSeed = new CaseBusiness.CC.Mensageria.GrupoTesteDestinatario(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Consultar(idSeedList);

                    if (dtGrupoSeed != null && dtGrupoSeed.Rows.Count > 0)
                    {
                        DataTable dtCmpLoteSeed = new Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).BuscarCampanhaLoteTipoDestinatario(idCampanha, nrLote, "SEED");

                        if (dtCmpLoteSeed != null && dtCmpLoteSeed.Rows.Count > 0)
                        {
                            for (int sl = 0; sl < dtGrupoSeed.Rows.Count; sl++)
                            {
                                Boolean enviado = false;

                                for (int m = 0; m < dtCmpLoteSeed.Rows.Count; m++)
                                {
                                    if (dtCmpLoteSeed.Rows[m]["Destinatario"].ToString() == ("55" + dtGrupoSeed.Rows[sl]["DescricaoDestinatario"].ToString()))
                                    {
                                        enviado = true;
                                        break;
                                    }
                                }

                                if (enviado)
                                    continue;
                                else
                                {
                                    destinarioEnvio = dtGrupoSeed.Rows[sl]["DescricaoDestinatario"].ToString();
                                    break;
                                }
                            }
                        }
                        else
                            destinarioEnvio = dtGrupoSeed.Rows[0]["DescricaoDestinatario"].ToString();
                    }
                    else
                        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro,"Grupo de SEED List ID: " + idSeedList.ToString() + " não possui destinatários cadastrados", "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                }

                if (destinarioEnvio == "")
                {
                    MemoryCache.Default.Set(kCacheKeySeedListCampanhaLote, true,
                    new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(kCache_ABSOLUTEEXPIRATION_MINUTES)
                    });
                }
                else
                {
                    MemoryCache.Default.Set(kCacheKeySeedListCampanhaLote, false,
                    new CacheItemPolicy()
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(kCache_ABSOLUTEEXPIRATION_MINUTES)
                    });
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            }

            return destinarioEnvio;
        }

        public void RequisitarEnvioMensagem(CaseBusiness.CC.Mensageria.MensagemRequisicao mensagem)
        {
            String descricaoMensagemLog = String.Empty;
            DateTime dhUsuarioIns = DateTime.Now;
            String textoMensagemEnvio = "";
            String processo = String.Empty;
            String status = String.Empty;
            String destinatario = String.Empty;
            String descricaoLog = String.Empty;
            String mensagemRetorno = String.Empty;

            try
            {
                //dynamic mensagem = (System.Dynamic.ExpandoObject)((Object[])state)[0];

                DataTable dt = new CaseBusiness.CC.Mensageria.CampanhaComunicacao(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Buscar(Convert.ToInt32(mensagem.ID_CAMPANHA), Convert.ToInt32(mensagem.ID_CANAL), Convert.ToInt32(mensagem.ID_CONF_COM));

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (mensagem.FLG_TESTE == "S")
                    {
                        DataTable dtGrupoTesteDestinario = new CaseBusiness.CC.Mensageria.GrupoTesteDestinatario(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Consultar(Convert.ToInt32(dt.Rows[0]["idGrupoTeste"]));

                        if (dtGrupoTesteDestinario != null && dtGrupoTesteDestinario.Rows.Count > 0)
                        {
                            CaseBusiness.CC.Mensageria.ConfiguracaoComunicacao busConfiguracaoMensagem = new CaseBusiness.CC.Mensageria.ConfiguracaoComunicacao(Convert.ToInt32(mensagem.ID_CONF_COM), Convert.ToInt32(dt.Rows[0]["CodigoOrganizacao"]), CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao);

                            if (busConfiguracaoMensagem.CodigoConfiguracaoComunicacao == Int32.MinValue)
                                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Aviso, "Código da Configuração da Comunicação: " + mensagem.ID_CONF_COM.ToString() + " não existe.", "CaseBusiness.CC.Mensageria.Mensagem.RequisitarEnvioMensagem()", "MGCONFCOM_CD", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                            else
                            {
                                textoMensagemEnvio = GerarTextoMensagem(busConfiguracaoMensagem.TextoConfiguracaoComunicacaoCliente, mensagem);

                                if (String.IsNullOrEmpty(textoMensagemEnvio))
                                {
                                    // Ocorreu erro na montagem da configuração da mensagem
                                    processo = "REST";
                                    status = "RESTD";
                                    descricaoLog = "Requisitado envio de mensagem para o destinatário " + destinatario + " porém não foi enviado porque ocorreu um erro na configuração da mensagem.";
                                }

                                for (int i = 0; i < dtGrupoTesteDestinario.Rows.Count; i++)
                                {

                                    // Insere na tabela de Mensagem para envio pelo serviço somente para o teste.
                                    processo = "REQ";
                                    status = "AGUAR";
                                    destinatario = "55" + Util.RemoveFormat(dtGrupoTesteDestinario.Rows[i]["DescricaoDestinatario"].ToString());
                                    descricaoLog = "Requisitado envio de mensagem para o destinatário " + destinatario;

                                    mensagemRetorno = AgendarEnvio(mensagem.ID_MSG.ToString(),
                                                      busConfiguracaoMensagem.CodigoConfiguracaoComunicacao,
                                                      status,
                                                      mensagem.NUM_CPF.ToString(),
                                                      textoMensagemEnvio,
                                                      destinatario,
                                                      "TESTE",
                                                      Convert.ToInt32(dtGrupoTesteDestinario.Rows[i]["IdGrupoTeste"]),
                                                      DateTime.Now,
                                                      descricaoLog,
                                                      processo,
                                                      DateTime.Now,
                                                      Convert.ToInt32(mensagem.ID_CAMPANHA),
                                                      Convert.ToInt32(mensagem.COD_LOTE),
                                                      CaseBusiness.CB.Fornecedora.TWW,
                                                      mensagem.ID_CLIENTE.ToString());
                                }
                            }
                        }
                        else
                        {
                            CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Não localizado grupo de teste: " + dt.Rows[0]["IdGrupoTesteDestinatario"].ToString() + " para a mensagem de ID_MSG: " + mensagem.ID_MSG, "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                            return;
                        }
                    }
                    else
                    {
                        _descricaoMensagemLog = new CaseBusiness.CC.Mensageria.Restricao(base.UsuarioManutencao.ID).RestricaoDefinitiva("", mensagem.DES_DESTINATARIO.ToString());

                        // Está na lista restritiva definitiva ?
                        if (!String.IsNullOrEmpty(_descricaoMensagemLog))
                        {

                            Incluir(mensagem.ID_MSG.ToString(),
                                                      Int32.MinValue,
                                                      "RESTD",
                                                      mensagem.NUM_CPF.ToString(),
                                                      "",
                                                      mensagem.DES_DESTINATARIO.ToString(),
                                                      "",
                                                      Int32.MinValue,
                                                      DateTime.Now,
                                                      DateTime.MinValue,
                                                      Convert.ToInt32(mensagem.ID_CAMPANHA),
                                                      Convert.ToInt32(mensagem.COD_LOTE),
                                                      mensagem.ID_CLIENTE);

                            new CaseBusiness.CC.Mensageria.MensagemSMSLog(base.UsuarioManutencao.ID).Incluir(mensagem.ID_MSG.ToString(), descricaoLog, "", processo, "RESTD", (Int32)CaseBusiness.CB.Fornecedora.TWW, DateTime.Now, DateTime.Now, DateTime.MinValue, "", "");

                            return;
                        }

                        CaseBusiness.CC.Mensageria.Campanha objCampanha = new Campanha(Convert.ToInt32(mensagem.ID_CAMPANHA), UsuarioManutencao.ID);

                        if (objCampanha.IsLoaded)
                        {
                            DataTable dtHorarioDisparo = new CaseBusiness.CC.Mensageria.CampanhaHorarioDisparo(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Buscar(Convert.ToInt32(mensagem.ID_CAMPANHA), Convert.ToInt32(mensagem.ID_CANAL));

                            if (dtHorarioDisparo != null && dtHorarioDisparo.Rows.Count > 0)
                            {
                                DataTable dtFeriados = new CaseBusiness.CC.Mensageria.Feriado(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Listar();
                                DateTime? dhLiberadoEnvio = null;
                                Boolean hojeFeriado = false;

                                if (dtFeriados != null && dtFeriados.Rows.Count > 0)
                                {
                                    String hoje = DateTime.Now.ToString("yyyyMMdd");

                                    foreach (DataRow r in dtFeriados.Rows)
                                    {
                                        if (Convert.ToDateTime(r["DataFeriado"]).ToString("yyyyMMdd") == hoje)
                                        {
                                            DateTime horario = Convert.ToDateTime(dtHorarioDisparo.Rows[0]["HorarioFeriado"]);
                                            dhLiberadoEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horario.Hour, horario.Minute, 0);
                                            hojeFeriado = true;
                                            break;
                                        }
                                    }
                                }

                                if (!hojeFeriado)
                                {
                                    DateTime horario = DateTime.Now;

                                    switch (DateTime.Now.DayOfWeek)
                                    {
                                        case DayOfWeek.Sunday:
                                            horario = Convert.ToDateTime(dtHorarioDisparo.Rows[0]["HorarioDomingo"]);
                                            break;
                                        case DayOfWeek.Monday:
                                            horario = Convert.ToDateTime(dtHorarioDisparo.Rows[0]["HorarioSegunda"]);
                                            break;
                                        case DayOfWeek.Tuesday:
                                            horario = Convert.ToDateTime(dtHorarioDisparo.Rows[0]["HorarioTerca"]);
                                            break;
                                        case DayOfWeek.Wednesday:
                                            horario = Convert.ToDateTime(dtHorarioDisparo.Rows[0]["HorarioQuarta"]);
                                            break;
                                        case DayOfWeek.Thursday:
                                            horario = Convert.ToDateTime(dtHorarioDisparo.Rows[0]["HorarioQuinta"]);
                                            break;
                                        case DayOfWeek.Friday:
                                            horario = Convert.ToDateTime(dtHorarioDisparo.Rows[0]["HorarioSexta"]);
                                            break;
                                        case DayOfWeek.Saturday:
                                            horario = Convert.ToDateTime(dtHorarioDisparo.Rows[0]["HorarioSabado"]);
                                            break;
                                    }


                                    dhLiberadoEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horario.Hour, horario.Minute, 0);
                                }

                                //checa se horário de liberacao do envio já foi
                                if (dhLiberadoEnvio < DateTime.Now)
                                {
                                    CaseBusiness.CC.Mensageria.Configuracao objConfiguracao = new CaseBusiness.CC.Mensageria.Configuracao(Convert.ToInt32(dt.Rows[0]["CodigoOrganizacao"]), Convert.ToInt32(mensagem.ID_CANAL), base.UsuarioManutencao.ID);

                                    var horarioAtual = TimeSpan.Parse(DateTime.Now.ToString("0:HH:mm:ss"));
                                    var horarioInicioEnvioLimite = TimeSpan.Parse(objConfiguracao.HoraEnvioInicioCliente.ToString("0:HH:mm:ss"));
                                    var horarioFimEnvioLimite = TimeSpan.Parse(objConfiguracao.HoraEnvioFimCliente.ToString("0:HH:mm:ss"));

                                    if (objCampanha.ConfiguracaoDisparo.Trim() == "MSMDIA")
                                    {
                                        //CHECA SE HORA ATUAL ESTA FORA DO HORARIO LIMITE PARA O CANAL
                                        if (horarioAtual < horarioInicioEnvioLimite || horarioAtual > horarioFimEnvioLimite)
                                        {
                                            //4.Execução da campanha finalizou as 21h, disparo agendado para 15h e ela estava cadastrada como “apenas no mesmo dia” : campanha NÃO será disparada. 
                                            //NAO PODE ENVIAR MENSAGEM E LOGA Q CAMPANHA ESTÁ FORA DO HORÁRIO
                                            return;
                                        }
                                    }
                                    else if (objCampanha.ConfiguracaoDisparo.Trim() == "SEGDIA")
                                    {
                                        //3.Execução da campanha finalizou as 19h, disparo agendado para 15h e ela estava cadastrada como “no dia seguinte” : campanha será disparada no dia seguinte às 15h.
                                        //5.Execução da campanha finalizou as 21h, disparo agendado para 15h e ela estava cadastrada como “no dia seguinte” : campanha será disparada no dia seguinte às 15h.
                                        dhLiberadoEnvio = dhLiberadoEnvio.Value.AddDays(1);
                                    }
                                }
                                //AS DUAS REGRAS ABAIXO ESTAO APLICADAS A PARTIR DESTE PONTO
                                //1.	Execução da campanha finalizou as 9h, disparo agendado para 15h: campanha será disparada as 15h.
                                //2.Execução da campanha finalizou as 19h, disparo agendado para 15h e ela estava cadastrada como “apenas no mesmo dia” : campanha será disparada as 19h. --ok

                                CaseBusiness.CC.Mensageria.ConfiguracaoComunicacao busConfiguracaoMensagem = new CaseBusiness.CC.Mensageria.ConfiguracaoComunicacao(Convert.ToInt32(mensagem.ID_CONF_COM),
                                     Convert.ToInt32(dt.Rows[0]["CodigoOrganizacao"]),
                                                                                                                                                             CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao);
                                if (busConfiguracaoMensagem.CodigoConfiguracaoComunicacao == Int32.MinValue)
                                {
                                    CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Aviso, "Código da Configuração da Comunicação: " + mensagem.ID_CONF_COM.ToString() + " não existe.", "CaseBusiness.CC.Mensageria.Mensagem.RequisitarEnvioMensagem()", "MGCONFCOM_CD", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                                }
                                else
                                {
                                    textoMensagemEnvio = GerarTextoMensagem(busConfiguracaoMensagem.TextoConfiguracaoComunicacaoCliente, mensagem);

                                    if (String.IsNullOrEmpty(textoMensagemEnvio))
                                    {
                                        // Ocorreu erro na montagem da configuração da mensagem
                                        processo = "REST";
                                        status = "RESTD";
                                        descricaoLog = "Requisitado envio de mensagem para o destinatário " + destinatario + " porém não foi enviado porque ocorreu um erro na configuração da mensagem.";
                                    }

                                    // Insere na tabela de Mensagem para envio pelo serviço somente para o teste.
                                    processo = "REQ";
                                    status = "AGUAR";
                                    destinatario = "55" + Util.RemoveFormat(mensagem.DES_DESTINATARIO.ToString());
                                    descricaoLog = "Requisitado envio de mensagem para o destinatário " + destinatario;

                                    mensagemRetorno = AgendarEnvio(mensagem.ID_MSG.ToString(),
                                                      busConfiguracaoMensagem.CodigoConfiguracaoComunicacao,
                                                      status,
                                                      mensagem.NUM_CPF.ToString(),
                                                      textoMensagemEnvio,
                                                      destinatario,
                                                      "CLIEN",
                                                      Int32.MinValue,
                                                      DateTime.Now,
                                                      descricaoLog,
                                                      processo,
                                                      dhLiberadoEnvio,
                                                      Convert.ToInt32(mensagem.ID_CAMPANHA),
                                                      Convert.ToInt32(mensagem.COD_LOTE),
                                                      CaseBusiness.CB.Fornecedora.TWW,
                                                      mensagem.ID_CLIENTE.ToString());


                                    String destinatarioSeed = ChecarEnvioSeedList(Convert.ToInt32(mensagem.ID_CAMPANHA), Convert.ToInt32(mensagem.COD_LOTE), Convert.ToInt32(dt.Rows[0]["idSeedList"]));

                                    if (destinatarioSeed != "")
                                    {
                                        mensagemRetorno = AgendarEnvio(mensagem.ID_MSG.ToString() + "_SEED",
                                           busConfiguracaoMensagem.CodigoConfiguracaoComunicacao,
                                           status,
                                           mensagem.NUM_CPF.ToString(),
                                           textoMensagemEnvio,
                                           "55" + destinatarioSeed,
                                           "SEED",
                                           Int32.MinValue,
                                           DateTime.Now,
                                           descricaoLog,
                                           processo,
                                           dhLiberadoEnvio,
                                           Convert.ToInt32(mensagem.ID_CAMPANHA),
                                           Convert.ToInt32(mensagem.COD_LOTE),
                                           CaseBusiness.CB.Fornecedora.TWW,
                                           mensagem.ID_CLIENTE.ToString());
                                    }
                                }
                            }
                            else
                                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Horario de dispardo da campanha: " + mensagem.ID_CAMPANHA.ToString() + " de canal: " + mensagem.ID_CANAL.ToString() + " não localizada para a mensagem de ID_MSG: " + mensagem.ID_MSG, "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                        }
                        else
                            CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Campanha: " + mensagem.ID_CAMPANHA.ToString() + " não localizada para a mensagem de ID_MSG: " + mensagem.ID_MSG, "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                    }
                }
                else
                {
                    CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Não localizado associação campanha comunicacao (Campanha,Canal,ConfComunicacao): " + mensagem.ID_CAMPANHA.ToString() + "," + mensagem.ID_CANAL.ToString() + "," + mensagem.ID_CONF_COM.ToString() + ") para a mensagem de ID_MSG: " + mensagem.ID_MSG, "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            }

        }


        //TODO: rever codigoGrupoTeste
        /// <summary>
        /// Requisita um envio de mensagem
        /// </summary>
        /// <param name="codigoConfiguracaoComunicacao">Código da Configuração da Comunicação</param>
        /// <param name="statusMensagem">Status da Mensagem (ver tabela de Status)</param>
        /// <param name="CPF_Descriptografado">CPF Descriptografado do Cliente</param>
        /// <param name="textoMensagem">Texto da Mensagem</param>
        /// <param name="destinatario">Celular do Destinatário</param>
        /// <param name="tipoDestinatario">Tipo de Destinatário (aceitos somente: CLIEN ou MONIT)</param>
        /// <param name="codigoGrupoTeste">Código do Grupo de Teste (Obs.: Quando for CLIEN gravar zero (0)</param> 
        /// <param name="dhInclusao">Data da Inclusão da Mensagem</param>
        /// <param name="descricaoLog">Descrição da Mensagem de Log</param>
        /// <param name="processo">Processo do Envio (ver tabela de Processo)</param>
        /// <param name="acaoEfetivada">Ação Efetivada</param>
        /// <returns>retorno = Se não conseguir requisitar retorna o motivo</returns>
        public string AgendarEnvio(String idMensagem,
                                      Int32 codigoConfiguracaoComunicacao,
                                      String statusMensagem,
                                      String CPF_Descriptografado,
                                      String textoMensagem,
                                      String destinatario,
                                      String tipoDestinatario,
                                      Int32 codigoGrupoTeste,
                                      DateTime dhInclusao,
                                      String descricaoLog,
                                      String processo,
                                      DateTime? dhLiberadoEnvio,
                                      Int32 idCampanha,
                                      Int32 codLote,
                                      CaseBusiness.CB.Fornecedora fornecedora,
                                      String idCliente)
        {
            String retorno = String.Empty;
            String mensagemRetornoErro = String.Empty;

            try
            {
                Incluir(idMensagem,
                                      codigoConfiguracaoComunicacao,
                                      statusMensagem,
                                      CPF_Descriptografado,
                                      textoMensagem,
                                      destinatario,
                                      tipoDestinatario,
                                      codigoGrupoTeste,
                                      dhInclusao,
                                      dhLiberadoEnvio,
                                      idCampanha,
                                      codLote,
                                      idCliente);

                new CaseBusiness.CC.Mensageria.MensagemSMSLog(base.UsuarioManutencao.ID).Incluir(idMensagem, descricaoLog, "", processo, statusMensagem, (Int32)fornecedora, dhInclusao, DateTime.Now, DateTime.MinValue, "", "");
            }
            catch (Exception ex)
            {
                retorno = "Ocorreu um erro ao requisitar a mensagem.";
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return retorno;
        }

        /// <summary>
        /// Inclui o log de execução de processo para uma mensagem (tbMENSAGERIA_MSG_LOG)
        /// Atualiza o Status de uma mensagem (tbMENSAGERIA_MENSAGEM)
        /// </summary>
        /// <param name="idMensagemSMS">Id da mensagem</param>
        /// <param name="idFornecedora">Quantidade de envio da mensagem</param>
        /// <param name="descricao">Detalhes sobre a execução do processo</param>
        /// <param name="respostaCliente">Resposta do Destinatário</param>
        /// <param name="processo">Sigla do processo (REQUI, ENV, RET, ERRO, ACAO, REST)</param>
        /// <param name="status">Status_</param>
        /// <param name="acaoEfetivada">S = Ação executada com sucesso. N = Ação não executada. E = Erro na execução da ação</param>
        /// <param name="dataHoraLiberado">Data e Hora Liberado para Envio</param>
        /// <param name="dataHoraExpiracao">Data e Hora para Expirar a Tempo de Resposta</param>
        public void CriarEnvio(String idMensagemSMS,
                               String descricao,
                               String respostaCliente,
                               String processo,
                               String status,
                               Int32 quantidadeTentativaEnvio, //TODO revisar se realmente precisa ?????
                               String acaoEfetivada,
                               Int32 idFornecedora,
                               DateTime dataHoraLiberado,
                               DateTime dataHoraExpiracao,
                               Int32 quantidadeReenvio, //TODO revisar se realmente precisa ?????
                               DateTime dataHoraEvento,
                               String Operadora)
        {
            try
            {
                // Inclui log da mensagem
                new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagemSMS, descricao, respostaCliente, processo, status, idFornecedora, DateTime.Now, DateTime.Now, DateTime.MinValue, "", Operadora);

                // Atualiza status da mensagem
                //////AtualizarStatus(idMensagemSMS, status);     --- SE PRECISAR ATUALIZAR DATAHORA LIBERADO OU EXPIRACAO, CRIAR OUTRO METODO Q ATUALIZA STATUS E ESTAS DATAS E HORAS. E DAR NOME COERENTE ex: AtualizarStatusDataHoraLiberado
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Lista as mensagens com status AGUAR (AGUARDANDO ENVIO) e AGURE (AGUARDANDO REENVIO)
        /// Obs.: Só começa a contar o tempo quando envia a mensagem pela fornecedora, sendo que:
        ///       _dataHoraLiberadoEnvio = Data e Hora que conectou no WebService da fornecedora e enviou a mensagem
        /// Altera o status para restrição definitiva, erro no envio, aguardando envio ou enviado
        ///     RESTD = RESTRIÇÃO DEFINITIVA (restrição definitiva por tentativa ou cliente bloqueado)
        ///     ERREN = ERRO ENVIO (ocorreu erro no envio)
        ///     AGUAR = AGUARDANDO ENVIO e AGURE = AGUARDANDO REENVIO (sistema indisponível continua aguardando envio e conta como tentativa de envio)
        ///     ENV = ENVIADO e REENV = REENVIADO (mensagem enviado para o cliente)
        /// </summary>
        /// <param name="status">Status da Mensagem</param>
        public async void ProcessarAguardandoEnvio(Object state)
        {
            List<Mensagem> lsMsgAguardandoEnvio;
            String IdMensagemSMS = "0";
            Int32 tempoEntreCadaEnvio = 0;
            Int32 qtdeEnvioSMS = Convert.ToInt32(new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.QTDE_ENVIO_SMS, UsuarioManutencao.ID).Valor);
            Int32 cicloEnvio = (Convert.ToInt32(new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.CICLO_ENVIO_SMS, UsuarioManutencao.ID).Valor) * 1000);
            String urlProcMensagemSMS = new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.URL_PROC_ENVIO_SMS, 0).Valor;
            String execEnvio = new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.PROC_AGUAR_ENV_SMS, 0).Valor;
            Int32 execucao = 0;

            if (qtdeEnvioSMS == 0)
                qtdeEnvioSMS = 1;

            tempoEntreCadaEnvio = cicloEnvio / qtdeEnvioSMS;

            try
            {
                while (true)
                {
                    if (execEnvio.ToUpper() == "ATIV")
                    {
                        lsMsgAguardandoEnvio = BuscarLiberadoEnvio(qtdeEnvioSMS);

                        if (lsMsgAguardandoEnvio != null && lsMsgAguardandoEnvio.Count > 0)
                        {
                            for (int m = 0; m < lsMsgAguardandoEnvio.Count; m++)
                            {
                                using (var httpClient = new HttpClient())
                                {
                                    StringContent content = new StringContent(JsonConvert.SerializeObject(lsMsgAguardandoEnvio[m]), System.Text.Encoding.UTF8, "application/json");

                                    using (var response = await httpClient.PostAsync(urlProcMensagemSMS, content))
                                    {
                                        string apiResponse = await response.Content.ReadAsStringAsync();
                                        //receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);
                                    }
                                }

                                System.Threading.Thread.Sleep(tempoEntreCadaEnvio);
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(cicloEnvio);
                            execEnvio = new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.PROC_AGUAR_ENV_SMS, 0).Valor;
                            execucao++;

                            if (execucao >= 10)
                            {
                                qtdeEnvioSMS = Convert.ToInt32(new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.QTDE_ENVIO_SMS, UsuarioManutencao.ID).Valor);
                                cicloEnvio = (Convert.ToInt32(new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.CICLO_ENVIO_SMS, UsuarioManutencao.ID).Valor) * 1000);
                                execucao = 0;
                            }
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(cicloEnvio);
                        execEnvio = new CaseBusiness.CC.Admin.ConfiguracaoChaveValor(CaseBusiness.CC.Admin.ConfiguracaoChaveValor.ConfiguracaoChave.PROC_AGUAR_ENV_SMS, 0).Valor;
                    }
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Id Mensagem: " + IdMensagemSMS + " Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                new CaseBusiness.CC.Mensageria.MensagemErroLog(base.UsuarioManutencao.ID).Incluir(IdMensagemSMS, ex.Message, ex.StackTrace, DateTime.Now);
            }
            finally
            {
                lsMsgAguardandoEnvio = null;
            }
        }

        /// <summary>
        /// Processa individualmente mensagem com status aguardando envio (AGUAR = AGUARDANDO ENVIO) e aguardando reenvio (AGURE = AGUARDANDO REENVIO)  da tbMENSAGERIA_MENSAGEM
        /// Obs.: Só começa a contar o tempo quando envia a mensagem pela fornecedora, sendo que:
        ///       _dataHoraLiberadoEnvio = Data e Hora que conectou no WebService da fornecedora e enviou a mensagem
        /// Altera o status para restrição definitiva, erro no envio, aguardando envio ou enviado
        /// RESTD = RESTRIÇÃO DEFINITIVA (restrição definitiva por tentativa ou cliente bloqueado)
        /// ERREN = ERRO ENVIO (ocorreu erro no envio)
        /// AGUAR = AGUARDANDO ENVIO ou AGURE = AGUARDANDO REENVIO (sistema indisponível continua aguardando envio e conta como tentativa de envio)
        /// ENV = ENVIADO (mensagem enviado para o cliente)  
        /// <param name="idMensagemSMS">Id da Mensagem</param>
        /// <param name="status">Status da Mensagem</param>
        /// </summary>
        public void ProcessarAguardandoEnvioIndividual(Mensagem mensagemAEnviar)
        {
            DataTable dtConexao;

            try
            {
                String numUsu = String.Empty;
                String senha = String.Empty;
                String seuNum = String.Empty;
                String celular = String.Empty;
                String mensagem = String.Empty;
                String codigoRetorno = String.Empty;
                String descricaoRetorno = String.Empty;
                Boolean proximaFornecedoraRetorno = true;

                DateTime dataHoraExpiracao = DateTime.Now; //TODO revisar se realmente precisa ?????
                DateTime dataHoraEvento = DateTime.Now; //TODO revisar se realmente precisa ?????

                _dataHoraLiberadoEnvio = DateTime.Now;
                dataHoraExpiracao = DateTime.Now; //TODO revisar se realmente precisa ?????
                dataHoraEvento = DateTime.Now; //TODO revisar se realmente precisa ?????


                // Informações da mensagem
                seuNum = mensagemAEnviar.IdMensagemSMS;
                celular = Util.RemoveFormat(Convert.ToString(mensagemAEnviar.Destinatario));

                // Retirar os caracters especiais
                mensagem = Util.RemoverCaracterEspeciais(mensagemAEnviar.TextoMensagem.Trim(), "`´ºª°^~§©|ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç_", "__________AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc ");
                mensagem = mensagem.Replace("\n", " ");

                if (mensagem.Length > 140)
                {
                    _descricaoMensagemLog = "A mensagem tem " + mensagem.Length.ToString() + " caracteres. O máximo de carecteres aceito é 140.";

                    new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(mensagemAEnviar.IdMensagemSMS, descricaoRetorno, "", "ENV", "ERREN", (Int32)CaseBusiness.CB.Fornecedora.TWW, DateTime.Now, DateTime.Now, DateTime.MinValue, "", "");

                    new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(mensagemAEnviar.IdMensagemSMS, "ERREN");

                    return;
                }


                dtConexao = new CaseBusiness.CC.Comunicacao.FornecedoraOrg(base.UsuarioManutencao.ID).BuscarAcesso(CaseBusiness.CB.Fornecedora.TWW.ToString(), mensagemAEnviar.IdCanal, mensagemAEnviar.CodigoOrganizacao, "A");

                if (dtConexao.Rows.Count > 0)
                {
                    for (int m = 0; m < dtConexao.Rows.Count; m++)
                    {
                        numUsu = Convert.ToString(dtConexao.Rows[m]["UsuarioConexao"]);
                        senha = Convert.ToString(dtConexao.Rows[m]["SenhaConexao"]);

                        switch (Convert.ToString(dtConexao.Rows[m]["CodigoFornecedora"]).ToUpper())
                        {
                            // Fornecedora 2RP (somente para realizar teste/simulação)
                            case "2RP":
                                //CaseBusiness.CC.Comunicacao.Fornecedora2RP objFornecedora2RP = new Comunicacao.Fornecedora2RP(base.UsuarioManutencao.ID);

                                //objFornecedora2RP.EnviaSMS(numUsu, senha, seuNum, celular, mensagem, status);

                                //codigoRetorno = objFornecedora2RP.CodigoRetorno;
                                //descricaoRetorno = objFornecedora2RP.DescricaoRetorno;
                                //proximaFornecedoraRetorno = objFornecedora2RP.ProximaFornecedoraRetorno;

                                break;

                            // Fornecedora TWW
                            case "TWW":
                                CaseBusiness.CC.Comunicacao.FornecedoraTWW objFornecedoraTWW = new Comunicacao.FornecedoraTWW(base.UsuarioManutencao.ID);

                                objFornecedoraTWW.EnviaSMS(numUsu, senha, seuNum, celular, mensagem, "AGUAR");

                                codigoRetorno = objFornecedoraTWW.CodigoRetorno;
                                descricaoRetorno = objFornecedoraTWW.DescricaoRetorno;
                                proximaFornecedoraRetorno = objFornecedoraTWW.ProximaFornecedoraRetorno;

                                break;

                            // Fornecedora Zenvia, etc
                            case "ZENVIA":
                                //CaseBusiness.CC.Comunicacao.FornecedoraZenvia objFornecedoraZenvia = new Comunicacao.FornecedoraZenvia(base.UsuarioManutencao.ID);

                                //// Task
                                //// Call and await the Task-returning async method in the same statement.
                                //await objFornecedoraZenvia.EnviaSMS(numUsu, senha, seuNum, celular, mensagem, status);

                                //codigoRetorno = objFornecedoraZenvia.CodigoRetorno;
                                //descricaoRetorno = objFornecedoraZenvia.DescricaoRetorno;
                                //proximaFornecedoraRetorno = objFornecedoraZenvia.ProximaFornecedoraRetorno;

                                break;
                        }

                        _dataHoraLiberadoEnvio = DateTime.Now;

                        switch (codigoRetorno.Trim().ToUpper())
                        {
                            // Mensagem enviada com sucesso
                            case "ENV":

                                new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(mensagemAEnviar.IdMensagemSMS, descricaoRetorno, "", "ENV", "ENV", (Int32)CaseBusiness.CB.Fornecedora.TWW, DateTime.Now, DateTime.Now, DateTime.MinValue, "", "");

                                new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(mensagemAEnviar.IdMensagemSMS, "ENV");

                                break;

                            // Mensagem reenviada com sucesso
                            case "REENV":

                                new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(mensagemAEnviar.IdMensagemSMS, descricaoRetorno, "", "ENV", "REENV", (Int32)CaseBusiness.CB.Fornecedora.TWW, DateTime.Now, DateTime.Now, DateTime.MinValue, "", "");

                                new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(mensagemAEnviar.IdMensagemSMS, "REENV");

                                break;

                            // Ocorreu um erro
                            case "ERREN":

                                new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(mensagemAEnviar.IdMensagemSMS, descricaoRetorno, "", "ENV", "ERREN", (Int32)CaseBusiness.CB.Fornecedora.TWW, DateTime.Now, DateTime.Now, DateTime.MinValue, "", "");

                                new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(mensagemAEnviar.IdMensagemSMS, "ERREN");

                                break;

                            // Aguardando envio (tentar enviar novamente)
                            case "AGUAR":
                                new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(mensagemAEnviar.IdMensagemSMS, descricaoRetorno, "", "REQ", "AGUAR", (Int32)CaseBusiness.CB.Fornecedora.TWW, DateTime.Now, DateTime.Now, DateTime.MinValue, "", "");

                                new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(mensagemAEnviar.IdMensagemSMS, "AGUAR");

                                break;

                            // Aguardando reenvio (tentar reenviar novamente)
                            case "AGURE":
                                new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(mensagemAEnviar.IdMensagemSMS, descricaoRetorno, "", "REQ", "AGURE", (Int32)CaseBusiness.CB.Fornecedora.TWW, DateTime.Now, DateTime.Now, DateTime.MinValue, "", "");

                                new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(mensagemAEnviar.IdMensagemSMS, "AGURE");

                                break;
                        }

                        if (mensagemAEnviar.TipoDestinatario == "CLIEN")
                        {
                            CaseBusiness.CC.Mensageria.EventoContato eventoContato = new CaseBusiness.CC.Mensageria.EventoContato();

                            eventoContato.ID_CAMPANHA = mensagemAEnviar.IdCampanha;
                            eventoContato.ID_CLIENTE = mensagemAEnviar.IdCliente;
                            eventoContato.NOM_CANAL = CaseBusiness.CB.Canal.SMS.ToString();
                            eventoContato.ID_MSG = mensagemAEnviar.IdMensagemSMS;
                            eventoContato.COD_EVENTO = new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).RetornarCodigoEvento("");
                            eventoContato.DTH_EVENTO = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                            eventoContato.COD_RETORNO = "";
                            eventoContato.TXT_MSG_RETORNO = "";
                            eventoContato.DTH_RETORNO = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                            eventoContato.ID_CANAL = (Int32)CaseBusiness.CB.Canal.SMS;
                            eventoContato.COD_LOTE = mensagemAEnviar.NumeroLote;
                            eventoContato.ID_MSG_EXT = "";
                            eventoContato.DS_OPERADORA = "";
                            eventoContato.dh_relatorio = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                            eventoContato.operation = "Insert";
                            eventoContato.operation_sequence = 1;
                            eventoContato.hashKey = "";
                            eventoContato.productionDate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                            eventoContato.source = "envioSMS";

                            eventoContato.EnviarKafka();
                        }
                    }
                }
                else
                {
                    _descricaoMensagemLog = "Não existe conexão ativa com a fornecedora TWW.";

                    new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(mensagemAEnviar.IdMensagemSMS, _descricaoMensagemLog, "", "ENV", "ERREN", (Int32)CaseBusiness.CB.Fornecedora.TWW, DateTime.Now, DateTime.Now, DateTime.MinValue, "", "");

                    new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(mensagemAEnviar.IdMensagemSMS, "ERREN");

                    return;
                }
                // **************************************************************************************************************************************************************************************************************
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Id Mensagem: " + mensagemAEnviar.IdMensagemSMS + " Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                new CaseBusiness.CC.Mensageria.MensagemErroLog(base.UsuarioManutencao.ID).Incluir(mensagemAEnviar.IdMensagemSMS, ex.Message, ex.StackTrace, DateTime.Now);
            }
            finally
            {
                dtConexao = null;
            }
        }



        //TODO: rever codigoGrupoTeste
        /// <summary>
        /// Inclui uma mensagem
        /// </summary>
        /// <param name="codigoConfiguracaoComunicacao">Id da Mensagem de Comunicação</param>
        /// <param name="statusMensagem">Código do Status da Mensagem</param>
        /// <param name="CPF_Descriptografado">Número do CPF Descriptografado (somente quando tipoDestinatario = CLIEN)</param>
        /// <param name="textoMensagem">Texto da Mensagem a ser Enviado Cliente ou teste</param>
        /// <param name="destinatario">Destinatário da mensagem</param>
        /// <param name="tipoDestinatario">Tipo de Destinatário (CLIEN ou MONIT)</param>
        /// <param name="codigoGrupoTeste">Id do Grupo de Teste (somente quando tipoDestinatario = MONIT)</param>
        /// <param name="dhInclusao">Data e Hora da Inclusão</param>
        /// <returns></returns>
        public void Incluir(String idMensagem,
                            Int32 codigoConfiguracaoComunicacao,
                             String statusMensagem,
                             String CPF_Descriptografado,
                             String textoMensagem,
                             String destinatario,
                             String tipoDestinatario,
                             Int32 codigoGrupoTeste,
                             DateTime dhInclusao,
                             DateTime? dhLiberadoEnv,
                             Int32 idCampanha,
                             Int32 codLote,
                             String idCliente)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new CaseBusiness.Framework.Criptografia.Cript(PCI_ConfigPath, "");

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagem);
                acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);
                acessoDadosBase.AddParameter("@MGMSGST_ST", statusMensagem.Trim());
                acessoDadosBase.AddParameter("@MGSMS_NR_CPF", pci.Codificar(CPF_Descriptografado.Trim()));
                acessoDadosBase.AddParameter("@MGSMS_TX_MENSAGEM", textoMensagem.Trim());
                acessoDadosBase.AddParameter("@MGSMS_DS_DEST", destinatario.Trim());
                acessoDadosBase.AddParameter("@MGSMS_TP_DEST", tipoDestinatario.Trim());
                acessoDadosBase.AddParameter("@MGGRPTST_ID", codigoGrupoTeste);
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGSMS_DH_USU_INS", dhInclusao);
                acessoDadosBase.AddParameter("@MGSMS_DH_LIBERADO_ENV", dhLiberadoEnv);
                acessoDadosBase.AddParameter("@CMP_ID", idCampanha);
                acessoDadosBase.AddParameter("@CMP_NR_LOTE", codLote);
                acessoDadosBase.AddParameter("@MGSMS_ID_CLIENTE", idCliente);

                acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGSMS_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Busca mensagens pelo status
        /// </summary>
        /// <returns></returns>
        public List<Mensagem> BuscarLiberadoEnvio(Int32 qtdeMaxRegistros)
        {
            DataTable dt = null;

            List<Mensagem> listMensagem = null;
            Mensagem mensagem = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");



                acessoDadosBase.AddParameter("@QTDE_MAX_REGISTROS", qtdeMaxRegistros);
                acessoDadosBase.AddParameter("@MGSMS_ID_LOTE_ENV", DateTime.Now.Ticks);
                acessoDadosBase.AddParameter("@MGMSGST_ST", "AGUAR");


                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "PRMGSMS_SEL_LIBERADOENVIO").Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    listMensagem = new List<Mensagem>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mensagem = new Mensagem(0);
                        mensagem.IdMensagemSMS = dt.Rows[i]["MGSMS_ID"].ToString();
                        mensagem.IdCampanha = Convert.ToInt32(dt.Rows[i]["CMP_ID"]);
                        mensagem.CodigoOrganizacao = Convert.ToInt32(dt.Rows[i]["ORG_CD"]);
                        mensagem.CodigoConfiguracaoComunicacao = Convert.ToInt32(dt.Rows[i]["MGCONFCOM_CD"]);
                        mensagem.TipoDestinatario = dt.Rows[i]["MGSMS_TP_DEST"].ToString();

                        mensagem.NumeroCPF = dt.Rows[i]["MGSMS_NR_CPF"].ToString();
                        mensagem.TextoMensagem = dt.Rows[i]["MGSMS_TX_MENSAGEM"].ToString();
                        mensagem.Destinatario = dt.Rows[i]["MGSMS_DS_DEST"].ToString();

                        mensagem.IdFornecedora = Convert.ToInt32(dt.Rows[i]["COMFORN_ID"]);
                        mensagem.StatusMensagem = dt.Rows[i]["MGMSGST_ST"].ToString();
                        mensagem.IdCliente = dt.Rows[i]["MGSMS_ID_CLIENTE"].ToString();

                        mensagem.NumeroLote = Convert.ToInt32(dt.Rows[i]["CMP_NR_LOTE"]);
                        mensagem.IdCanal = (Int32)CB.Canal.SMS;

                        listMensagem.Add(mensagem);
                    }

                }

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region DesCriptografando
                //foreach (DataRow dr in dt.Rows)
                //{
                //    //Descriptografar o Número do CPF
                //    if (!String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF"])))
                //    {
                //        _numeroCPF = Convert.ToString(dr["NumeroCPF"]);
                //        dr["NumeroCPF"] = pci.Decodificar(_numeroCPF);

                //        if (String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF"])))
                //        {
                //            // ERRO DE DECRIPT
                //            dr["NumeroCPF"] = "!!ERRO CRYPT!! " + _numeroCPF;
                //        }
                //    }
                //}
                #endregion DesCriptografando

                pci = null;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return listMensagem;
        }

        /// <summary>
        /// Busca mensagens pelo status
        /// </summary>
        /// <param name="idMensagemSMS">Id da Mensagem ("0" = todas as mensagens)</param>
        /// <param name="status">Status da Mensagem</param>
        /// <param name="destinatario">Tipo de Destinatário (AMBOS = cliente e teste, CLIEN = cliente, MONIT = teste)</param>
        /// <returns></returns>
        public DataTable BuscarPorStatus(String idMensagemSMS,
                                         String status)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagemSMS);
                acessoDadosBase.AddParameter("@MGMSGST_ST", status.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGSMS_SEL_BUSCARPORSTATUS").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region DesCriptografando
                //foreach (DataRow dr in dt.Rows)
                //{
                //    //Descriptografar o Número do CPF
                //    if (!String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF"])))
                //    {
                //        _numeroCPF = Convert.ToString(dr["NumeroCPF"]);
                //        dr["NumeroCPF"] = pci.Decodificar(_numeroCPF);

                //        if (String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF"])))
                //        {
                //            // ERRO DE DECRIPT
                //            dr["NumeroCPF"] = "!!ERRO CRYPT!! " + _numeroCPF;
                //        }
                //    }
                //}
                #endregion DesCriptografando

                pci = null;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }


        public DataTable BuscarLiberadoEnvio(String status)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                acessoDadosBase.AddParameter("@MGMSGST_ST", status.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "PRMGSMS_SEL_LIBERADOENVIO").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region DesCriptografando
                foreach (DataRow dr in dt.Rows)
                {
                    //Descriptografar o Número do CPF
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF"])))
                    {
                        _numeroCPF = Convert.ToString(dr["NumeroCPF"]);
                        dr["NumeroCPF"] = pci.Decodificar(_numeroCPF);

                        if (String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF"])))
                        {
                            // ERRO DE DECRIPT
                            dr["NumeroCPF"] = "!!ERRO CRYPT!! " + _numeroCPF;
                        }
                    }
                }
                #endregion DesCriptografando

                pci = null;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Buscar a mensagem enviada e seu respectivo retorno
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="tipoDestinatario">Tipo de Destinatário (AMBOS, CLIEN ou MONIT</param>
        /// <param name="CPF_Descriptografado">Número CPF do Cliente</param>
        /// <returns></returns>
        public DataTable BuscarEnvioRetorno(Int32 codigoOrganizacao,
                                            String tipoDestinatario,
                                            String CPF_Descriptografado)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGSMS_TP_DEST", tipoDestinatario);
                acessoDadosBase.AddParameter("@MGSMS_NR_CPF", pci.Codificar(CPF_Descriptografado.Trim()));

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGSMS_SEL_ENVIO_RETORNO").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region Mascara
                foreach (DataRow dr in dt.Rows)
                {
                    //Colocando máscara no ddd/celular do destinatário
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Destinatario"])))
                    {
                        _destinatario = Convert.ToString(dr["Destinatario"]);
                        dr["Destinatario"] = Util.Telefone_ComMascara(Util.RemoveFormat(_destinatario), Util.enumTipoTelefone.CELULAR);
                    }
                }
                #endregion Mascara
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Busca total de mensagens - Escolhendo uma Fila
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="quantidadeMinutos">Prazo em minutos para buscar as mensagens</param>
        /// <returns>DataTable</returns>
        public DataTable BuscarSintetico(Int32 codigoOrganizacao,
                                         Int32 idCanal,
                                         Int32 idFornecedora,
                                         Int32 quantidadeMinutos)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@QTD_MINUTOS", quantidadeMinutos);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGSMS_SEL_SINTETICO").Tables[0];

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
        /// Busca mensagens - Detalhes da Mensagem
        /// </summary>
        /// <param name="status">Status da Mensagem</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="quantidadeMinutos">Prazo em minutos para buscar as mensagens</param>
        /// <returns></returns>
        public DataTable BuscarAnalitico(String status,
                                         Int32 codigoOrganizacao,
                                         Int32 idCanal,
                                         Int32 idFornecedora,
                                         Int32 quantidadeMinutos)
        {

            DataTable dt = null;
            CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGMSGST_ST", status);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@QTD_MINUTOS", quantidadeMinutos);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGSMS_SEL_ANALITICO").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region Ajuste
                foreach (DataRow dr in dt.Rows)
                {
                    #region DesCriptografando
                    //Descriptografar o Número do CPF
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["NumeroCPF"])))
                    {
                        _numeroCPF = Convert.ToString(dr["NumeroCPF"]);
                        dr["NumeroCPF"] = pci.Decodificar(NumeroCPF);
                    }
                    #endregion DesCriptografando

                    #region Mascara
                    //Colocando máscara no ddd/celular do destinatário
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Destinatario"])))
                    {
                        _destinatario = Convert.ToString(dr["Destinatario"]);
                        dr["Destinatario"] = Util.Telefone_ComMascara(Util.RemoveFormat(_destinatario), Util.enumTipoTelefone.CELULAR);
                    }
                    #endregion Mascara
                }
                #endregion Ajuste
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }


        /// <summary>
        /// Atualiza o Status de uma mensagem
        /// </summary>
        /// <param name="idMensagemSMS">Id da Mensagem</param>
        /// <param name="status">Novo Status</param>
        public void AtualizarStatus(String idMensagemSMS,
                                    String status)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagemSMS);
                acessoDadosBase.AddParameter("@MGMSGST_ST", status);
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMGSMS_UPD_STATUS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar Mensagens Por Status
        /// </summary>
        /// <param name="codigoOrg">Código da Organização</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="codigoFornecedora">Id da Fornecedora</param>
        /// <param name="destinatarioTipo">Tipo de Destinatário (AMBOS, MONIT, CLIEN)</param>
        /// <param name="statusMensagem">Status da Mensagem</param>
        /// <param name="dataHoraInicio">Data Hora Inicio</param>
        /// <param name="dataHoraFim">Data Hora Fim</param>
        /// <returns></returns>
        public DataTable BuscarPorStatus(Int32 codigoOrg,
                                         Int32 idCanal,
                                         Int32 codigoFornecedora,
                                         String destinatarioTipo,
                                         String statusMensagem,
                                         Int32 idUsuario,
                                         DateTime dataHoraInicio,
                                         DateTime dataHoraFim)
        {
            DataTable dt = null;
            CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrg);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@COMFORN_ID", codigoFornecedora);
                acessoDadosBase.AddParameter("@MGCONFMSG_TP_DEST", destinatarioTipo);
                acessoDadosBase.AddParameter("@MGMSGST_ST", statusMensagem);
                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@MGSMSLOG_DH_INICIO", dataHoraInicio);
                acessoDadosBase.AddParameter("@MGSMSLOG_DH_FIM", dataHoraFim);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGSMS_SEL_BUSCAR_STATUS").Tables[0];

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
        /// Buscar Mensagens Por Status
        /// </summary>
        /// <param name="codigoOrg">Código da Organização</param>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="codigoFornecedora">Id da Fornecedora</param>
        /// <param name="destinatarioTipo">Tipo de Destinatário (AMBOS, MONIT, CLIEN)</param>
        /// <param name="statusMensagem">Status da Mensagem</param>
        /// <param name="dataHoraInicio">Data Hora Inicio</param>
        /// <param name="dataHoraFim">Data Hora Fim</param>
        /// <returns></returns>
        public DataTable ListarGrafico(Int32 codigoOrg,
                                       Int32 idCanal,
                                       Int32 codigoFornecedora,
                                       String destinatarioTipo,
                                       String statusMensagem,
                                       Int32 idUsuario,
                                       DateTime dataHoraInicio,
                                       DateTime dataHoraFim,
                                       String agrupamento)
        {
            DataTable dt = null;
            CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrg);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@COMFORN_ID", codigoFornecedora);
                acessoDadosBase.AddParameter("@MGCONFMSG_TP_DEST", destinatarioTipo);
                acessoDadosBase.AddParameter("@MGMSGST_ST", statusMensagem);
                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@MGSMSLOG_DH_INICIO", dataHoraInicio);
                acessoDadosBase.AddParameter("@MGSMSLOG_DH_FIM", dataHoraFim);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGSMS_SEL_LISTAR_GRF_STATUS").Tables[0];

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

        ////TODO: revisar porque o Ivan excluiu o campo MGMSG_FL_FIM_ENVIAR_MSG que indica fim do ciclo
        ///////////// <summary>
        ///////////// Busca total de mensagem de uma transação e configuração de mensagem finaliza ou não
        ///////////// </summary>
        ///////////// <param name="codigoConfiguracaoComunicacao">Código da Configuração da Comunicação</param>
        ///////////// <param name="enviarMensagem">Mensagem Finalizada o Ciclo de Envio/Retorno (N = não finalizada, S = finalizada</param>
        ///////////// <returns></returns>
        //////////public Int32 BuscarFimEnvioMensagem(Int32 codigoConfiguracaoComunicacao,
        //////////                                    String enviarMensagem)
        //////////{
        //////////    Int32 qtdeMensagens = 0;

        //////////    try
        //////////    {
        //////////        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

        //////////        acessoDadosBase.AddParameter("@MGCONFCOM_CD", codigoConfiguracaoComunicacao);
        //////////        acessoDadosBase.AddParameter("@MGMSG_FL_FIM_ENVIAR_MSG", enviarMensagem.Trim());
        //////////        acessoDadosBase.AddParameter("@MGMSG_TOTAL", qtdeMensagens, ParameterDirection.InputOutput);

        //////////        qtdeMensagens = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGSMS_SEL_FIM_ENVIO_MSG")[0]);
        //////////    }
        //////////    catch (Exception ex)
        //////////    {
        //////////        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
        //////////        throw;
        //////////    }

        //////////    return qtdeMensagens;
        //////////}

        public Int64 BuscarEnvelope(String idMensagemSMS)
        {
            Int64 idEnvelopeAnalise = 0;
            String retorno = "0";

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Mensagem está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGSMS_ID", idMensagemSMS);
                acessoDadosBase.AddParameter("@EANL_ID", idEnvelopeAnalise, ParameterDirection.InputOutput);

                retorno = acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGSMS_SEL_ENVELOPE")[0].ToString();

                if (!String.IsNullOrEmpty(retorno))
                {
                    idEnvelopeAnalise = Convert.ToInt64(retorno);
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return idEnvelopeAnalise;
        }

        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("CMP_ID")) { dt.Columns["CMP_ID"].ColumnName = "IdCampanha"; }
            if (dt.Columns.Contains("CMP_NR_LOTE")) { dt.Columns["CMP_NR_LOTE"].ColumnName = "NumeroLote"; }
            if (dt.Columns.Contains("COMFORN_ID")) { dt.Columns["COMFORN_ID"].ColumnName = "IdFornecedora"; }
            if (dt.Columns.Contains("MGCONFCOM_CD")) { dt.Columns["MGCONFCOM_CD"].ColumnName = "CodigoConfiguracaoComunicacao"; }
            if (dt.Columns.Contains("MGGRPTST_ID")) { dt.Columns["MGGRPTST_ID"].ColumnName = "CodigoGrupoTeste"; }
            if (dt.Columns.Contains("MGSMS_DH_LIBERADO_ENV")) { dt.Columns["MGSMS_DH_LIBERADO_ENV"].ColumnName = "DataHoraLiberadoEnvio"; }
            if (dt.Columns.Contains("MGSMS_DH_USU_INS")) { dt.Columns["MGSMS_DH_USU_INS"].ColumnName = "DataHoraInclusao"; }
            if (dt.Columns.Contains("MGSMS_DH_USU_UPD")) { dt.Columns["MGSMS_DH_USU_UPD"].ColumnName = "DataHoraStatusMensagem"; }
            if (dt.Columns.Contains("MGSMS_DS_DEST")) { dt.Columns["MGSMS_DS_DEST"].ColumnName = "Destinatario"; }
            if (dt.Columns.Contains("MGSMS_ID")) { dt.Columns["MGSMS_ID"].ColumnName = "idMensagemSMS"; }
            if (dt.Columns.Contains("MGSMS_ID_CLIENTE")) { dt.Columns["MGSMS_ID_CLIENTE"].ColumnName = "IdCliente"; }
            if (dt.Columns.Contains("MGSMS_ID_MSG_EXT")) { dt.Columns["MGSMS_ID_MSG_EXT"].ColumnName = "IdMensagemSMSExterno"; }
            if (dt.Columns.Contains("MGSMS_NM_CLIENTE")) { dt.Columns["MGSMS_NM_CLIENTE"].ColumnName = "NomeCliente"; }
            if (dt.Columns.Contains("MGSMS_NR_CPF")) { dt.Columns["MGSMS_NR_CPF"].ColumnName = "NumeroCPF"; }
            if (dt.Columns.Contains("MGSMS_TP_DEST")) { dt.Columns["MGSMS_TP_DEST"].ColumnName = "TipoDestinatario"; }
            if (dt.Columns.Contains("MGSMS_TX_MENSAGEM")) { dt.Columns["MGSMS_TX_MENSAGEM"].ColumnName = "TextoMensagem"; }
            if (dt.Columns.Contains("MGMSGST_ST")) { dt.Columns["MGMSGST_ST"].ColumnName = "StatusMensagem"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioIns"; }
            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "IdUsuarioUpd"; }

            if (dt.Columns.Contains("MGMSGST_DS")) { dt.Columns["MGMSGST_DS"].ColumnName = "DescricaoStatusMensagem"; }
            if (dt.Columns.Contains("MGSMSLOG_DS")) { dt.Columns["MGSMSLOG_DS"].ColumnName = "ObservacaoStatus"; }
            if (dt.Columns.Contains("COMCNL_NM")) { dt.Columns["COMCNL_NM"].ColumnName = "NomeCanal"; }
            if (dt.Columns.Contains("MGGRPTST_NM")) { dt.Columns["MGGRPTST_NM"].ColumnName = "NomeGrupoTeste"; }
            //if (dt.Columns.Contains("COMFORN_NM")) { dt.Columns["COMFORN_NM"].ColumnName = "NomeFornecedora"; }            
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            //if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "NomeOrganizacao"; }
            //if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }                        
            //if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioResponsavel"; }
            //if (dt.Columns.Contains("MGSMS_DH_REQUISITADO")) { dt.Columns["MGSMS_DH_REQUISITADO"].ColumnName = "DhRequisitado"; }
            //if (dt.Columns.Contains("MGCONFMSG_ST")) { dt.Columns["MGCONFMSG_ST"].ColumnName = "StatusConfiguracaoMensagem"; }
            //if (dt.Columns.Contains("MGCONFCOM_CD_NM")) { dt.Columns["MGCONFCOM_CD_NM"].ColumnName = "CodigoNomeConfiguracaoComunicacao"; }
            //if (dt.Columns.Contains("DH_MENSAGEM")) { dt.Columns["DH_MENSAGEM"].ColumnName = "DataHoraMensagem"; }
            //if (dt.Columns.Contains("ST_MENSAGEM")) { dt.Columns["ST_MENSAGEM"].ColumnName = "StatusMensagem"; }
            //if (dt.Columns.Contains("DH_RESPOSTA")) { dt.Columns["DH_RESPOSTA"].ColumnName = "DataHoraResposta"; }
            //if (dt.Columns.Contains("ST_RETORNO")) { dt.Columns["ST_RETORNO"].ColumnName = "StatusResposta"; }
            //if (dt.Columns.Contains("RETRE")) { dt.Columns["RETRE"].ColumnName = "RETRE"; }
            //if (dt.Columns.Contains("RETNR")) { dt.Columns["RETNR"].ColumnName = "RETNR"; }
            //if (dt.Columns.Contains("RETNI")) { dt.Columns["RETNI"].ColumnName = "RETNI"; }
            //if (dt.Columns.Contains("ENV")) { dt.Columns["ENV"].ColumnName = "ENV"; }
            //if (dt.Columns.Contains("REENV")) { dt.Columns["ENV"].ColumnName = "REENV"; }
            //if (dt.Columns.Contains("RET")) { dt.Columns["RET"].ColumnName = "RET"; }
            //if (dt.Columns.Contains("RETEX")) { dt.Columns["RETEX"].ColumnName = "RETEX"; }
            //if (dt.Columns.Contains("RETER")) { dt.Columns["RETER"].ColumnName = "RETER"; }
            //if (dt.Columns.Contains("MGSMSLOG_DH")) { dt.Columns["MGSMSLOG_DH"].ColumnName = "DhEnvio"; }
            if (dt.Columns.Contains("MGSMSLOG_DH_RESPOSTA")) { dt.Columns["MGSMSLOG_DH_RESPOSTA"].ColumnName = "DhResposta"; }
            if (dt.Columns.Contains("MGSMSLOG_DS_RESPOSTA")) { dt.Columns["MGSMSLOG_DS_RESPOSTA"].ColumnName = "RespostaCliente"; }
            //if (dt.Columns.Contains("QTD_STATUS")) { dt.Columns["QTD_STATUS"].ColumnName = "QuantidadeStatus"; }
            //if (dt.Columns.Contains("QTD_FORNECEDOR")) { dt.Columns["QTD_FORNECEDOR"].ColumnName = "QuantidadeFornecedora"; }
            //if (dt.Columns.Contains("MONITORIA_REQUISICAO_RESTRICAO")) { dt.Columns["MONITORIA_REQUISICAO_RESTRICAO"].ColumnName = "MonitoriaRequisicaoRestricao"; }
            //if (dt.Columns.Contains("GRP_REGRA")) { dt.Columns["GRP_REGRA"].ColumnName = "Grupo"; }
            //if (dt.Columns.Contains("REGRA")) { dt.Columns["REGRA"].ColumnName = "Regra"; }
            //if (dt.Columns.Contains("MCC")) { dt.Columns["MCC"].ColumnName = "MCC"; }
            //if (dt.Columns.Contains("POS")) { dt.Columns["POS"].ColumnName = "POS"; }
            //if (dt.Columns.Contains("T0010_CD_CLIENTE")) { dt.Columns["T0010_CD_CLIENTE"].ColumnName = "CodigoCliente"; } // Utilizado no CaseManager
            //if (dt.Columns.Contains("T0014_CD_CARTAO")) { dt.Columns["T0014_CD_CARTAO"].ColumnName = "CodigoCartao"; } // Utilizado no CaseManager
            //if (dt.Columns.Contains("MGSMSCMPLTRS_DT_TRANSACAO")) { dt.Columns["MGSMSCMPLTRS_DT_TRANSACAO"].ColumnName = "DataHoraTransacao"; } // Utilizado no CaseManager
            //if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            //if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "IdCanal"; }
            if (dt.Columns.Contains("MGCONFCOM_ST")) { dt.Columns["MGCONFCOM_ST"].ColumnName = "StatusConfiguracaoComunicao"; }
            if (dt.Columns.Contains("MGCONFCOM_CD_NNM")) { dt.Columns["MGCONFCOM_CD_NNM"].ColumnName = "CodigoNomeConfiguracaoComunicao"; }
        }
    }
}