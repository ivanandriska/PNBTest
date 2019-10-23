using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

using CaseBusiness.ISO.MISO8583;
using CaseBusiness.ISO.MISO8583.Parsing;

namespace CaseBusiness.ISO
{
    public class Mensagem 
    {
        private static readonly Int32 TAMANHO_USADO_PARA_HEADER_DA_MENSAGEM = 0;
        MessageFactory _factory;

        public Mensagem()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            _factory = new MessageFactory();
        }

        /// <summary>
        /// Cria template de mensagem baseado no MTI da mensagem
        /// </summary>
        /// <param name="tipo">MTI</param>
        /// <returns></returns>
        //public IMensagem CriarTemplate(MTI mti)
        //{
        //    IsoMessage isoMessage;
        //    IMensagem retorno;

        //    try
        //    {
        //        isoMessage = _factory.NewTemplateMessage(Convert.ToInt32(mti));

        //        retorno = new BaseMensagemFormatoISO(isoMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return retorno;
        //}

        //public IMensagem CriarMensagemParse(MTI tipo)
        //{
        //    IsoMessage isoMessage;
        //    IMensagem retorno;

        //    try
        //    {
        //        //isoMessage = _factory.NewParseMessage(Convert.ToInt32(tipo));

        //        //retorno = new BaseMensagemFormatoISO(isoMessage);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return retorno;
        //}

        /// <summary>
        /// Cria sua respectiva mensagem ISO de resposta a partir de uma mensagem de entrada obdecendo o layout definido em: formatoMensagens.xml
        /// Será gerado a mensagem de resposta baseado no MTI da mensagem de entrada, seguindo o exemplo: 0100 -> 0110, 0420 -> 0430, ....
        /// </summary>
        /// <param name="mensagem"></param>
        /// <returns>Mensagem ISO de resposta baseada na mensagem ISO de entrada</returns>
        public IsoMessage GerarResposta(CaseBusiness.CC.Global.MensagemInterfaceDados interfaceDados, IsoMessage mensagem)
        {
            //IMensagem retorno;
            IsoMessage retorno;

            try
            {
                //MensagemAdapter respostaAdaptada = mensagem as MensagemAdapter;

                //if (respostaAdaptada == null)
                //    throw new InvalidCastException("Mensagem no formato diferente do esperado.");

                //retorno = new BaseMensagemFormatoISO(_factory.CreateResponse(respostaAdaptada.Mensagem));
                retorno = _factory.CreateResponse(interfaceDados, mensagem);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }

       
        /// <summary>
        /// Descomprime a mensagem ISO recebida
        /// </summary>
        /// <param name="mensagem">Array de bytes da mensagem ISO recebida</param>
        /// <returns>Mensagem ISO descomprimida</returns>
        public IsoMessage Parse(CaseBusiness.CC.Global.MensagemInterfaceDados interfaceDados, CaseBusiness.ISO.ParseMode parseMode)
        {
            IsoMessage iso;

            try
            {
                iso = _factory.ParseMessage(interfaceDados, TAMANHO_USADO_PARA_HEADER_DA_MENSAGEM, parseMode);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return iso;
        }

        public Byte[] Format(CaseBusiness.CC.Global.MensagemInterfaceDados interfaceDados, IsoMessage mensagem)
        {
            Byte[] retorno;

            try
            {
                retorno = _factory.FormatMessage(interfaceDados, mensagem.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }
    }
}