using System;
using System.Collections.Generic;
using System.Text;

using CaseBusiness.Framework.ComunicacaoMF.Entidade;
using CaseBusiness.Framework.ComunicacaoMF.Interface;

namespace CaseBusiness.Framework.ComunicacaoMF.Processo
{
    public class RespostaMensagem
    {
        private static List<Entidade.RespostaMensagem> _respostasMensagem = null;

        public static List<Entidade.RespostaMensagem> RespostasMensagem
        {
            get 
            {
                //if (_respostasMensagem == null)
                //    _respostasMensagem = new Framework.ComunicacaoMF.Processo.RespostaMensagem().CarregarRespostas();
                
                return _respostasMensagem;
            }
        }

        //public List<Entidade.RespostaMensagem> CarregarRespostas()
        //{
        //    return new AcessoDados.RespostaMensagem().CarregarRespostas();
        //}
    }
}
