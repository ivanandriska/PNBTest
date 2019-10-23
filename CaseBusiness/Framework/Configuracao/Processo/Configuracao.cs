using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;

namespace CaseBusiness.Framework.Configuracao.Processo
{
    public class Configuracao
    {
        public void CarregarConfiguracao(CaseBusiness.Framework.App aplicacao, Int32 numeroCluster)
        {
            try
            {
				DataSet ds = null;

				if (CaseBusiness.Framework.Configuracao.Configuracao._tipoComunicacao == TipoComunicacao.Local)
					ds = BuscarConfiguracao(aplicacao, numeroCluster);
				else
				{
					//CaseWSFramework.FrameworkSoapClient fWS = new CaseWSFramework.FrameworkSoapClient();
					//CaseWSFramework.App app = (CaseWSFramework.App)((Int32)aplicacao);
					//ds = fWS.BuscarConfiguracao(app, numeroCluster);
				}

				if (ds != null)
				{
					if (ds.Tables.Count > 0)
					{
						if (ds.Tables[0].Rows.Count > 0)
						{
							CaseBusiness.Framework.Configuracao.Configuracao._nomeAplicacao = ds.Tables[0].Rows[0]["T0083_DS_APLICACAO"].ToString();

							CaseBusiness.Framework.Configuracao.Configuracao._templateEmail = ds.Tables[0].Rows[0]["T0091_DS_TEMPLATE_EMAIL"].ToString();

							if (ds.Tables[0].Rows[0]["T0091_FL_APRESENTA_TODOS_INDIC"] != DBNull.Value)
								CaseBusiness.Framework.Configuracao.Configuracao._apresentaTodosIndiacadores = Convert.ToBoolean(ds.Tables[0].Rows[0]["T0091_FL_APRESENTA_TODOS_INDIC"]);

							if (ds.Tables[0].Columns.Contains("T0091_FL_EXECUTA_REPROCESSAMEN"))
							{
								if (ds.Tables[0].Rows[0]["T0091_FL_EXECUTA_REPROCESSAMEN"] != DBNull.Value)
									CaseBusiness.Framework.Configuracao.Configuracao._executaReprocessamento = Convert.ToBoolean(ds.Tables[0].Rows[0]["T0091_FL_EXECUTA_REPROCESSAMEN"]);
							}
						}
					}
				}
			}
            catch (System.Exception ex)
            {
                CaseBusiness.Framework.Log.Log.LogarArquivo("Erro (CarregarConfiguracao): " + ex.Message, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
            }
        }

		public void CarregarConfiguracao(CaseBusiness.Framework.App aplicacao)
		{
			CarregarConfiguracao(aplicacao, 1);
		}

		public DataSet BuscarConfiguracao(CaseBusiness.Framework.App aplicacao, Int32 numeroCluster)
        {
            DataSet ds = null;

            ds = new AcessoDados.Configuracao().BuscarConfiguracao(aplicacao, numeroCluster);

            return ds;
        }

        public List<String[]> CarregarConfiguracaoTCPIP(String pathConfiguracao)
        {
            DataSet cnConsulta = new DataSet();
            List<String[]> listConfiguracoesTcpIP = new List<String[]>();
            String[] r = null;

            try
            {
                cnConsulta.ReadXml(pathConfiguracao);

                for (int i = 0; i < cnConsulta.Tables[0].Rows.Count; i++)
                {
                    r = new String[3];

                    r[0] = cnConsulta.Tables[0].Rows[i][1].ToString();
                    r[1] = cnConsulta.Tables[0].Rows[i][2].ToString();
                    r[2] = (i + 1).ToString(); //Id da conexão

                    listConfiguracoesTcpIP.Add(r);
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            return listConfiguracoesTcpIP;
        }
    }
}
