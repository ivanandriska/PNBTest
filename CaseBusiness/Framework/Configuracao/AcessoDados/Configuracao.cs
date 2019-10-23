using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using CaseBusiness.Framework.BancoDados;

namespace CaseBusiness.Framework.Configuracao.AcessoDados
{
    public class Configuracao
    {
        #region Atributos

        private  AcessoDadosBase persiste = null;

        #endregion Atributos

        public Configuracao()
        {
            persiste = new AcessoDadosBase();
        }

        public Configuracao(Transacao transacao)
        {
            persiste = new AcessoDadosBase(transacao);
        }

        public DataSet BuscarConfiguracao(CaseBusiness.Framework.App aplicacao, Int32 numeroCluster)
        {
            DataSet ds = null;

            try
            {
                persiste.AddParameter("@T0083_CD_APLICACAO", (Int32)aplicacao);
				persiste.AddParameter("@T0091_NR_CLUSTER", numeroCluster);
				ds = persiste.ExecuteDataSet(CommandType.StoredProcedure, "PR_CONFIGURACAO_SEL_T0091_APLI");
            }
            catch (System.Exception ex)
            {
                CaseBusiness.Framework.Log.Log.LogarArquivo("Erro(CarregarConfiguracao): " + ex.Message, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
            }

            return ds;
        }
    }
}
