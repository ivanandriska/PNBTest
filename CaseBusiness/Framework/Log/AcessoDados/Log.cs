using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using CaseBusiness.Framework.BancoDados;

namespace CaseBusiness.Framework.Log.AcessoDados
{
    internal class Log
    {
        #region Atributos

        //private SqlDataReader dr = null;
        private  AcessoDadosBase persiste = null;

        #endregion Atributos

        public Log()
        {
            persiste = new AcessoDadosBase(CaseBusiness.Framework.Configuracao.Configuracao.BancoLog);
        }

        public Log(Transacao transacao)
        {
            persiste = new AcessoDadosBase(transacao);
        }

        public Int32 Logar(TipoLog tipoLog, String mensagem, String stackTrace, String campo, String tipoErroFisico, DateTime dataHora, App aplicacao, Tela tela, Int64 cdLogTransacao)
        {
            Int32 log = 0;
            List<Object> r = null;

            try
            {
                persiste.AddParameter("@LOG_DS_MENSAGEM", mensagem);
                persiste.AddParameter("@LOG_DS_STACK_TRACE", stackTrace);
                persiste.AddParameter("@LOG_DS_CAMPO", campo.ToString());
                persiste.AddParameter("@LOG_TP_ERRO_FISICO", tipoErroFisico);
                persiste.AddParameter("@LOG_DH", dataHora);
                //persiste.AddParameter("@T0082_CD_TELA", (Int32)tela);
                persiste.AddParameter("@LOG_DS_APLICACAO", aplicacao.ToString());

                if (cdLogTransacao > 0)
                    persiste.AddParameter("@MSG_ID", cdLogTransacao);
                else
                    persiste.AddParameter("@MSG_ID", DBNull.Value);

                persiste.AddParameter("@LOG_DS_TIPO", tipoLog.ToString());
                persiste.AddParameter("@LOG_ID", Int32.MinValue, ParameterDirection.Output);

                r = persiste.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prLOG_INS");

                log = Convert.ToInt32(r[0]);
            }
            catch (System.Exception ex) 
            {
                CaseBusiness.Framework.Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }

            return log;
        }
    }
}
