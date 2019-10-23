using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace CaseBusiness.Framework
{
    public class Aplicacao
    {
        public Boolean Iniciar(CaseBusiness.Framework.TipoAplicacao tipo, CaseBusiness.Framework.App aplicacao, List<String> arquivosInicializacao)
        {
            Boolean apto = true;

            try
            {
                if (arquivosInicializacao != null)
                    CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao = arquivosInicializacao;

                CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao = tipo;
                CaseBusiness.Framework.Configuracao.Configuracao._aplicacao = aplicacao;
                CaseBusiness.Framework.Configuracao.Configuracao._usuarioAplicacao = definirUsuarioAplicacao(aplicacao);

                try
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._cliente = (CaseBusiness.Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Cliente não identificado. " + ex.Message + " " + ex.StackTrace,  CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (VerificarArquivosInicializacao())
                {
                    foreach (String itemArquivo in arquivosInicializacao)
                    {
                        switch (itemArquivo)
                        {
                            case "configuracao.xml":
                                new BancoDados.Processo.Configuracao().CarregarStringsConexao();

                                //new Configuracao.Processo.Configuracao().CarregarConfiguracao(aplicacao);
                                break;

							case "configuracao_pci.xml":
								new CaseBusiness.Framework.Criptografia.Cript();

								if (!Configuracao.Configuracao.pciCarregado)
									return false;
								break;
                        }
                    }
                }
                else
                    return false;

				if (!VerificarArquivosAdicionaisInicializacao())
					return false;
			}
            catch (System.Exception ex)
            {
                apto = false;
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return apto;
        }

        public Boolean Iniciar(CaseBusiness.Framework.TipoAplicacao tipo, CaseBusiness.Framework.App aplicacao, List<String> arquivosInicializacao, CaseBusiness.Framework.BancoDeDados bancoLog)
        {
            Boolean apto = true;
            CaseBusiness.Framework.Configuracao.Configuracao._bancoLog = bancoLog;

            try
            {
                if (arquivosInicializacao != null)
                    CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao = arquivosInicializacao;

                CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao = tipo;
                CaseBusiness.Framework.Configuracao.Configuracao._aplicacao = aplicacao;
                CaseBusiness.Framework.Configuracao.Configuracao._usuarioAplicacao = definirUsuarioAplicacao(aplicacao);

                try
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._cliente = (CaseBusiness.Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Cliente não identificado. " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (VerificarArquivosInicializacao())
                {
                    foreach (String itemArquivo in arquivosInicializacao)
                    {
                        switch (itemArquivo)
                        {
                            case "configuracao.xml":
                                new BancoDados.Processo.Configuracao().CarregarStringsConexao();

                                //new Configuracao.Processo.Configuracao().CarregarConfiguracao(aplicacao);
                                break;

							case "configuracao_pci.xml":
								new CaseBusiness.Framework.Criptografia.Cript();

								if (!Configuracao.Configuracao.pciCarregado)
									return false;
								break;
						}
                    }
                }
                else
                    apto = false;

				if (!VerificarArquivosAdicionaisInicializacao())
					return false;

			}
            catch (System.Exception ex)
            {
                apto = false;
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return apto;
        }

        public Boolean Iniciar(CaseBusiness.Framework.TipoAplicacao tipo, CaseBusiness.Framework.App aplicacao, List<String> arquivosInicializacao, Boolean verificaComunicacao)
        {
            Boolean apto = true;

            try
            {
                if (arquivosInicializacao != null)
                    CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao = arquivosInicializacao;

                CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao = tipo;
                CaseBusiness.Framework.Configuracao.Configuracao._aplicacao = aplicacao;
                CaseBusiness.Framework.Configuracao.Configuracao._usuarioAplicacao = definirUsuarioAplicacao(aplicacao);

                try
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._cliente = (CaseBusiness.Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Cliente não identificado. " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (VerificarArquivosInicializacao())
                {
                    foreach (String itemArquivo in arquivosInicializacao)
                    {
                        switch (itemArquivo)
                        {
                            case "configuracao.xml":
                                new BancoDados.Processo.Configuracao().CarregarStringsConexao();

                                if (verificaComunicacao)
                                    new CaseBusiness.Framework.Comunicacao.Comunicacao().IdentificaComunicacaoAplicacaoLocalRemota();

                               // new Configuracao.Processo.Configuracao().CarregarConfiguracao(aplicacao);
                                break;

							case "configuracao_pci.xml":
								new CaseBusiness.Framework.Criptografia.Cript();

								if (!Configuracao.Configuracao.pciCarregado)
									return false;
								break;
						}
                    }
                }
                else
                    apto = false;

				if (!VerificarArquivosAdicionaisInicializacao())
					return false;
			}
            catch (System.Exception ex)
            {
                apto = false;
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return apto;
        }

        public Boolean Iniciar(CaseBusiness.Framework.TipoAplicacao tipo, CaseBusiness.Framework.App aplicacao, List<String> arquivosInicializacao, Boolean verificaComunicacao, CaseBusiness.Framework.BancoDeDados bancoLog)
        {
            Boolean apto = true;
            CaseBusiness.Framework.Configuracao.Configuracao._bancoLog = bancoLog;

            try
            {
                if (arquivosInicializacao != null)
                    CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao = arquivosInicializacao;

                CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao = tipo;
                CaseBusiness.Framework.Configuracao.Configuracao._aplicacao = aplicacao;
                CaseBusiness.Framework.Configuracao.Configuracao._usuarioAplicacao = definirUsuarioAplicacao(aplicacao);

                try
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._cliente = (CaseBusiness.Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Cliente não identificado. " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (VerificarArquivosInicializacao())
                {
                    foreach (String itemArquivo in arquivosInicializacao)
                    {
                        switch (itemArquivo)
                        {
                            case "configuracao.xml":
                                new BancoDados.Processo.Configuracao().CarregarStringsConexao();

                                if (verificaComunicacao)
                                    new CaseBusiness.Framework.Comunicacao.Comunicacao().IdentificaComunicacaoAplicacaoLocalRemota();

                                //new Configuracao.Processo.Configuracao().CarregarConfiguracao(aplicacao);
                                break;

							case "configuracao_pci.xml":
								new CaseBusiness.Framework.Criptografia.Cript();

								if (!Configuracao.Configuracao.pciCarregado)
									return false;
								break;
						}
                    }
                }
                else
                    apto = false;

				if (!VerificarArquivosAdicionaisInicializacao())
					return false;
			}
            catch (System.Exception ex)
            {
                apto = false;
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return apto;
        }

        public Boolean Iniciar(CaseBusiness.Framework.TipoAplicacao tipo, CaseBusiness.Framework.App aplicacao, List<String> arquivosInicializacao, String chave)
        {
            Boolean apto = true;

            if (chave == Configuracao.Configuracao._chaveArmazenada)
                Configuracao.Configuracao._habilitaAcessoDadosPrivados = true;

            try
            {
                if (arquivosInicializacao != null)
                    CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao = arquivosInicializacao;

                CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao = tipo;
                CaseBusiness.Framework.Configuracao.Configuracao._aplicacao = aplicacao;
                CaseBusiness.Framework.Configuracao.Configuracao._usuarioAplicacao = definirUsuarioAplicacao(aplicacao);

                try
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._cliente = (CaseBusiness.Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Cliente não identificado. " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (VerificarArquivosInicializacao())
                {
                    foreach (String itemArquivo in arquivosInicializacao)
                    {
                        switch (itemArquivo)
                        {
                            case "configuracao.xml":
                                new BancoDados.Processo.Configuracao().CarregarStringsConexao();

                                //new Configuracao.Processo.Configuracao().CarregarConfiguracao(aplicacao);
                                
                                break;

							case "configuracao_pci.xml":
								new CaseBusiness.Framework.Criptografia.Cript();

								if (!Configuracao.Configuracao.pciCarregado)
									return false;
								break;
						}
                    }
                }
                else
                    apto = false;

				if (!VerificarArquivosAdicionaisInicializacao())
					return false;
			}
            catch (System.Exception ex)
            {
                apto = false;
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return apto;
        }

        public Boolean Iniciar(CaseBusiness.Framework.TipoAplicacao tipo, CaseBusiness.Framework.App aplicacao, List<String> arquivosInicializacao, String chave, CaseBusiness.Framework.BancoDeDados bancoLog)
        {
            //CaseBusiness.Framework.Configuracao.Configuracao.Loading.Add(DateTime.Now, "Definido arquivos de inicialização. Parte 1/10");

            Boolean apto = false;
            CaseBusiness.Framework.Configuracao.Configuracao._bancoLog = bancoLog;

            if (chave == Configuracao.Configuracao._chaveArmazenada)
                Configuracao.Configuracao._habilitaAcessoDadosPrivados = true;

            try
            {
                //CaseBusiness.Framework.Configuracao.Configuracao.Loading.Add(DateTime.Now, "Definindo tipo aplicação, aplicação e usuário aplicação. Parte 2/10");

                if (arquivosInicializacao != null)
                    CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao = arquivosInicializacao;

                CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao = tipo;
                CaseBusiness.Framework.Configuracao.Configuracao._aplicacao = aplicacao;
                CaseBusiness.Framework.Configuracao.Configuracao._usuarioAplicacao = definirUsuarioAplicacao(aplicacao);

                try
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._cliente = (CaseBusiness.Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Cliente não identificado. " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (VerificarArquivosInicializacao())
                {
                    foreach (String itemArquivo in arquivosInicializacao)
                    {
                        switch (itemArquivo)
                        {
                            case "configuracao.xml":
                                new BancoDados.Processo.Configuracao().CarregarStringsConexao();

                                //new Configuracao.Processo.Configuracao().CarregarConfiguracao(aplicacao);

                                break;

							case "configuracao_pci.xml":
								new CaseBusiness.Framework.Criptografia.Cript();

								if (!Configuracao.Configuracao.pciCarregado)
									return false;
								break;
						}
                    }

                    apto = true;

                    //CaseBusiness.Framework.Configuracao.Configuracao.Loading.Add(DateTime.Now, "Inicialização efetuada com sucesso. Parte 9/10");
                    //CaseBusiness.Framework.Configuracao.Configuracao.Loading.Add(DateTime.Now, "Serviço preparado para execução. Parte 10/10");

                    //gravarLogInicializacao("DB");
                }
                else
                    apto = false;

				if (!VerificarArquivosAdicionaisInicializacao())
					return false;
			}
            catch (System.Exception ex)
            {
                apto = false;
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return apto;
        }

        public Boolean Iniciar(CaseBusiness.Framework.TipoAplicacao tipo, CaseBusiness.Framework.App aplicacao, Int32 numeroCluster, List<String> arquivosInicializacao, String chave, Boolean verificaComunicacao)
        {
            Boolean apto = true;

            if (chave == Configuracao.Configuracao._chaveArmazenada)
                Configuracao.Configuracao._habilitaAcessoDadosPrivados = true;

            try
            {
                if (arquivosInicializacao != null)
                    CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao = arquivosInicializacao;

                CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao = tipo;
                CaseBusiness.Framework.Configuracao.Configuracao._aplicacao = aplicacao;
                CaseBusiness.Framework.Configuracao.Configuracao._usuarioAplicacao = definirUsuarioAplicacao(aplicacao);

                try
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._cliente = (CaseBusiness.Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Cliente não identificado. " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (VerificarArquivosInicializacao())
                {
                    foreach (String itemArquivo in arquivosInicializacao)
                    {
                        switch (itemArquivo)
                        {
                            case "configuracao.xml":
                                new BancoDados.Processo.Configuracao().CarregarStringsConexao();
                                
                                if (verificaComunicacao)
                                    new CaseBusiness.Framework.Comunicacao.Comunicacao().IdentificaComunicacaoAplicacaoLocalRemota();

                                //new Configuracao.Processo.Configuracao().CarregarConfiguracao(aplicacao);
                                break;

							case "configuracao_pci.xml":
								new CaseBusiness.Framework.Criptografia.Cript();

								if (!Configuracao.Configuracao.pciCarregado)
									return false;
								break;
						}
                    }
                }
                else
                    apto = false;

				if (!VerificarArquivosAdicionaisInicializacao())
					return false;
			}
            catch (System.Exception ex)
            {
                apto = false;
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return apto;
        }

        public Boolean Iniciar(CaseBusiness.Framework.TipoAplicacao tipo, CaseBusiness.Framework.App aplicacao, List<String> arquivosInicializacao, String chave, Boolean verificaComunicacao, CaseBusiness.Framework.BancoDeDados bancoLog)
        {
            Boolean apto = true;
            CaseBusiness.Framework.Configuracao.Configuracao._bancoLog = bancoLog;

            if (chave == Configuracao.Configuracao._chaveArmazenada)
                Configuracao.Configuracao._habilitaAcessoDadosPrivados = true;

            try
            {
                if (arquivosInicializacao != null)
                    CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao = arquivosInicializacao;

                CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao = tipo;
                CaseBusiness.Framework.Configuracao.Configuracao._aplicacao = aplicacao;
                CaseBusiness.Framework.Configuracao.Configuracao._usuarioAplicacao = definirUsuarioAplicacao(aplicacao);

                try
                {
                    CaseBusiness.Framework.Configuracao.Configuracao._cliente = (CaseBusiness.Framework.Cliente)Convert.ToInt32(CaseBusiness.CB.AppSettings["Cliente"]);
                }
                catch (System.Exception ex)
                {
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Cliente não identificado. " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (VerificarArquivosInicializacao())
                {
                    foreach (String itemArquivo in arquivosInicializacao)
                    {
                        switch (itemArquivo)
                        {
                            case "configuracao.xml":
                                new BancoDados.Processo.Configuracao().CarregarStringsConexao();

                                if (verificaComunicacao)
                                    new CaseBusiness.Framework.Comunicacao.Comunicacao().IdentificaComunicacaoAplicacaoLocalRemota();

                                //new Configuracao.Processo.Configuracao().CarregarConfiguracao(aplicacao);
                                break;

							case "configuracao_pci.xml":
								new CaseBusiness.Framework.Criptografia.Cript();

								if (!Configuracao.Configuracao.pciCarregado)
									return false;
								break;
						}
                    }
                }
                else
                    apto = false;

				if (!VerificarArquivosAdicionaisInicializacao())
					return false;
			}
            catch (System.Exception ex)
            {
                apto = false;
				Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			}

            return apto;
        }

		internal Int32 definirUsuarioAplicacao(App app)
        {
            switch (app)
            {
                case App.Nenhum:
                    return 0;
                case App.SMSServicoDispatcher:
                    return -49;
                case App.SMSApi:
                    return -50;
                case App.SMSServico:
                    return -51;
                default:
                    return Int32.MinValue;
            }
        }

        internal Boolean VerificarArquivosInicializacao()
        {
            Boolean aptoInicializacao = true;

            try
            {
                String msg = "";
                String caminho = CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig;
				Boolean arqExiste = false;
				Boolean arqPermitidoAcesso = false;

				//if (CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao == TipoAplicacao.Web)
    //                caminho = AppDomain.CurrentDomain.BaseDirectory;
    //            else
    //                caminho = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location);
                
                DirectoryInfo dir = new DirectoryInfo(caminho);
                FileInfo[] files = dir.GetFiles();

                foreach (String arq in CaseBusiness.Framework.Configuracao.Configuracao.ArquivosInicializacao)
                {
					arqExiste = false;
					arqPermitidoAcesso = false;

					foreach (FileInfo f in files)
					{
						if (f.Name.ToUpper() == arq.ToUpper())
						{
							arqExiste = true;

							if (f.Extension.ToUpper() == ".XML")
							{
								try
								{
									System.Data.DataSet cnConsulta = new System.Data.DataSet();

                                    cnConsulta.ReadXml(CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig + @"/" + f.Name);


         //                           if (Configuracao.Configuracao._tipoAplicacao == TipoAplicacao.Web)
									//	cnConsulta.ReadXml(AppDomain.CurrentDomain.BaseDirectory + @"/" + f.Name);
									//else
									//	cnConsulta.ReadXml(Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location) + @"/" + f.Name);

									arqPermitidoAcesso = true;
								}
								catch (System.Exception ex)
								{
									Log.Log.LogarArquivo("Não permitido acesso ao arquivo de inicialização: " + ex.Message, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
									arqPermitidoAcesso = false;
								}
							}

							break;
						}
					}

					if (!arqExiste || !arqPermitidoAcesso)
					{
						aptoInicializacao = false;

						if (msg.Length == 0)
							msg += arq;
						else
							msg += ", " + arq;
					}
				}

                try
                {
                    String nomeArquivosTestePermissao = "Case_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")  + ".log";

                    StreamWriter str = new StreamWriter(caminho + @"/" + nomeArquivosTestePermissao,true);
                    str.Write("Teste de permissão de leitura/escrita no diretório de execução da aplicação");
                    str.Close();

                    File.Delete(caminho + @"/" + nomeArquivosTestePermissao);
                }
                catch (System.Exception ex)
                {
                    aptoInicializacao = false;
                    CaseBusiness.Framework.Log.Log.LogarArquivo("Serviço não possui permissão de leitura/escrita no diretório " + caminho + " de execução da aplicação. " + ex.Message, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                }

                if (!aptoInicializacao)
                {
					if (Configuracao.Configuracao.TipoAplicacao == TipoAplicacao.Servico)
						Log.Log.LogarArquivo("Impossível inicializar. Os arquivos listados não foram localizados ou estão inacessíveis: " + msg, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
					else
						Configuracao.Configuracao._erroInicializacao = "Impossível inicializar. Os arquivos listados não foram localizados ou estão inacessíveis: " + msg;

					return aptoInicializacao;
                }

                aptoInicializacao = true;
            }
            catch (System.Exception ex)
            {
                aptoInicializacao = false;

                Log.Log.LogarArquivo("Erro: " + ex.Message + " " + ex.StackTrace, CaseBusiness.Framework.Log.TipoEventoLog.Erro, "Case Framework");
                throw;
            }

            return aptoInicializacao;
        }

        internal Boolean VerificarArquivosAdicionaisInicializacao()
        {
            Boolean arquivosAdicionaisEncontrados = true;
			Boolean aptoInicializacao = true;

			//try
			//{
			//	String caminho = CaseBusiness.Framework.Configuracao.Configuracao.CaminhoConfig;

			//	//if (CaseBusiness.Framework.Configuracao.Configuracao._tipoAplicacao == TipoAplicacao.Web)
			//	//	caminho = AppDomain.CurrentDomain.BaseDirectory;
			//	//else
			//	//	caminho = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location);

			//	//Se criptografia complementar é do tipo Voltage, necessário que tenha o arquivo parametros.xml
			//	if (CaseBusiness.Framework.Configuracao.Configuracao.ComplementaryCryptType == ComplementaryCryptType.Voltage)
			//	{
			//		DirectoryInfo dir = new DirectoryInfo(caminho);
			//		FileInfo[] files = dir.GetFiles();

			//		arquivosAdicionaisEncontrados = false;

			//		foreach (FileInfo f in files)
			//		{
			//			switch (f.Name.ToUpper())
			//			{
			//				case "PARAMETROS.XML":
			//					if (new Configuracao.Processo.Configuracao().CarregarParametros())
			//					{
			//						try
			//						{
			//							new Criptografia.Cript(CaseBusiness.Framework.Configuracao.Configuracao.ComplementaryCryptType).Codificar("1234567890123456");
			//							arquivosAdicionaisEncontrados = true;
			//						}
			//						catch (System.Exception ex)
			//						{
			//							CaseBusiness.Framework.Configuracao.Configuracao._erroInicializacao = "Erro no acesso ao serviço de criptografia: " + ex.Message;
			//							Log.Log.Logar(TipoLog.Erro, "Não permitido acesso ao serviço de criptografia: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			//						}
			//					}
								
			//					break;
			//				default:
			//					break;
			//			}
			//		}
			//	}

			//	if (!arquivosAdicionaisEncontrados)
			//	{
			//		aptoInicializacao = false;
			//		CaseBusiness.Framework.Configuracao.Configuracao._erroInicializacao = "Arquivo parametros.xml não localizado/acessível ou serviço de criptografia indisponível. Não é possível iniciar.";
			//		Log.Log.Logar(TipoLog.Aviso, "Arquivo parametros.xml não localizado/acessível ou serviço de criptografia indisponível. Não é possível iniciar.", "", "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			//	}
   //         }
   //         catch (System.Exception ex)
   //         {
			//	CaseBusiness.Framework.Configuracao.Configuracao._erroInicializacao = "Erro: " + ex.Message;
			//	Log.Log.Logar(TipoLog.Erro, "Erro: " + ex.Message, ex.StackTrace, "", "", DateTime.Now, Configuracao.Configuracao._aplicacao, Tela.Nenhum, 0);
			//	aptoInicializacao = false;
			//}

			return aptoInicializacao;
		}
    }
}
