#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Runtime.Caching;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Mensageria
{
    public class Configuracao : BusinessBase
    {
        #region MemoryCache
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 120;  // 60 minutos * 24 horas
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 30;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idConfiguracaoEnvio = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private String _nomeOrganizacao = String.Empty;
        private String _codigoNomeOrganizacao = String.Empty;
        private Int32 _idCanal = Int32.MinValue;
        private String _nomeCanal = String.Empty;

        // Cliente
        private DateTime _horaEnvioInicioCliente = DateTime.MinValue;
        private DateTime _horaEnvioFimCliente = DateTime.MinValue;
        private Int32 _quantidadeMaximaMensagemEnviadaTransacaoCliente = Int32.MinValue;

        //Teste
        private DateTime _horaEnvioInicioTeste = DateTime.MinValue;
        private DateTime _horaEnvioFimTeste = DateTime.MinValue;

        private DataTable _dtRestricoesAlteracao = null;
        #endregion Atributos

        #region Propriedades
        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public String NomeOrganizacao
        {
            get { return _nomeOrganizacao; }
            set { _nomeOrganizacao = value; }
        }

        public String CodigoNomeOrganizacao
        {
            get { return _codigoNomeOrganizacao; }
            set { _codigoNomeOrganizacao = value; }
        }

        public Int32 idCanal
        {
            get { return _idCanal; }
            set { _idCanal = value; }
        }

        public String NomeCanal
        {
            get { return _nomeCanal; }
            set { _nomeCanal = value; }
        }

        public DateTime HoraEnvioInicioCliente
        {
            get { return _horaEnvioInicioCliente; }
            set { _horaEnvioInicioCliente = value; }
        }

        public DateTime HoraEnvioFimCliente
        {
            get { return _horaEnvioFimCliente; }
            set { _horaEnvioFimCliente = value; }
        }

        public Int32 QuantidadeMaximaMensagemEnviadaTransacaoCliente
        {
            get { return _quantidadeMaximaMensagemEnviadaTransacaoCliente; }
            set { _quantidadeMaximaMensagemEnviadaTransacaoCliente = value; }
        }
        
        public DateTime HoraEnvioInicioTeste
        {
            get { return _horaEnvioInicioTeste; }
            set { _horaEnvioInicioTeste = value; }
        }

        public DateTime HoraEnvioFimTeste
        {
            get { return _horaEnvioFimTeste; }
            set { _horaEnvioFimTeste = value; }
        }

        public Boolean AlteracaoPermitida
        {
            get
            {
                if (!IsLoaded)
                {
                    _dtRestricoesAlteracao = null;
                    return false;
                }

                if (RestricaoAlteracao == null)
                {
                    return false;
                }
                else
                {
                    if (_dtRestricoesAlteracao.Rows.Count <= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public DataTable RestricaoAlteracao
        {
            get
            {
                if (_dtRestricoesAlteracao == null)
                {
                    _dtRestricoesAlteracao = new DataTable();
                    _dtRestricoesAlteracao = ObterRestricaoAlteracao();
                }

                return _dtRestricoesAlteracao;
            }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Configuracao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Configuracao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará Configuracao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public Configuracao()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Configuracao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Configuracao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Configuracao(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe Configuracao e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Configuracao(Int32 codigoOrganizacao, Int32 idCanal, Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(codigoOrganizacao,
                      idCanal);
        }

        /// <summary>
        /// Construtor classe Configuracao utilizando uma Transação e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Configuracao(Int32 codigoOrganizacao, Int32 idCanal, Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
            : this(idUsuarioManutencao, transacao)
        {
            Consultar(codigoOrganizacao,
                      idCanal);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Incluir uma Configuração
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id da Canal</param>
        /// <param name="horaEnvioInicioCliente">Hora Envio Início Cliente</param>
        /// <param name="horaEnvioFimCliente">Hora Envio Fim Cliente</param>
        /// <param name="horaEnvioInicioTeste">Hora Envio Início Teste</param>
        /// <param name="horaEnvioFimTeste">Hora Envio Fim Teste</param>
        public Int32 IncluirConfiguracao(Int32 codigoOrganizacao,
                                        Int32 idCanal,
                                        DateTime horaEnvioInicioCliente,
                                        DateTime horaEnvioFimCliente,
                                        DateTime horaEnvioInicioTeste,
                                        DateTime horaEnvioFimTeste)
        {
            _idConfiguracaoEnvio = Incluir(codigoOrganizacao,
                                      idCanal,
                                      horaEnvioInicioCliente,
                                      horaEnvioFimCliente,
                                      horaEnvioInicioTeste,
                                      horaEnvioFimTeste);

            return _idConfiguracaoEnvio;
        }

        /// <summary>
        /// Incluir uma Configuração
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id da Canal</param>
        /// <param name="horaEnvioInicioCliente">Hora Envio Início Cliente</param>
        /// <param name="horaEnvioFimCliente">Hora Envio Fim Cliente</param>
        /// <param name="horaEnvioInicioTeste">Hora Envio Início Teste</param>
        /// <param name="horaEnvioFimTeste">Hora Envio Fim Teste</param>
        /// <returns>Id Configuração de Envio</returns>
        private Int32 Incluir(Int32 codigoOrganizacao,
                             Int32 idCanal,
                             DateTime horaEnvioInicioCliente,
                             DateTime horaEnvioFimCliente,
                             DateTime horaEnvioInicioTeste,
                             DateTime horaEnvioFimTeste)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organizacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCFG_CLIEN_DH_ENVIO_INI", horaEnvioInicioCliente);
                acessoDadosBase.AddParameter("@MGCFG_CLIEN_DH_ENVIO_FIM", horaEnvioFimCliente);
                acessoDadosBase.AddParameter("@MGCFG_TST_DH_ENVIO_INI", horaEnvioInicioTeste);
                acessoDadosBase.AddParameter("@MGCFG_TST_DH_ENVIO_FIM", horaEnvioFimTeste);
                acessoDadosBase.AddParameter("@MGCFG_ID", _idConfiguracaoEnvio, ParameterDirection.Output);

                _idConfiguracaoEnvio = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGCFG_INS")[0]);


                return _idConfiguracaoEnvio;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Altera uma Configuração
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id da Canal</param>
        /// <param name="horaEnvioInicioCliente">Hora Envio Início Cliente</param>
        /// <param name="horaEnvioFimCliente">Hora Envio Fim Cliente</param>
        /// <param name="horaEnvioInicioTeste">Hora Envio Início Teste</param>
        /// <param name="horaEnvioFimTeste">Hora Envio Fim Teste</param>
        public void AlterarConfiguracao(Int32 codigoOrganizacao,
                                        Int32 idCanal,
                                        DateTime horaEnvioInicioCliente,
                                        DateTime horaEnvioFimCliente,
                                        DateTime horaEnvioInicioTeste,
                                        DateTime horaEnvioFimTeste)
        {
            Alterar(codigoOrganizacao,
                    idCanal,
                    horaEnvioInicioCliente,
                    horaEnvioFimCliente,
                    horaEnvioInicioTeste,
                    horaEnvioFimTeste);
        }

        /// <summary>
        /// Altera uma Configuração
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id da Canal</param>
        /// <param name="horaEnvioInicioCliente">Hora Envio Início Cliente</param>
        /// <param name="horaEnvioFimCliente">Hora Envio Fim Cliente</param>
        /// <param name="horaEnvioInicioTeste">Hora Envio Início Teste</param>
        /// <param name="horaEnvioFimTeste">Hora Envio Fim Teste</param>
        private void Alterar(Int32 codigoOrganizacao,
                             Int32 idCanal,
                             DateTime horaEnvioInicioCliente,
                             DateTime horaEnvioFimCliente,
                             DateTime horaEnvioInicioTeste,
                             DateTime horaEnvioFimTeste)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Organizacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@MGCFG_CLIEN_DH_ENVIO_INI", horaEnvioInicioCliente);
                acessoDadosBase.AddParameter("@MGCFG_CLIEN_DH_ENVIO_FIM", horaEnvioFimCliente);
                acessoDadosBase.AddParameter("@MGCFG_TST_DH_ENVIO_INI", horaEnvioInicioTeste);
                acessoDadosBase.AddParameter("@MGCFG_TST_DH_ENVIO_FIM", horaEnvioFimTeste);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMGCFG_UPD");

            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Obtem as Restrições de Alteração da Conexção
        /// </summary>
        private DataTable ObterRestricaoAlteracao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@ORG_CD", CodigoOrganizacao);

                dt = new DataTable();
                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGCFG_SEL_RESTRIC_UPD").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Buscar Configuração de Envio
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="idCanal">Id da Canal</param>
        public DataTable Buscar(Int32 codigoOrganizacao,
                                Int32 idCanal)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGCFG_SEL_BUSCAR").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }


        /// <summary>
        /// Consultar uma Configuracao de Envio por Código da Organização
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        private void Consultar(Int32 codigoOrganizacao,
                               Int32 idCanal)
        {
            try
            {
                DataTable dt = null;
                String kCacheKey = "MensagemConfiguracao_" + codigoOrganizacao.ToString() + idCanal.ToString();

                if (MemoryCache.Default.Contains(kCacheKey))
                {
                    dt = MemoryCache.Default[kCacheKey] as DataTable;
                }
                else
                {

                    if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                    acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                    acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);

                    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGCFG_SEL_CONSULTARID").Tables[0];
                }

                // Fill Object
                PreencherAtributos(ref dt);

                if (__blnIsLoaded)
                {
                    MemoryCache.Default.Set(kCacheKey, dt,
                    new CacheItemPolicy()
                    {
                        SlidingExpiration = new TimeSpan(DateTime.Now.AddMinutes(kCache_SLIDINGEXPIRATION_MINUTES).Ticks - DateTime.Now.Ticks)
                    });
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                NomeOrganizacao = (String)dt.Rows[0]["ORG_NM"];
                CodigoNomeOrganizacao = (String)dt.Rows[0]["ORG_CD_NM"];
                idCanal = Convert.ToInt32(dt.Rows[0]["COMCNL_ID"]);
                NomeCanal = (String)dt.Rows[0]["COMCNL_NM"];

                // Cliente
                HoraEnvioInicioCliente = Convert.ToDateTime(dt.Rows[0]["MGCFG_CLIEN_DH_ENVIO_INI"]);
                HoraEnvioFimCliente = Convert.ToDateTime(dt.Rows[0]["MGCFG_CLIEN_DH_ENVIO_FIM"]);

                // Teste
                HoraEnvioInicioTeste = Convert.ToDateTime(dt.Rows[0]["MGCFG_TST_DH_ENVIO_INI"]);
                HoraEnvioFimTeste = Convert.ToDateTime(dt.Rows[0]["MGCFG_TST_DH_ENVIO_FIM"]);

                __blnIsLoaded = true;
            }
        }


        public String RestricaoCliente(Int32 codigoOrganizacao,
                                       Int32 idCanal,
                                       Int32 codigoConfiguracaoMensagem,
                                       String cpfCnpj,
                                       String destinatario,
                                       String numeroCartao,
                                       String segmento,
                                       Int64 transacao,
                                       Int32 idGrupoTeste)
        {
            String listaRestricoesCliente = "";

            try
            {


                // Busca dados da MensagemConfiguraca
                CaseBusiness.CC.Mensageria.ConfiguracaoComunicacao configuracaoMensagem = new CaseBusiness.CC.Mensageria.ConfiguracaoComunicacao(codigoConfiguracaoMensagem, codigoOrganizacao, UsuarioManutencao.ID);

                // Busca se está na lista de restrições
                listaRestricoesCliente = new CaseBusiness.CC.Mensageria.Restricao(base.UsuarioManutencao.ID).RestricaoDefinitiva(cpfCnpj, destinatario);

                // Carrega dados da configuração de Envio de Mensagem de um tipo de Canal de uma Organização.
                CaseBusiness.CC.Mensageria.Configuracao confMensageria = new CaseBusiness.CC.Mensageria.Configuracao(codigoOrganizacao, idCanal, UsuarioManutencao.ID);

                // Estourou qtde max msg trs nas ultimas 24h 
                Int32 qtdeMsgTrs = new CaseBusiness.CC.Mensageria.MensagemSMSLog(base.UsuarioManutencao.ID).QuantidadeMensagensEnviada("CLIEN", cpfCnpj, "TRANSACAO", idGrupoTeste, 1440);
                if (qtdeMsgTrs >= confMensageria.QuantidadeMaximaMensagemEnviadaTransacaoCliente)
                {
                    listaRestricoesCliente = listaRestricoesCliente + "Excedido quantidade máxima de " + confMensageria.QuantidadeMaximaMensagemEnviadaTransacaoCliente.ToString() + " mensagem(ns) enviado(s) por transação no período de 24 horas para o cliente. ";
                }

                var horarioAtual = TimeSpan.Parse(DateTime.Now.ToString("0:HH:mm:ss"));
                var horarioInicio = TimeSpan.Parse(confMensageria.HoraEnvioInicioCliente.ToString("0:HH:mm:ss"));
                var horarioFim = TimeSpan.Parse(confMensageria.HoraEnvioFimCliente.ToString("0:HH:mm:ss"));

                // Verificar restricao temporaria - Faixa de horario
                if (horarioAtual < horarioInicio | horarioAtual > horarioFim)
                {
                    DateTime dhHoraLiberadoEnvio = DateTime.MinValue;

                    // Se horário atual for maior que horário fim, significa que ainda está no mesmo dia. Soma 1 no dia e determina horário de envio
                    if (horarioAtual > horarioFim)
                    {
                        dhHoraLiberadoEnvio = DateTime.Now.AddDays(1);
                        dhHoraLiberadoEnvio = new DateTime(dhHoraLiberadoEnvio.Year, dhHoraLiberadoEnvio.Month, dhHoraLiberadoEnvio.Day, confMensageria.HoraEnvioInicioCliente.Hour, confMensageria.HoraEnvioInicioCliente.Minute, 0);
                    }
                    // Se horário atual for menor que horário inicial, significa que esta no dia. Apenas determina horário de envio
                    else if (horarioAtual < horarioInicio)
                    {
                        dhHoraLiberadoEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, confMensageria.HoraEnvioInicioCliente.Hour, confMensageria.HoraEnvioInicioCliente.Minute, 0);
                    }

                    listaRestricoesCliente = listaRestricoesCliente + "Mensagem somente poderá ser enviada às " + dhHoraLiberadoEnvio.ToString("dd/MM/yyyy HH:mm:ss") + ".";
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                listaRestricoesCliente = ex.Message;
                throw;
            }

            return listaRestricoesCliente;
        }

        public String RestricaoTeste(Int32 codigoOrganizacao,
                                         Int32 idCanal,
                                         Int32 codigoConfiguracaoMensagem,
                                         String cpfCnpj,
                                         String destinatario,
                                         String numeroCartao,
                                         String segmento,
                                         Int64 transacao,
                                         Int32 idGrupoTeste)
        {
            String listaRestricoesTeste = "";

            try
            {
                // Carrega dados da configuração de Envio de Mensagem de um tipo de Canal de uma Organização.
                CaseBusiness.CC.Mensageria.Configuracao confMensageria = new CaseBusiness.CC.Mensageria.Configuracao(codigoOrganizacao, idCanal, UsuarioManutencao.ID);

                var horarioAtual = TimeSpan.Parse(DateTime.Now.ToString("0:HH:mm:ss"));
                var horarioInicio = TimeSpan.Parse(confMensageria.HoraEnvioInicioTeste.ToString("0:HH:mm:ss"));
                var horarioFim = TimeSpan.Parse(confMensageria.HoraEnvioFimTeste.ToString("0:HH:mm:ss"));

                // Verificar restricao temporaria - Faixa de horario
                if (horarioAtual < horarioInicio | horarioAtual > horarioFim)
                {
                    DateTime dhHoraLiberadoEnvio = DateTime.MinValue;

                    //Se horário atual for maior que horário fim, significa que ainda está no mesmo dia. Soma 1 no dia e determina horário de envio
                    if (horarioAtual > horarioFim)
                    {
                        dhHoraLiberadoEnvio = DateTime.Now.AddDays(1);
                        dhHoraLiberadoEnvio = new DateTime(dhHoraLiberadoEnvio.Year, dhHoraLiberadoEnvio.Month, dhHoraLiberadoEnvio.Day, confMensageria.HoraEnvioInicioTeste.Hour, confMensageria.HoraEnvioInicioTeste.Minute, 0);
                    }
                    // Se horário atual for menor que horário inicial, significa que esta no dia. Apenas determina horário de envio
                    else if (horarioAtual < horarioInicio)
                    {
                        dhHoraLiberadoEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, confMensageria.HoraEnvioInicioTeste.Hour, confMensageria.HoraEnvioInicioTeste.Minute, 0);
                    }

                    listaRestricoesTeste = listaRestricoesTeste + "Mensagem somente poderá ser enviada às " + dhHoraLiberadoEnvio.ToString("dd/MM/yyyy HH:mm:ss") + ".";
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                listaRestricoesTeste = ex.Message;
                throw;
            }

            return listaRestricoesTeste;
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "NomeOrganizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "idCanal"; }
            if (dt.Columns.Contains("COMCNL_NM")) { dt.Columns["COMCNL_NM"].ColumnName = "NomeCanal"; }

            //Cliente
            if (dt.Columns.Contains("MGCFG_CLIEN_DH_ENVIO_INI")) { dt.Columns["MGCFG_CLIEN_DH_ENVIO_INI"].ColumnName = "HoraEnvioInicioCliente"; }
            if (dt.Columns.Contains("MGCFG_CLIEN_DH_ENVIO_FIM")) { dt.Columns["MGCFG_CLIEN_DH_ENVIO_FIM"].ColumnName = "HoraEnvioFimCliente"; }

            // Teste
            if (dt.Columns.Contains("MGCFG_TST_DH_ENVIO_INI")) { dt.Columns["MGCFG_TST_DH_ENVIO_INI"].ColumnName = "HoraEnvioInicioTeste"; }
            if (dt.Columns.Contains("MGCFG_TST_DH_ENVIO_FIM")) { dt.Columns["MGCFG_TST_DH_ENVIO_FIM"].ColumnName = "HoraEnvioFimTeste"; }

            if (dt.Columns.Contains("ROWS_COUNT_UPD")) { dt.Columns["ROWS_COUNT_UPD"].ColumnName = "RestricoesAlteracaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA_UPD")) { dt.Columns["RESTRICAO_TABELA_UPD"].ColumnName = "RestricoesAlteracaoTabela"; }
        }
        #endregion Métodos
    }
}
