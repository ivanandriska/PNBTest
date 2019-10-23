using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Data;

namespace SMS.Api
{
    public class Processar
    {

        public void processarRetornoStatus(Object state)
        {
            processarRetornoStatus(((Object[])state)[0].ToString(), Convert.ToInt64(((Object[])state)[1]), ((Object[])state)[2].ToString(), Convert.ToDateTime(((Object[])state)[3]), ((Object[])state)[4].ToString());
        }

        public void processarRetornoResposta(Object state)
        {
            processarRetornoResposta(((Object[])state)[0].ToString(), Convert.ToInt64(((Object[])state)[1]), ((Object[])state)[2].ToString(), ((Object[])state)[3].ToString(), Convert.ToDateTime(((Object[])state)[4]), ((Object[])state)[5].ToString());
        }

        public void processarRetornoStatus(string idMensagem, long Celular, string codRetorno, DateTime dhRetorno, string Operadora)
        {
            String descricaoMensagemLog = String.Empty;
            DateTime dhUsuarioIns = DateTime.Now;
            Int32 idFornecedora = (Int32)CaseBusiness.CB.Fornecedora.TWW;

            try
            {
                //Se mensagem estiver com status informado então altera o status da mensagem conforme o código de retorno abaixo
                switch (codRetorno.Trim().ToUpper())
                {
                    case "OK":
                        descricaoMensagemLog = "Fornecedora TWW recebeu a mensagem e colocou na fila para enviar para a operadora.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "RET", "RET", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno,  Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "RET");

                        break;

                    case "CL":
                        descricaoMensagemLog = "Celular confirmou o recebimento.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "RET", "ENVMR", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "ENVMR");

                        break;

                    case "OP":
                        descricaoMensagemLog = "Fornecedora TWW enviou a mensagem para operadora de celular.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "RET", "ENV", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "ENV");


                        break;

                    case "E0":
                        descricaoMensagemLog = "Fornecedora TWW informou que o celular não pertence a nenhuma operadora.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "RET", "ERREN", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "ERREN");

                        break;

                    case "E1":
                        descricaoMensagemLog = "Celular cadastrado no Blacklist da lista da fornecedora TWW.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "REST", "RESTD", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "RESTD");


                        break;

                    case "E2":
                        descricaoMensagemLog = "Fornecedora TWW vetou o envio do SMS (foram enviadas mais de 1 mensagem para o mesmo celular no período de 7 dias).";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "REST", "RESTD", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "RESTD");

                        break;

                    case "E3":
                        descricaoMensagemLog = "Mensagem duplicada na fornecedora TWW.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ERREN", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "ERREN");

                        break;

                    case "E4":
                        descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi rejeitada pela operadora antes da transmissão (número cancelado ou com restrições).";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ERREN", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "ERREN");

                        break;

                    case "E5":
                        descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi expirada porque não teve retorno de DLR da operadora.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "RETEX", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "RETEX");

                        break;

                    case "E6":
                        descricaoMensagemLog = "Fornecedora TWW informou que a mensagem foi expirada após sequencias de tentativas.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "RETEX", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "RETEX");

                        break;

                    case "E7":
                        descricaoMensagemLog = "Fornecedora TWW não aceitou a mensagem para transmissão.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ERREN", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "ERREN");

                        break;

                    case "E8":
                        descricaoMensagemLog = "Erro de processamento na fornecedora TWW.";

                        new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, "", "ENV", "ERREN", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, codRetorno, Operadora);

                        new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "ERREN");

                        break;
                }

                DataTable dadosMensagem = new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Buscar(idMensagem);

                if (dadosMensagem != null && dadosMensagem.Rows.Count > 0)
                {
                    //se for uma msg de seed list, não enviar evento de retorno para o cloudera, só registrar na aplicação-------------------------------------------

                    CaseBusiness.CC.Mensageria.EventoContato eventoContato = new CaseBusiness.CC.Mensageria.EventoContato();

                    eventoContato.ID_CAMPANHA = Convert.ToInt32(dadosMensagem.Rows[0]["IdCampanha"]);
                    eventoContato.ID_CLIENTE = dadosMensagem.Rows[0]["IdCliente"].ToString();
                    eventoContato.NOM_CANAL = CaseBusiness.CB.Canal.SMS.ToString();
                    eventoContato.ID_MSG = idMensagem;
                    eventoContato.COD_EVENTO = new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).RetornarCodigoEvento(codRetorno);
                    eventoContato.DTH_EVENTO = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                    eventoContato.COD_RETORNO = codRetorno;
                    eventoContato.TXT_MSG_RETORNO = "";
                    eventoContato.DTH_RETORNO = dhRetorno.ToString("yyyy-MM-dd'T'HH:mm:ss");
                    eventoContato.ID_CANAL = (Int32)CaseBusiness.CB.Canal.SMS;
                    eventoContato.COD_LOTE = Convert.ToInt32(dadosMensagem.Rows[0]["NumeroLote"]);
                    eventoContato.ID_MSG_EXT = "";
                    eventoContato.DS_OPERADORA = Operadora;
                    eventoContato.dh_relatorio = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                    eventoContato.operation = "Insert";
                    eventoContato.operation_sequence = 1;
                    eventoContato.hashKey = "";
                    eventoContato.productionDate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                    eventoContato.source = "retornoStatusSMS";

                    eventoContato.EnviarKafka();
                }
                else
                    CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Aviso, "Recebido retorno de status da mensagem de ID: " + idMensagem + "  que não possui registro de envio", "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);


            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            }
        }

        public void processarRetornoResposta(string idMensagem, long Celular, string textoRetorno, String ShortCode, DateTime dhRetorno, string Operadora)
        {
            String descricaoMensagemLog = String.Empty;
            DateTime dhUsuarioIns = DateTime.Now;
            Int32 idFornecedora = (Int32)CaseBusiness.CB.Fornecedora.TWW;

            try
            {
                descricaoMensagemLog = "Cliente respondeu mensagem";

                new CaseBusiness.CC.Mensageria.MensagemSMSLog(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Incluir(idMensagem, descricaoMensagemLog, textoRetorno, "RET", "RETCL", idFornecedora, dhUsuarioIns, dhUsuarioIns, dhRetorno, "", Operadora);

                new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).AtualizarStatus(idMensagem, "RETCL");

                DataTable dadosMensagem = new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).Buscar(idMensagem);

                if (dadosMensagem != null && dadosMensagem.Rows.Count > 0)
                {

                    //se for uma msg de seed list, não enviar evento de retorno para o cloudera, só registrar na aplicação-------------------------------------------

                    CaseBusiness.CC.Mensageria.EventoContato eventoContato = new CaseBusiness.CC.Mensageria.EventoContato();

                    eventoContato.ID_CAMPANHA = Convert.ToInt32(dadosMensagem.Rows[0]["IdCampanha"]);
                    eventoContato.ID_CLIENTE = dadosMensagem.Rows[0]["IdCliente"].ToString();
                    eventoContato.NOM_CANAL = CaseBusiness.CB.Canal.SMS.ToString();
                    eventoContato.ID_MSG = idMensagem;
                    eventoContato.COD_EVENTO = new CaseBusiness.CC.Mensageria.Mensagem(CaseBusiness.Framework.Configuracao.Configuracao.UsuarioAplicacao).RetornarCodigoEvento("");
                    eventoContato.DTH_EVENTO = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                    eventoContato.COD_RETORNO = "";
                    eventoContato.TXT_MSG_RETORNO = textoRetorno;
                    eventoContato.DTH_RETORNO = dhRetorno.ToString("yyyy-MM-dd'T'HH:mm:ss");
                    eventoContato.ID_CANAL = (Int32)CaseBusiness.CB.Canal.SMS;
                    eventoContato.COD_LOTE = Convert.ToInt32(dadosMensagem.Rows[0]["NumeroLote"]);
                    eventoContato.ID_MSG_EXT = "";
                    eventoContato.DS_OPERADORA = Operadora;
                    eventoContato.dh_relatorio = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss");
                    eventoContato.operation = "Insert";
                    eventoContato.operation_sequence = 1;
                    eventoContato.hashKey = "";
                    eventoContato.productionDate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                    eventoContato.source = "retornoRespostaSMS";

                    eventoContato.EnviarKafka();


                }
                else
                    CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Aviso, "Recebido retorno de resposta da mensagem de ID: " + idMensagem + "  que não possui registro de envio", "", "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);


            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
            }
        }
    }
}
