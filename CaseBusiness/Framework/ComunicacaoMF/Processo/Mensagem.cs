using System;
using System.Collections.Generic;
using System.Text;

using CaseBusiness.Framework.ComunicacaoMF.Interface;

namespace CaseBusiness.Framework.ComunicacaoMF.Processo
{
    public class Mensagem
    {
        //public static IEntidade Comunicar(IEntidade entidade, String acaoEnvio, String acaoRetorno)
        //{
        //    //IEntidade entidadeRetorno = null;
        //    //Layout l = new Layout();
        //    //String msgEnvio = "";
        //    //String msgRetorno = "";
        //    //Entidade.Header headerMsg = null;

        //    //try
        //    //{
        //    //    msgEnvio = l.GerarMensagem(entidade, acaoEnvio);

        //    //    CaseBusiness.Framework.Comunicacao.Comunicacao comm = new CaseBusiness.Framework.Comunicacao.Comunicacao(CaseBusiness.Framework.Configuracao.Configuracao.IP, CaseBusiness.Framework.Configuracao.Configuracao.Port, CaseBusiness.Framework.Configuracao.Configuracao.TimeOut, false);

        //    //    if (comm.Enviar(msgEnvio))
        //    //    {
        //    //        msgRetorno = comm.Receber();
        //    //        entidadeRetorno = l.LerMensagem(entidade, acaoRetorno, msgRetorno);
        //    //        headerMsg = (Entidade.Header)entidadeRetorno;
        //    //        ((Entidade.Header)entidadeRetorno).Mensagem = MensagemRetorno(msgEnvio, msgRetorno, ref headerMsg, "", "").Mensagem;
        //    //    }
        //    //}
        //    //catch (System.Exception ex)
        //    //{
        //    //    if (entidadeRetorno != null)
        //    //        ((Entidade.Header)entidadeRetorno).Mensagem = MensagemRetorno(msgEnvio, msgRetorno, ref headerMsg, ex.Message, ex.StackTrace).Mensagem;

        //    //    throw;
        //    //}

        //    //return entidadeRetorno;
        //}

        //public static List<IEntidade> ComunicarLista<T>(T entidade, String acaoEnvio, String acaoRetorno) where T : IEntidade, new()
        //{
        //    List<IEntidade> listRetorno = null;
        //    Entidade.Header headerMsg = null;
        //    Layout l = new Layout();
        //    String msgEnvio = "";
        //    String msgRetorno = "";

        //    try
        //    {
        //        msgEnvio = l.GerarMensagem(entidade, acaoEnvio);

        //        CaseBusiness.Framework.Comunicacao.Comunicacao comm = new CaseBusiness.Framework.Comunicacao.Comunicacao(CaseBusiness.Framework.Configuracao.Configuracao.IP, CaseBusiness.Framework.Configuracao.Configuracao.Port, CaseBusiness.Framework.Configuracao.Configuracao.TimeOut, false);

        //        if (comm.Enviar(msgEnvio))
        //        {
        //            msgRetorno = comm.Receber();
        //            listRetorno = l.LerMensagens<T>(entidade, acaoRetorno, msgRetorno);
        //            headerMsg = (Entidade.Header)listRetorno[0];
        //            ((Entidade.Header)listRetorno[0]).Mensagem = MensagemRetorno(msgEnvio, msgRetorno, ref headerMsg, "", "").Mensagem;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        headerMsg = (Entidade.Header)listRetorno[0];
        //        ((Entidade.Header)listRetorno[0]).Mensagem = MensagemRetorno(msgEnvio, msgRetorno, ref headerMsg, ex.Message, ex.StackTrace).Mensagem;

        //        throw;
        //    }

        //    return listRetorno;
        //}

        //private static Entidade.RespostaMensagem MensagemRetorno(String msgEnvio, String msgRetorno, ref Entidade.Header header, String msgErro, String stackTrace)
        //{
        //    Int32 codigoResposta = header.Resposta;
        //    Entidade.RespostaMensagem mensagemRetorno = new Entidade.RespostaMensagem();
        //    mensagemRetorno.Mensagem = header.Mensagem;

        //    foreach (CaseBusiness.Framework.ComunicacaoMF.Entidade.RespostaMensagem resposta in RespostaMensagem.RespostasMensagem)
        //    {
        //        if (resposta.Codigo == codigoResposta)
        //        {
        //            if (resposta.TipoMensagem == TipoMensagem.Erro)
        //            {
        //                CaseBusiness.Framework.ComunicacaoMF.Entidade.MensagemErro msg = new CaseBusiness.Framework.ComunicacaoMF.Entidade.MensagemErro();

        //                msg.MensagemEnvio = msgEnvio;
        //                msg.Erro = resposta.Mensagem;
        //                msg.MensagemRetorno = QuebrarMensagemRetorno(msgRetorno);
        //                msg.DataHoraErro = DateTime.Now;

        //                throw new CaseBusiness.Framework.Exception.CustomException(CaseBusiness.Framework.Exception.Nivel.Erro, resposta.Mensagem);
        //            }

        //            header.RespostaMensagem = resposta;
        //            mensagemRetorno.Mensagem = resposta.Mensagem;
        //            mensagemRetorno.TipoMensagem = resposta.TipoMensagem;
        //            break;
        //        }

        //        if (header.RespostaMensagem == null || codigoResposta == Int32.MinValue)
        //            header.RespostaMensagem = new Entidade.RespostaMensagem();

        //            header.RespostaMensagem.Mensagem = mensagemRetorno.Mensagem;
        //            header.RespostaMensagem.TipoMensagem = TipoMensagem.Erro;
        //    }

        //    if (mensagemRetorno.Mensagem.Length == 0)
        //    {
        //        if (msgErro.Length > 0 && stackTrace.Length > 0)
        //        {
        //            CaseBusiness.Framework.ComunicacaoMF.Entidade.MensagemErro msg = new CaseBusiness.Framework.ComunicacaoMF.Entidade.MensagemErro();

        //            msg.MensagemEnvio = msgEnvio;
        //            msg.Erro = msgErro;
        //            msg.MensagemRetorno = QuebrarMensagemRetorno(msgRetorno);
        //            msg.DataHoraErro = DateTime.Now;
        //            msg.StackTrace = stackTrace;

        //            mensagemRetorno.Mensagem = msgErro;
        //            mensagemRetorno.TipoMensagem = TipoMensagem.Erro;

        //            header.RespostaMensagem = new Entidade.RespostaMensagem();
        //            header.RespostaMensagem.Mensagem = mensagemRetorno.Mensagem;
        //            header.RespostaMensagem.TipoMensagem = mensagemRetorno.TipoMensagem;
        //        }
        //    }

        //    return mensagemRetorno;
        //}

        //private static List<String> QuebrarMensagemRetorno(String msgRetorno)
        //{
        //    Int32 pos = 0;
        //    List<String> msgRetornoLista = new List<String>();

        //    for (Int32 t = 0; t < 8; t++)
        //    {
        //        String msgTemp = "";

        //        if (msgRetorno.Length > 4000)
        //        {
        //            msgTemp = msgRetorno.Substring(pos, 4000);
        //            msgRetorno = msgRetorno.Remove(pos, 4000);
        //            pos += 4000;
        //        }
        //        else
        //        {
        //            msgTemp = msgRetorno.Substring(pos, msgRetorno.Length);
        //            msgRetorno = msgRetorno.Remove(pos, msgRetorno.Length);
        //        }

        //        msgRetornoLista.Add(msgTemp);

        //        if (msgRetorno.Length == 0)
        //            break;
        //    }

        //    return msgRetornoLista;
        //}
    }
}
