using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.Log.Processo
{
    internal class Log
    {
        public Int32 Logar(TipoLog tipoLog, String mensagem, String stackTrace, String campo, String tipoErroFisico, DateTime dataHora, App aplicacao, Tela tela, Int32 cdLog)
        {
            Int32 cod = 0;

            if (CaseBusiness.Framework.Configuracao.Configuracao.TipoComunicacao == TipoComunicacao.Local)
                cod = new AcessoDados.Log().Logar(tipoLog, mensagem, stackTrace, campo, tipoErroFisico, dataHora, aplicacao, tela, cdLog);
            else if (CaseBusiness.Framework.Configuracao.Configuracao.TipoComunicacao == TipoComunicacao.Remota)
            {
                //CaseWSFramework.FrameworkSoapClient fWS = new CaseWSFramework.FrameworkSoapClient();
                //cod = fWS.Logar((CaseWSFramework.TipoLog)((Int32)(tipoLog)), mensagem, stackTrace, campo, tipoErroFisico, dataHora, (CaseWSFramework.App)((Int32)(aplicacao)), (CaseWSFramework.Tela)((Int32)(tela)), cdLog);
            }

            return cod;
        }
    }
}
