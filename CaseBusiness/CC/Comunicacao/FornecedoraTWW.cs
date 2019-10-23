using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
using System.Xml;
using System.Net;
using System.IO;

namespace CaseBusiness.CC.Comunicacao
{
    public class FornecedoraTWW : BusinessBase
    {
        #region Atributos
        private String _codigoRetorno = String.Empty;
        private String _descricaoRetorno = String.Empty;
        private Boolean _proximaFornecedoraRetorno = false;
        #endregion Atributos

        #region Propriedades
        public String CodigoRetorno
        {
            get { return _codigoRetorno; }
            set { _codigoRetorno = value; }
        }

        public String DescricaoRetorno
        {
            get { return _descricaoRetorno; }
            set { _descricaoRetorno = value; }
        }

        public Boolean ProximaFornecedoraRetorno
        {
            get { return _proximaFornecedoraRetorno; }
            set { _proximaFornecedoraRetorno = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe AnalyticsExecucaoErro - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public FornecedoraTWW(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe AnalyticsExecucaoErro
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public FornecedoraTWW(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe AnalyticsExecucaoErro utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public FornecedoraTWW(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numUsu">Login ( ID ), com até 10 caracteres alfa-numéricos, no sistema Unimessage, fornecido pela TWW.</param>
        /// <param name="senha">Com até 18 caracteres alfanuméricos</param>
        /// <param name="seuNum">Id da Mensagem</param>
        /// <param name="celular">(55DDNNNNNNNN) – Número do celular de destino da mensagem, onde D = Código de área e N = Número do celular</param>
        /// <param name="mensagem">Texto ASCII com até 145 caracteres</param>
        /// <param name="status">Status da Mensagem (AGUAR = AGUARDANDO ENVIO, AGURE = AGUARDANDO REENVIO)</param>
        public void EnviaSMS(String numUsu,
                             String senha,
                             String seuNum,
                             String celular,
                             String mensagem,
                             String status)
        {
            String retorno = String.Empty;

            String seuNumId = String.Empty;
            String seuNumQuantidadeEnvio = String.Empty;

            seuNumId = Util.RetirarCaracterEsquerda(seuNum, 4);
            seuNumQuantidadeEnvio = Util.RetirarCaracterDireita(seuNum, 4);

            try
            {
                //// Instanciando o WebService
                //wsTWW.ReluzCapWebServiceSoapClient ws = new wsTWW.ReluzCapWebServiceSoapClient("ReluzCap Web ServiceSoap");

                //// Chamando o método EnviaSMS
                //retorno = ws.EnviaSMSAsync(numUsu, senha, seuNum, celular, mensagem).Result;


                //Instrução SOAP
                String StrXML = @"<?xml version=""1.0"" encoding=""utf-8""?>
                              <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                       <soap:Body>
                                          <EnviaSMS xmlns=""https://www.twwwireless.com.br/reluzcap/wsreluzcap"">
                                             <NumUsu><![CDATA[{0}]]></NumUsu>
                                             <Senha><![CDATA[{1}]]></Senha>
                                             <SeuNum><![CDATA[{2}]]></SeuNum>
                                             <Celular><![CDATA[{3}]]></Celular>
                                             <Mensagem><![CDATA[{4}]]></Mensagem>
                                           </EnviaSMS>
                                         </soap:Body>
                                        </soap:Envelope>";

                StrXML = String.Format(StrXML, numUsu, senha, seuNum, celular, mensagem);
                XmlDocument soapEnvelopeXML = new XmlDocument();
                soapEnvelopeXML.LoadXml(StrXML);

                //Conexão com endpoind 
                String url = "https://webservices.twwwireless.com.br/reluzcap/wsreluzcap.asmx";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "text/xml; charset=utf-8";
                request.Method = "POST";
                request.Accept = "text/xml";
                request.Headers.Add("SOAPAction: \"https://www.twwwireless.com.br/reluzcap/wsreluzcap/EnviaSMS\"");

                //Envia Dados
                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXML.Save(stream);
                }

                XmlDocument result = new XmlDocument();
                //Pega Resultado
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        result.LoadXml(soapResult);
                    }
                }
                XmlNodeList list = result.GetElementsByTagName("EnviaSMSResult");
                if (list != null)
                {
                    //Console.WriteLine("Result: {0}", list[0].FirstChild.Value);
                    retorno = list[0].FirstChild.Value;
                }


                switch (retorno.Trim().ToUpper())
                {
                    // Mensagem aceita para transmisão
                    case "OK":
                        if (status == "AGUAR")
                        {
                            CodigoRetorno = "ENV";
                            DescricaoRetorno = "Enviado a mensagem e a fornecedora TWW recebeu com sucesso.";
                        }
                        else
                        {
                            CodigoRetorno = "REENV";
                            DescricaoRetorno = "Reenviado a mensagem e a fornecedora TWW recebeu com sucesso.";
                        }

                        ProximaFornecedoraRetorno = false;

                        break;

                    // Mensagem não aceita para transmissão
                    case "NOK":
                        CodigoRetorno = "ERREN";
                        DescricaoRetorno = "A fornecedora TWW não aceitou a mensagem para transmissão.";
                        ProximaFornecedoraRetorno = true;

                        break;

                    // Ocorreu um erro na fornecedora TWW
                    case "ERRO":
                        CodigoRetorno = "ERREN";

                        if (status == "AGUAR")
                        {
                            DescricaoRetorno = "Ocorreu um erro ao enviar a mensagem para a fornecedora TWW.";
                        }
                        else
                        {
                            DescricaoRetorno = "Ocorreu um erro ao reenviar a mensagem para a fornecedora TWW.";
                        }

                        ProximaFornecedoraRetorno = true;

                        break;

                    // (não disponível) - Sistema não disponível
                    case "NA":
                        CodigoRetorno = status;
                        DescricaoRetorno = "Sistema da fornecedora TWW não está disponível.";
                        ProximaFornecedoraRetorno = true;

                        break;

                    // Empresa bloqueada na fornecedora por falta de crédito, falta de pagamento, etc.
                    case "NOCBLOQUEADO":
                        CodigoRetorno = "ERREN";
                        DescricaoRetorno = "Empresa bloqueada na fornecedora TWW.";
                        ProximaFornecedoraRetorno = true;

                        break;

                    // Outros
                    default:
                        CodigoRetorno = status;
                        DescricaoRetorno = "Retornou erro na fornecedora TWW: " + retorno.Trim();
                        ProximaFornecedoraRetorno = true;

                        break;
                }
            }
            catch (Exception ex)
            {
                CodigoRetorno = "ERREN";
                DescricaoRetorno = "Ocorreu um erro ao instanciar o WebService da fornecedora TWW.";
                ProximaFornecedoraRetorno = true;

                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Id Mensagem: " + seuNumId.Trim() + " Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                new CaseBusiness.CC.Mensageria.MensagemErroLog(base.UsuarioManutencao.ID).Incluir(seuNumId.Trim(), ex.Message, ex.StackTrace, DateTime.Now);
            }
        }

        /// <summary>
        /// Receber o Status da Fornecedora TWW
        /// A TWW retorna um DataSet chamado OutDataSet contendo a tabela StatusSMS com no máximo 400 linhas, contendo somente os status de SMS dos últimos 4 dias 
        /// que ainda não tenham sido lidos, e os MARCA COMO LIDOS. Se houverem 400 linhas na tabela, podem haver mais status não lidos, e estes devem ser lidos 
        /// usando chamadas subsequentes à função. Retorna Nothing em caso de erro
        /// Altera o status para restrição definitiva, erro no envio, erro no retorno, mensagem expirada ou retorno de status
        /// RESTD = RESTRIÇÃO DEFINITIVA (celular não pertence a nenhuma operadora, mensagem rejeitada, etc)
        /// ERREN = ERRO ENVIO (erro no envio porque mensagem não foi aceita para transmissão)
        /// RETER = ERRO RETORNO (erro no retorno do processamento da fornecedora)
        /// RETEX = RETORNO EXPIRADO (mensagem expirada sem retorno de DLR da operadora ou conforme outras informações da operadora)
        /// RET = RETORNADO (retornou status da mensagem)
        /// ENVMR = MENSAGEM RECEBIDA (Celular confirmou o recebimento)
        /// ENVML = MENSAGEM LIDA (Celular confirmou a leitura. Obs.: A TWW não tem esse status, portanto não vai ser implantado aqui e somente quando for WhatsApp)
        /// </summary>
        /// <param name="status">Status da Mensagem</param>
        /// <param name="formato">Formato da Mensagem (A = bidirecional e unidirecional, B = mensagem bidirecional, U = mensagem unidirecional)</param>
        public void StatusSMSNaoLido(String status,
                                     String formato)
        {
            DataTable dtMensagem;
            DataTable dtConexao;

            String idMensagem = "0";
            Int32 quantidadeEnvio = 0;

            try
            {
                DataSet dsStatusSMSNaoLido = new DataSet();

                DateTime dhUsarioIns = DateTime.Now;
                String numUsu = String.Empty;
                String senha = String.Empty;
                String seuNumId = String.Empty;
                String seuNumQuantidadeEnvio = String.Empty;
                String statusFornecedora = String.Empty;
                String descricaoMensagemLog = String.Empty;
                Int32 idFornecedora = Int16.MinValue;

                dtConexao = new CaseBusiness.CC.Comunicacao.FornecedoraOrg(base.UsuarioManutencao.ID).BuscarAcessoFornecedor("TWW", 1, "A");

                if (dtConexao.Rows.Count > 0)
                {
                    CaseBusiness.CC.Comunicacao.Fornecedora objFornecedora = new CaseBusiness.CC.Comunicacao.Fornecedora("TWW", UsuarioManutencao.ID);

                    if (objFornecedora.IdFornecedora == Int16.MinValue)
                    {
                        // Não achou fornecefor
                        idFornecedora = 0;
                    }
                    else
                    {
                        idFornecedora = objFornecedora.IdFornecedora;
                    }

                    for (int c = 0; c < dtConexao.Rows.Count; c++)
                    {
                        numUsu = Convert.ToString(dtConexao.Rows[c]["UsuarioConexao"]);
                        senha = Convert.ToString(dtConexao.Rows[c]["SenhaConexao"]);

                        // Instanciando o WebService
                        //wsTWW.ReluzCapWebServiceSoapClient ws = new wsTWW.ReluzCapWebServiceSoapClient("ReluzCap Web ServiceSoap");

                        throw (new Exception("NECESSÁRIO NOVA IMPLEMENTAÇÃO DEVIDO A ALTERAÇÕES"));

                        //dsStatusSMSNaoLido = ws.StatusSMSNaoLidoAsync(numUsu, senha).Result;

                        //if (dsStatusSMSNaoLido.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int m = 0; m < dsStatusSMSNaoLido.Tables[0].Rows.Count; m++)
                        //    {
                        //        dhUsarioIns = DateTime.Now;
                        //        seuNumId = Util.RetirarCaracterEsquerda(dsStatusSMSNaoLido.Tables[0].Rows[m].ItemArray[0].ToString().Trim(), 4);
                        //        seuNumQuantidadeEnvio = Util.RetirarCaracterDireita(dsStatusSMSNaoLido.Tables[0].Rows[m].ItemArray[0].ToString().Trim(), 4);
                        //        statusFornecedora = dsStatusSMSNaoLido.Tables[0].Rows[m].ItemArray[3].ToString();

                        //        idMensagem = Convert.ToInt64(seuNumId.Trim());
                        //        quantidadeEnvio = Convert.ToInt32(seuNumQuantidadeEnvio.Trim());

                        //        dtMensagem = new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).BuscarPorStatus(idMensagem, status, "AMBOS", formato);

                        //        if (dtMensagem.Rows.Count > 0)
                        //        {
                        //            //Se mensagem estiver com status informado então altera o status da mensagem conforme o código de retorno abaixo
                        //            switch (statusFornecedora.Trim().ToUpper())
                        //            {
                        //                //case "OK":
                        //                //    descricaoMensagemLog = "Fornecedora TWW recebeu a mensagem e colocou na fila para enviar para a operadora.";

                        //                //    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ENV", "N", dhUsarioIns);
                        //                //    //new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, descricaoMensagemLog, "ENV", "ENV", 0, "N", 0, dhUsarioIns, dhUsarioIns, 0);

                        //                //    break;

                        //                case "CL":
                        //                    descricaoMensagemLog = "Celular confirmou o recebimento.";

                        //                    //new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ENVMR", "N", dhUsarioIns);
                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ENVMR", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                //case "OP":
                        //                //    descricaoMensagemLog = "Fornecedora TWW enviou a mensagem para operadora de celular.";

                        //                //    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ENV", "N", dhUsarioIns);
                        //                //    //new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, descricaoMensagemLog, "ENV", "ENV", 0, "N", 0, dhUsarioIns, dhUsarioIns, 0);

                        //                //    break;

                        //                case "E0":
                        //                    descricaoMensagemLog = "Fornecedora TWW informou que o celular não pertence a nenhuma operadora.";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                case "E1":
                        //                    descricaoMensagemLog = "Celular cadastrado no Blacklist da lista da fornecedora TWW.";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "REST", "RESTD", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                case "E2":
                        //                    descricaoMensagemLog = "Fornecedora TWW vetou o envio do SMS (foram enviadas mais de 1 mensagem para o mesmo celular no período de 7 dias).";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "REST", "RESTD", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                case "E3":
                        //                    descricaoMensagemLog = "Mensagem duplicada na fornecedora TWW.";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                case "E4":
                        //                    descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi rejeitada pela operadora antes da transmissão (número cancelado ou com restrições).";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                case "E5":
                        //                    descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi expirada porque não teve retorno de DLR da operadora.";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "RETEX", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                case "E6":
                        //                    descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi expirada após sequencias de tentativas.";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "RETEX", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                case "E7":
                        //                    descricaoMensagemLog = "Fornecedora TWW não aceitou a mensagem para transmissão.";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;

                        //                case "E8":
                        //                    descricaoMensagemLog = "Erro de processamento na fornecedora TWW.";

                        //                    new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", 0, "N", idFornecedora, dhUsarioIns, dhUsarioIns, 0);

                        //                    break;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            //Se a mensagem estiver em outro status diferente do pesquisado então somente log a ocorrência
                        //            switch (statusFornecedora.Trim().ToUpper())
                        //            {
                        //                //case "OK":
                        //                //    descricaoMensagemLog = "Fornecedora TWW recebeu a mensagem e colocou na fila para enviar para a operadora.";

                        //                //    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ENV", "N", dhUsarioIns);

                        //                //    break;

                        //                case "CL":
                        //                    descricaoMensagemLog = "Celular confirmou o recebimento.";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ENVMR", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                //case "OP":
                        //                //    descricaoMensagemLog = "Fornecedora TWW enviou a mensagem para operadora de celular.";

                        //                //    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ENV", "N", dhUsarioIns);

                        //                //    break;

                        //                case "E0":
                        //                    descricaoMensagemLog = "Fornecedora TWW informou que o celular não pertence a nenhuma operadora.";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                case "E1":
                        //                    descricaoMensagemLog = "Celular cadastrado no Blacklist da lista da fornecedora TWW.";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "RESTD", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                case "E2":
                        //                    descricaoMensagemLog = "Fornecedora TWW vetou o envio do SMS (foram enviadas mais de 1 mensagem para o mesmo celular no período de 7 dias).";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "RESTD", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                case "E3":
                        //                    descricaoMensagemLog = "Mensagem duplicada na fornecedora TWW.";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                case "E4":
                        //                    descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi rejeitada pela operadora antes da transmissão (número cancelado ou com restrições).";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                case "E5":
                        //                    descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi expirada porque não teve retorno de DLR da operadora.";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "RETEX", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                case "E6":
                        //                    descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi expirada após sequencias de tentativas.";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "RETEX", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                case "E7":
                        //                    descricaoMensagemLog = "Fornecedora TWW não aceitou a mensagem para transmissão.";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", "N", idFornecedora, dhUsarioIns);

                        //                    break;

                        //                case "E8":
                        //                    descricaoMensagemLog = "Erro de processamento na fornecedora TWW.";

                        //                    new CaseBusiness.CC.Mensageria.MensagemLog(base.UsuarioManutencao.ID).Incluir(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "ENV", "ERREN", "N", idFornecedora, dhUsarioIns);

                        //                    break;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Id Mensagem: " + idMensagem.Trim() + " Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                new CaseBusiness.CC.Mensageria.MensagemErroLog(base.UsuarioManutencao.ID).Incluir(idMensagem.Trim(), ex.Message, ex.StackTrace, DateTime.Now);
            }
            finally
            {
                dtMensagem = null;
            }
        }

        /// <summary>
        /// Recebe o Retorno do SMS
        /// A TWW retorna um DataSet chamado OutDataSet contendo uma Tabela chamada SMSMO com no máximo 400 linhas, com as mensagens SMS MO não lidas, 
        /// recebidas nos últimos 4 dias como resposta a SMS enviados anteriormente, e marca esses MOs COMO LIDOS. Se houverem 400 linhas na tabela, 
        /// podem haver mais MOs não lidos, e estes devem ser lidos usando chamadas subsequentes à função. Retorna Nothing em caso de erro
        /// Altera o status para erro no retorno, não identificado, cliente reconheceu a transação, cliente não reconheceu a transação
        /// RETER = ERRO RETORNO
        /// RETNI = RETORNADO NÃO IDENTIFICADO
        /// RETRE = RETORNADO RECONHECIDO CLIENTE
        /// RETNR = RETORNADO NÃO RECONHECIDO CLIENTE
        /// </summary>
        public void BuscaSMSMONaoLido()
        {
            DataTable dtConexao;

            String idMensagem = "0";
            Int32 quantidadeEnvio = 0;

            try
            {
                DataSet dsBuscaSMSMONaoLido = new DataSet();

                DateTime dhUsarioIns = DateTime.Now;
                String descricaoMensagemLog = String.Empty;

                String numUsu = String.Empty;
                String senha = String.Empty;
                String seuNumId = String.Empty;
                String seuNumQuantidadeEnvio = String.Empty;
                String mensagem = String.Empty;
                String status = String.Empty;

                dtConexao = new CaseBusiness.CC.Comunicacao.FornecedoraOrg(base.UsuarioManutencao.ID).BuscarAcessoFornecedor("TWW", 1, "A");

                if (dtConexao.Rows.Count > 0)
                {
                    for (int c = 0; c < dtConexao.Rows.Count; c++)
                    {
                        numUsu = Convert.ToString(dtConexao.Rows[c]["UsuarioConexao"]);
                        senha = Convert.ToString(dtConexao.Rows[c]["SenhaConexao"]);

                        // Instanciando o WebService
                        //wsTWW.ReluzCapWebServiceSoapClient ws = new wsTWW.ReluzCapWebServiceSoapClient("ReluzCap Web ServiceSoap");

                        //wsTWW.ArrayOfXElement r = ws.BuscaSMSMONaoLidoAsync(numUsu, senha).Result;

                        throw (new Exception("NECESSÁRIO NOVA IMPLEMENTAÇÃO DEVIDO A ALTERAÇÕES"));


                        //dsBuscaSMSMONaoLido = ws.BuscaSMSMONaoLidoAsync(numUsu, senha);

                        //if (dsBuscaSMSMONaoLido.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int m = 0; m < dsBuscaSMSMONaoLido.Tables[0].Rows.Count; m++)
                        //    {
                        //        dhUsarioIns = DateTime.Now;
                        //        seuNumId = Util.RetirarCaracterEsquerda(dsBuscaSMSMONaoLido.Tables[0].Rows[m].ItemArray[0].ToString().Trim(), 4);
                        //        seuNumQuantidadeEnvio = Util.RetirarCaracterDireita(dsBuscaSMSMONaoLido.Tables[0].Rows[m].ItemArray[0].ToString().Trim(), 4);
                        //        mensagem = dsBuscaSMSMONaoLido.Tables[0].Rows[m].ItemArray[2].ToString();
                        //        status = dsBuscaSMSMONaoLido.Tables[0].Rows[m].ItemArray[3].ToString();

                        //        idMensagem = Convert.ToInt64(seuNumId.Trim());
                        //        quantidadeEnvio = Convert.ToInt32(seuNumQuantidadeEnvio.Trim());

                        //        // MO - Resposta recebida do número enviado
                        //        if (status.Trim() == "MO")
                        //        {
                        //            // Processo mensagem com status ENV = ENVIADO
                        //            new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).ProcessarRetornoSMS(idMensagem, quantidadeEnvio, "ENV", "AMBOS", "A", mensagem, dhUsarioIns);

                        //            // Processo mensagem com status REENV = REENVIADO
                        //            new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).ProcessarRetornoSMS(idMensagem, quantidadeEnvio, "REENV", "AMBOS", "A", mensagem, dhUsarioIns);

                        //            // Processo mensagem com status ENVMR = MENSAGEM RECEBIDA
                        //            new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).ProcessarRetornoSMS(idMensagem, quantidadeEnvio, "ENVMR", "AMBOS", "A", mensagem, dhUsarioIns);

                        //            // Processo mensagem com status RETEX = RETORNO EXPIRADO
                        //            // Obs.: Foi informado RETE1 para na procedure diferenciar qual RETEX obter
                        //            new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).ProcessarRetornoSMSLog(idMensagem, quantidadeEnvio, "RETE1", "AMBOS", "A", mensagem, dhUsarioIns);
                        //        }
                        //        else
                        //        {
                        //            descricaoMensagemLog = "Código de retorno " + status.Trim() + " não classicado na Fornecedora.";

                        //            new CaseBusiness.CC.Mensageria.Mensagem(base.UsuarioManutencao.ID).CriarEnvio(idMensagem, quantidadeEnvio, descricaoMensagemLog, "", "RET", "RETER", 0, "N", 0, dhUsarioIns, dhUsarioIns, 0);
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, "Id Mensagem: " + idMensagem.Trim() + " Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                new CaseBusiness.CC.Mensageria.MensagemErroLog(base.UsuarioManutencao.ID).Incluir(idMensagem.Trim(), ex.Message, ex.StackTrace, DateTime.Now);
            }
            finally
            {
                dtConexao = null;
            }
        }
        #endregion Métodos
    }
}
