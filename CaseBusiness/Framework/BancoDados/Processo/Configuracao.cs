using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;

namespace CaseBusiness.Framework.BancoDados.Processo
{
    internal class Configuracao
    {
        internal void CarregarStringsConexao()
        {
            //CaseBusiness.Framework.Configuracao.Configuracao.Loading.Add(DateTime.Now, "Iniciando carga das strings de conexão. Parte 5/10");

            DataSet cnConsulta = new DataSet();
            Entidade.Configuracao conf = null;
            Criptografia.TripleDESCript.TripleDES tDes = new CaseBusiness.Framework.Criptografia.TripleDESCript.TripleDES();

            try
            {
                cnConsulta.ReadXml(CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig + @"/configuracao.xml");

                //if (CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao == TipoAplicacao.Web)
                //    cnConsulta.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"/configuracao.xml");
                //else
                //    cnConsulta.ReadXml(Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location) + @"/configuracao.xml");

                foreach (DataTable dt in cnConsulta.Tables)
                {
                    foreach (DataRow cnLinha in dt.Rows)
                    {
                        conf = new Entidade.Configuracao();

                        if (dt.Columns.Count >= 13)
                            conf.Sgdb = (SGDB)Convert.ToInt32(tDes.Descriptografar(cnLinha[13].ToString()));
                        else
                            conf.Sgdb = SGDB.SQLServer;

                        String strConexao = "";

                        if (conf.Sgdb == SGDB.SQLServer)
                        {
                            conf.Ambiente = tDes.Descriptografar(cnLinha[0].ToString());
                            conf.Servidor = tDes.Descriptografar(cnLinha[1].ToString());
                            conf.Porta = tDes.Descriptografar(cnLinha[2].ToString());
                            conf.BancoDeDados = tDes.Descriptografar(cnLinha[3].ToString());
                            conf.TrustedConnection = tDes.Descriptografar(cnLinha[4].ToString());
                            conf.Usuario = tDes.Descriptografar(cnLinha[5].ToString());
                            conf.Senha = tDes.Descriptografar(cnLinha[6].ToString());
                            conf.Dominio = tDes.Descriptografar(cnLinha[7].ToString());
                            conf.GrupoUsuario = tDes.Descriptografar(cnLinha[8].ToString());
                            conf.GravarLogEvento = tDes.Descriptografar(cnLinha[9].ToString());
                            conf.VersaoSQL = tDes.Descriptografar(cnLinha[10].ToString());
                            conf.Timeout = Convert.ToInt32(tDes.Descriptografar(cnLinha[12].ToString()));

                            if (conf.TrustedConnection == "S")
                            {
                                conf.Usuario = "";
                                conf.Senha = "";

                                strConexao += "Integrated Security=SSPI";
                                strConexao += ";Connect Timeout=" + conf.Timeout.ToString();
                                //strConexao += ";connection reset=true";
                                strConexao += ";connection lifetime=5";
                                strConexao += ";enlist=true";
                                strConexao += ";min pool size=100";
                                strConexao += ";max pool size=1000";
                                strConexao += ";Initial Catalog=" + conf.BancoDeDados;
                                strConexao += ";Data Source=" + conf.Servidor + "," + conf.Porta;
                                CaseBusiness.Framework.Configuracao.Configuracao._autenticacao = Autenticacao.Windows;
                            }
                            else
                            {
                                strConexao += "Persist Security Info=False";
                                strConexao += ";Connect Timeout=" + conf.Timeout.ToString();
                                //strConexao += ";connection reset=true";
                                strConexao += ";connection lifetime=5";
                                strConexao += ";enlist=true";
                                strConexao += ";min pool size=100";
                                strConexao += ";max pool size=1000";
                                strConexao += ";User ID=" + conf.Usuario;
                                strConexao += ";Password=" + conf.Senha;
                                strConexao += ";Initial Catalog=" + conf.BancoDeDados;
                                strConexao += ";Data Source=" + conf.Servidor + "," + conf.Porta;
                                CaseBusiness.Framework.Configuracao.Configuracao._autenticacao = Autenticacao.SQL;
                            }
                        }
                        else
                        {
                            conf.Ambiente = tDes.Descriptografar(cnLinha[0].ToString());
                            conf.Servidor = tDes.Descriptografar(cnLinha[1].ToString());
                            conf.Porta = tDes.Descriptografar(cnLinha[2].ToString());
                            conf.Servico = tDes.Descriptografar(cnLinha[3].ToString());
                            conf.TrustedConnection = tDes.Descriptografar(cnLinha[4].ToString());
                            conf.Usuario = tDes.Descriptografar(cnLinha[5].ToString());
                            conf.Senha = tDes.Descriptografar(cnLinha[6].ToString());
                            conf.Timeout = Convert.ToInt32(tDes.Descriptografar(cnLinha[12].ToString()));
							conf.ConnectionLifetime = Convert.ToInt32(tDes.Descriptografar(cnLinha[15].ToString()));
							conf.DecrPoolSize = Convert.ToInt32(tDes.Descriptografar(cnLinha[16].ToString()));
							conf.IncrPoolSize = Convert.ToInt32(tDes.Descriptografar(cnLinha[17].ToString()));
							conf.HAEvents = tDes.Descriptografar(cnLinha[18].ToString());
							conf.MinPoolSize = Convert.ToInt32(tDes.Descriptografar(cnLinha[19].ToString()));
							conf.MaxPoolSize = Convert.ToInt32(tDes.Descriptografar(cnLinha[20].ToString()));

							if (tDes.Descriptografar(cnLinha[14].ToString()) == "S")
                                conf.LoadingBalance = true;
                            else
                                conf.LoadingBalance = false;

                            strConexao = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = " + conf.Servidor + ")(PORT =" + conf.Porta + "))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = " + conf.Servico + ")))";

                            if (conf.TrustedConnection == "S")
                            {
                                conf.Usuario = "";
                                conf.Senha = "";

                                strConexao += ";Integrated Security=SSPI";
                                strConexao += ";Connect Timeout=" + conf.Timeout.ToString();
                                strConexao += ";Connection Lifetime=" + conf.Timeout.ToString();
								strConexao += ";Decr Pool Size=" + conf.DecrPoolSize.ToString();
								strConexao += ";Incr Pool Size=" + conf.IncrPoolSize.ToString();

								if (conf.HAEvents == "S")
									strConexao += ";HA Events=true";
								else
									strConexao += ";HA Events=false";

								strConexao += ";Min Pool Size=" + conf.MinPoolSize.ToString();
								strConexao += ";Max Pool Size=" + conf.MaxPoolSize.ToString();

								CaseBusiness.Framework.Configuracao.Configuracao._autenticacao = Autenticacao.Windows;
                            }
                            else
                            {
                                strConexao += ";Connect Timeout=" + conf.Timeout.ToString();
                                strConexao += ";User ID=" + conf.Usuario;
                                strConexao += ";Password=" + conf.Senha;
                                strConexao += ";Connection Lifetime=" + conf.Timeout.ToString();
								strConexao += ";Decr Pool Size=" + conf.DecrPoolSize.ToString();
								strConexao += ";Incr Pool Size=" + conf.IncrPoolSize.ToString();

								if (conf.HAEvents == "S")
									strConexao += ";HA Events=true";
								else
									strConexao += ";HA Events=false";

								strConexao += ";Min Pool Size=" + conf.MinPoolSize.ToString();
								strConexao += ";Max Pool Size=" + conf.MaxPoolSize.ToString();

								CaseBusiness.Framework.Configuracao.Configuracao._autenticacao = Autenticacao.SQL;
                            }
                        }

                        conf.StringConexao = strConexao;

                        CaseBusiness.Framework.Configuracao.Configuracao.BancosDisponiveis.Add((CaseBusiness.Framework.BancoDeDados)Convert.ToInt32(tDes.Descriptografar(cnLinha[11].ToString())), conf);
                    }
                }

                //CaseBusiness.Framework.Configuracao.Configuracao.Loading.Add(DateTime.Now, "Foram carregadas: " + CaseBusiness.Framework.Configuracao.Configuracao.BancosDisponiveis.Count.ToString() + " strings de conexão com sucesso. Parte 6/10");
            }
            catch (System.Exception ex)
            {
				//CaseBusiness.Framework.Configuracao.Configuracao.Loading.Add(DateTime.Now,"Erro (CarregarStringsConexao): " + ex.Message + " Parte 6/10");
				CaseBusiness.Framework.Configuracao.Configuracao._erroInicializacao = "Erro (CarregarStringsConexao): " + ex.Message;
				CaseBusiness.Framework.Log.Log.LogarArquivo("Erro (CarregarStringsConexao): " + ex.Message, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }
            finally
            {
                cnConsulta.Dispose();
            }
        }

        internal Entidade.Configuracao BuscarStringConexao(CaseBusiness.Framework.BancoDeDados banco)
        {
            Entidade.Configuracao conf = null;

            try
            {
                if (!CaseBusiness.Framework.Configuracao.Configuracao.BancosDisponiveis.ContainsKey(banco))
                {
                    Log.Log.LogarArquivo("String de conexão para o banco de dados: " + banco.ToString() + " não localizada", CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                    Log.Log.LogarArquivo("Bancos disponíveis: " + CaseBusiness.Framework.Configuracao.Configuracao.BancosDisponiveis.Keys.Count.ToString(), CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");

                    foreach (var item in CaseBusiness.Framework.Configuracao.Configuracao.BancosDisponiveis)
                        Log.Log.LogarArquivo("Disponível: " + item.Key.ToString(), CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }
                else
                    CaseBusiness.Framework.Configuracao.Configuracao.BancosDisponiveis.TryGetValue(banco, out conf);

            }
            catch (System.Exception ex)
            {
                Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }

            return conf;
        }
    }
}
