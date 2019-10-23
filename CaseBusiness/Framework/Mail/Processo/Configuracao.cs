using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;

namespace CaseBusiness.Framework.Mail.Processo
{
    internal class Configuracao
    {
        internal void Carregar()
         {
             DataSet cnConsulta = new DataSet();
             Criptografia.TripleDESCript.TripleDES tDes = new CaseBusiness.Framework.Criptografia.TripleDESCript.TripleDES();

            try
            {
                cnConsulta.ReadXml(CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig + @"/parametros.xml");

                //if (CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao == TipoAplicacao.Web)
                //    cnConsulta.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"/parametros.xml");
                //else
                //    cnConsulta.ReadXml(Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location) + @"/parametros.xml");

                foreach (DataTable dt in cnConsulta.Tables)
                {
                    foreach (DataRow cnLinha in dt.Rows)
                    {
                        Entidade.Smtp.ProtocoloSmtp = tDes.Decodificar(cnLinha[0].ToString());
                        Entidade.Smtp.Porta = Int32.Parse(tDes.Decodificar(cnLinha[1].ToString()));
                        Entidade.Smtp.Ssl = Boolean.Parse(tDes.Decodificar(cnLinha[2].ToString()));
                        Entidade.Smtp.Dominio = tDes.Decodificar(cnLinha[3].ToString());
                        Entidade.Smtp.Nome = tDes.Decodificar(cnLinha[4].ToString());
                        Entidade.Smtp.EmailDe = tDes.Decodificar(cnLinha[5].ToString());
                        Entidade.Smtp.Login = tDes.Decodificar(cnLinha[6].ToString());
                        Entidade.Smtp.Senha = tDes.Decodificar(cnLinha[7].ToString());                                                
                    }
                }
            }
            catch(System.Exception ex)
            {
                CaseBusiness.Framework.Log.Log.LogarArquivo("Erro(Carregar: parametros.xml): " + ex.Message, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
            }
            finally
            {
                cnConsulta.Dispose();
            }
         }
    }
}
