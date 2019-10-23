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

namespace CaseBusiness.CC.Comunicacao
{
    public class FornecedoraOrg : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "FornecedoraOrg_";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 120;  // 60 minutos * 24 horas
        private const Int16 kCache_SLIDINGEXPIRATION_MINUTES = 30;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idFornecedora = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private Int32 _prioridade = Int32.MinValue;
        private String _usuarioConexao = String.Empty;
        private String _senhaConexao = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private String _nomeUsuarioInclusao = String.Empty;
        private DateTime _dataInclusao = DateTime.MinValue;
        private Int32 _idUsuarioAlteracao = Int32.MinValue;
        private String _nomeUsuarioAlteracao = String.Empty;
        private DateTime _dataAlteracao = DateTime.MinValue;
        private String _status = String.Empty;
        private String _mensagemRetorno = String.Empty;
        #endregion Atributos

        #region Propriedades
        public Int32 IdFornecedora
        {
            get { return _idFornecedora; }
            set { _idFornecedora = value; }
        }

        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public Int32 Prioridade
        {
            get { return _prioridade; }
            set { _prioridade = value; }
        }

        public String UsuarioConexao
        {
            get { return _usuarioConexao; }
            set { _usuarioConexao = value; }
        }

        public String SenhaConexao
        {
            get { return _senhaConexao; }
            set { _senhaConexao = value; }
        }

        public Int32 IdUsuarioInclusao
        {
            get { return _idUsuarioInclusao; }
            set { _idUsuarioInclusao = value; }
        }

        public String NomeUsuarioInclusao
        {
            get { return _nomeUsuarioInclusao; }
            set { _nomeUsuarioInclusao = value; }
        }

        public DateTime DataInclusao
        {
            get { return _dataInclusao; }
            set { _dataInclusao = value; }
        }

        public Int32 IdUsuarioAlteracao
        {
            get { return _idUsuarioAlteracao; }
            set { _idUsuarioAlteracao = value; }
        }

        public String NomeUsuarioAlteracao
        {
            get { return _nomeUsuarioAlteracao; }
            set { _nomeUsuarioAlteracao = value; }
        }

        public DateTime DataAlteracao
        {
            get { return _dataAlteracao; }
            set { _dataAlteracao = value; }
        }

        public String Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public String MensagemRetorno
        {
            get { return _mensagemRetorno; }
            set { _mensagemRetorno = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe FornecedoraOrg - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public FornecedoraOrg(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará FornecedoraOrg com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe FornecedoraOrg
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public FornecedoraOrg()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe FornecedoraOrg
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public FornecedoraOrg(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe FornecedoraOrg utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public FornecedoraOrg(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe FornecedoraOrg e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Cóodigo da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public FornecedoraOrg(Int32 idFornecedora, Int32 codigoOrganizacao, Int32 prioridade, Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idFornecedora, codigoOrganizacao, prioridade);
        }

        /// <summary>
        /// Construtor classe FornecedoraOrg e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Cóodigo da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="tipo">Tipo de Pesquisa</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public FornecedoraOrg(Int32 idCanal, Int32 idFornecedora, Int32 codigoOrganizacao, Int32 prioridade, String tipo, Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idCanal, idFornecedora, codigoOrganizacao, prioridade, tipo);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Inclui uma FornecedoraOrg
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="usuarioConexao">Usuário de Conexão</param>
        /// <param name="senhaConexao">Senha de Conexão</param>
        /// <param name="dataInclusao">Data/Hora Inclusão</param>
        /// <param name="status">Status_</param>
        public void IncluirFornecedoraOrg(Int32 idFornecedora,
                                          Int32 codigoOrganizacao,
                                          Int32 prioridade,
                                          String usuarioConexao,
                                          String senhaConexao,
                                          DateTime dataInclusao,
                                          String status)
        {
            Incluir(idFornecedora, codigoOrganizacao, prioridade, usuarioConexao, senhaConexao, dataInclusao, status);
        }

        /// <summary>
        /// Inclui uma FornecedoraOrg
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="usuarioConexao">Usuário de Conexão</param>
        /// <param name="senhaConexao">Senha de Conexão</param>
        /// <param name="dataInclusao">Data/Hora Inclusão</param>
        /// <param name="status">Status_</param>
        private void Incluir(Int32 idFornecedora,
                             Int32 codigoOrganizacao,
                             Int32 prioridade,
                             String usuarioConexao,
                             String senhaConexao,
                             DateTime dataInclusao,
                             String status)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Conexão está operando em Modo Entidade Only"); }

                #region Criptografando
                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                senhaConexao = pci.Codificar(senhaConexao);

                pci = null;
                #endregion Criptografando

                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMFORNORG_PRIORIDADE", prioridade);
                acessoDadosBase.AddParameter("@COMFORNORG_USU", usuarioConexao.Trim());
                acessoDadosBase.AddParameter("@COMFORNORG_SENHA", senhaConexao.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@COMFORNORG_DH_USUARIO_INS", dataInclusao);
                acessoDadosBase.AddParameter("@COMFORNORG_ST", status.Trim());

                acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prCOMFORNORG_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Altera uma FornecedoraOrg
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Códio da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="usuarioConexao">Usuário de Conexão</param>
        /// <param name="senhaConexao">Senha de Conexão</param>
        /// <param name="status">Status da Conexão</param>
        /// <param name="dataAlteracao">Data Hora Alteracao</param>
        public void AlterarFornecedoraOrg(Int32 idFornecedora,
                                          Int32 codigoOrganizacao,
                                          Int32 prioridade,
                                          String usuarioConexao,
                                          String senhaConexao,
                                          String status,
                                          DateTime dataAlteracao)
        {
            Alterar(idFornecedora, codigoOrganizacao, prioridade, usuarioConexao, senhaConexao, status, dataAlteracao);
        }

        /// <summary>
        /// Altera uma FornecedoraOrg
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="usuarioConexao">Usuário de Conexão</param>
        /// <param name="senhaConexao">Senha de Conexão</param>
        /// <param name="status">Status da Conexão</param>
        /// <param name="dataAlteracao">Data Hora Alteracao</param>
        private void Alterar(Int32 idFornecedora,
                             Int32 codigoOrganizacao,
                             Int32 prioridade,
                             String usuarioConexao,
                             String senhaConexao,
                             String status,
                             DateTime dataAlteracao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Conexão está operando em Modo Entidade Only"); }

                #region Criptografando
                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                if (!String.IsNullOrEmpty(senhaConexao))
                    senhaConexao = pci.Codificar(senhaConexao);
                else
                    senhaConexao = "";

                pci = null;
                #endregion Criptografando

                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMFORNORG_PRIORIDADE", prioridade);
                acessoDadosBase.AddParameter("@COMFORNORG_USU", usuarioConexao.Trim());
                acessoDadosBase.AddParameter("@COMFORNORG_SENHA", senhaConexao.Trim());
                acessoDadosBase.AddParameter("@USU_ID_UPD", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@COMFORNORG_DH_USUARIO_UPD", dataAlteracao);
                acessoDadosBase.AddParameter("@COMFORNORG_ST", status);

                acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prCOMFORNORG_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar 'ORG / Fornecedora / Conexão' 
        /// </summary>        
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="usuarioConexao">Usuário de Conexão</param>
        public DataTable Buscar(Int32 idCanal,
                                Int32 idFornecedora,
                                Int32 codigoOrganizacao,
                                Int32 prioridade,
                                String usuarioConexao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FornecedoraOrg está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMFORNORG_PRIORIDADE", prioridade);
                acessoDadosBase.AddParameter("@COMFORNORG_USU", usuarioConexao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORNORG_SEL_BUSCAR").Tables[0];

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
        /// Buscar Fornecedora do Canal Selecionada
        /// </summary>      
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="idCanal">Id do Canal</param>
        public DataTable BuscarCanalFornecedora(Int32 idFornecedora,
                                                Int32 idCanal)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FornecedoraOrg está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORNORG_SEL_BUSCAR_FORN").Tables[0];

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
        /// Buscar conexão da fornecedora através canal/organização/status
        /// </summary>
        /// <param name="codigoFornecedor">Código do Fornecedor (Ex.: TWW = TWW BRASIL, 2RP = 2RP NET SERVIÇOS </param>
        /// <param name="idCanal"></param>
        /// <param name="codigoOrganizacao"></param>
        /// <param name="status"></param>
        /// <returns>Retorna um DataTable</returns>
        public DataTable BuscarAcesso(String codigoFornecedor,
                                      Int32 idCanal,
                                      Int32 codigoOrganizacao,
                                      String status)
        {
            DataTable dt = null;

            String _kCacheKey = kCacheKey + codigoFornecedor + idCanal.ToString() + codigoOrganizacao.ToString() + status;

            try
            {
                if (MemoryCache.Default.Contains(_kCacheKey))
                {
                    dt = MemoryCache.Default[_kCacheKey] as DataTable;
                }
                else
                {
                    if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FornecedoraOrg está operando em Modo Entidade Only"); }

                    CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                    acessoDadosBase.AddParameter("@COMFORN_CD", codigoFornecedor);
                    acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                    acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                    acessoDadosBase.AddParameter("@COMFORNORG_ST", status.Trim());

                    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORNORG_SEL_BUSCARACESSO").Tables[0];

                    // Renomear Colunas
                    RenomearColunas(ref dt);

                    #region DesCriptografando
                    foreach (DataRow dr in dt.Rows)
                    {
                        //Descriptogranado senha
                        if (!String.IsNullOrEmpty(Convert.ToString(dr["SenhaConexao"])))
                        {
                            _senhaConexao = Convert.ToString(dr["SenhaConexao"]);
                            dr["SenhaConexao"] = pci.Decodificar(_senhaConexao);

                            if (String.IsNullOrEmpty(Convert.ToString(dr["SenhaConexao"])))
                            {
                                // ERRO DE DECRIPT
                                dr["SenhaConexao"] = "!!ERRO CRYPT!! " + _senhaConexao;
                            }
                        }
                    }
                    #endregion DesCriptografando

                    MemoryCache.Default.Set(_kCacheKey, dt,
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

            return dt;
        }

        /// <summary>
        /// Buscar conexão da fornecedora através canal/organização/status
        /// </summary>
        /// <param name="codigoFornecedor">Código do Fornecedor (Ex.: TWW para TWW DO BRASIL, 2RP para 2RP NET SERVIÇOS, etc</param>
        /// <param name="idCanal"></param>
        /// <param name="status"></param>
        /// <returns>Retorna um DataTable</returns>
        public DataTable BuscarAcessoFornecedor(String codigoFornecedor,
                                                Int32 idCanal,
                                                String status)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FornecedoraOrg está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                acessoDadosBase.AddParameter("@COMFORN_CD", codigoFornecedor.Trim());
                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@COMFORNORG_ST", status.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORNORG_SEL_CONEXAO").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region DesCriptografando
                foreach (DataRow dr in dt.Rows)
                {
                    //Descriptogranado senha
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["SenhaConexao"])))
                    {
                        _senhaConexao = Convert.ToString(dr["SenhaConexao"]);
                        dr["SenhaConexao"] = pci.Decodificar(_senhaConexao);

                        if (String.IsNullOrEmpty(Convert.ToString(dr["SenhaConexao"])))
                        {
                            // ERRO DE DECRIPT
                            dr["SenhaConexao"] = "!!ERRO CRYPT!! " + _senhaConexao;
                        }
                    }
                }
                #endregion DesCriptografando
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Consultar uma FornecedoraOrg por ID, Código da Organização e Prioridade
        /// </summary>
        /// <param name="idFornecedora">ID da Fornecedora</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        private void Consultar(Int32 idFornecedora,
                               Int32 codigoOrganizacao,
                               Int32 prioridade)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FornecedoraOrg está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMFORNORG_PRIORIDADE", prioridade);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORNORG_SEL_CONSULTARID").Tables[0];

                // Fill Object
                PreencherAtributos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consultar uma FornecedoraOrg para inclusão e/ou alteração
        /// </summary>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Cóodigo da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="tipo">Tipo de Pesquisa</param>
        private void Consultar(Int32 idCanal,
                               Int32 idFornecedora,
                               Int32 codigoOrganizacao,
                               Int32 prioridade,
                               String tipo)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FornecedoraOrg está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@COMFORNORG_PRIORIDADE", prioridade);
                acessoDadosBase.AddParameter("@COMFORNORG_TP_PESQUISA", tipo);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORNORG_SEL_CON_CONEXAO").Tables[0];

                // Fill Object
                MensagemRetorno = (String)dt.Rows[0]["COMFORNORG_MSG_RETORNO"];
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consultar uma FornecedoraOrg por Canal e Organização
        /// </summary>
        /// <param name="idCanal">Id do Canal</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public void ConsultarCanalOrg(Int32 idCanal,
                                      Int32 codigoOrganizacao)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FornecedoraOrg está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMCNL_ID", idCanal);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORNORG_SEL_CON_PLATORG").Tables[0];

                // Fill Object
                PreencherAtributos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Remove do Cache as Moedas
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }

        /// <summary>
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                CaseBusiness.Framework.Criptografia.Cript pci = new Framework.Criptografia.Cript(PCI_ConfigPath, "");

                IdFornecedora = Convert.ToInt32(dt.Rows[0]["COMFORN_ID"]);
                CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                Prioridade = Convert.ToInt32(dt.Rows[0]["COMFORNORG_PRIORIDADE"]);

                if (dt.Rows[0]["COMFORNORG_USU"] != DBNull.Value) { UsuarioConexao = (String)dt.Rows[0]["COMFORNORG_USU"]; }
                if (dt.Rows[0]["COMFORNORG_SENHA"] != DBNull.Value) { SenhaConexao = pci.Decodificar((String)dt.Rows[0]["COMFORNORG_SENHA"]); }

                IdUsuarioInclusao = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                NomeUsuarioInclusao = (String)dt.Rows[0]["USU_NM_INS"];
                DataInclusao = Convert.ToDateTime(dt.Rows[0]["COMFORNORG_DH_USUARIO_INS"]);

                if (dt.Rows[0]["USU_ID_UPD"] != DBNull.Value)
                {
                    IdUsuarioAlteracao = Convert.ToInt32(dt.Rows[0]["USU_ID_UPD"]);
                    NomeUsuarioAlteracao = (String)dt.Rows[0]["USU_NM_UPD"];
                    DataAlteracao = Convert.ToDateTime(dt.Rows[0]["COMFORNORG_DH_USUARIO_UPD"]);
                }

                Status = (String)dt.Rows[0]["COMFORNORG_ST"];

                pci = null;

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "IdCanal"; }
            if (dt.Columns.Contains("COMCNL_NM")) { dt.Columns["COMCNL_NM"].ColumnName = "NomeCanal"; }
            if (dt.Columns.Contains("COMFORN_ID")) { dt.Columns["COMFORN_ID"].ColumnName = "IdFornecedora"; }
            if (dt.Columns.Contains("COMFORN_CD")) { dt.Columns["COMFORN_CD"].ColumnName = "CodigoFornecedora"; }
            if (dt.Columns.Contains("COMFORN_NM")) { dt.Columns["COMFORN_NM"].ColumnName = "Fornecedora"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_NM")) { dt.Columns["ORG_NM"].ColumnName = "NomeOrganizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("COMFORNORG_PRIORIDADE")) { dt.Columns["COMFORNORG_PRIORIDADE"].ColumnName = "Prioridade"; }
            if (dt.Columns.Contains("COMFORNORG_USU")) { dt.Columns["COMFORNORG_USU"].ColumnName = "UsuarioConexao"; }
            if (dt.Columns.Contains("COMFORNORG_SENHA")) { dt.Columns["COMFORNORG_SENHA"].ColumnName = "SenhaConexao"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            if (dt.Columns.Contains("USU_NM_INS")) { dt.Columns["USU_NM_INS"].ColumnName = "NomeUsuarioInclusao"; }
            if (dt.Columns.Contains("COMFORNORG_DH_USUARIO_INS")) { dt.Columns["COMFORNORG_DH_USUARIO_INS"].ColumnName = "DataInclusao"; }
            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "IdUsuarioAlteracao"; }
            if (dt.Columns.Contains("USU_NM_UPD")) { dt.Columns["USU_NM_UPD"].ColumnName = "NomeUsuarioAlteracao"; }
            if (dt.Columns.Contains("COMFORNORG_DH_USUARIO_UPD")) { dt.Columns["COMFORNORG_DH_USUARIO_UPD"].ColumnName = "DataAlteracao"; }
            if (dt.Columns.Contains("COMFORNORG_ST")) { dt.Columns["COMFORNORG_ST"].ColumnName = "Status"; }
        }
        #endregion Métodos
    }
}