#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CtrlAcesso
{
    public class Usuario : BusinessBase
    {
        #region Enums e Constantes
        public enum enumUsuarioSistemaId { EMPTY, CASE03, CASEMENSAGERIASERVICO }
        public const string kUsuarioSistema_Case03_DBValue = "-3";
        public const string kUsuarioSistema_CaseMensageriaServico_DBValue = "-51";

        public enum enumUsuarioSistemaNome { Case03, CaseMensageriaServico }
        public const string kUsuarioSistema_Case03_Texto = "Case03";
        public const string kUsuarioSistema_CaseMensageriaServico_Texto = "CaseMensageriaServico";
        #endregion Enums e Constantes

        #region Atributos
        private Int32 _idUsuario = Int32.MinValue;
        private String _nome = String.Empty;
        private String _cpf = String.Empty;
        private UsuarioStatus.enumStatus _status = UsuarioStatus.enumStatus.EMPTY;
        private Boolean _flagSolicitaNovaSenha = false;
        private Boolean _flagTemplatePermissao = false;
        private String _codigoLoginAD = String.Empty;
        private String _codigoLogin = String.Empty;
        private DateTime _dtLoginUltimo = DateTime.MinValue;
        private DateTime _dtSenha = DateTime.MinValue;
        private String _email = String.Empty;
        private String _codigoIdentificacaoFuncionario = String.Empty;
        private Int32 _idMarcadorPermissao = Int32.MinValue;
        private Boolean _flagLogin = false;
        //private DataTable _dtRestricoesExclusao;
        #endregion Atributos

        #region Propriedades
        public Int32 IdUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }

        public String Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public String CPF
        {
            get { return _cpf; }
            set { _cpf = value; }
        }

        public UsuarioStatus.enumStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Boolean FlagSolicitaNovaSenha
        {
            get { return _flagSolicitaNovaSenha; }
            set { _flagSolicitaNovaSenha = value; }
        }

        public Boolean FlagTemplatePermissao
        {
            get { return _flagTemplatePermissao; }
            set { _flagTemplatePermissao = value; }
        }

        public String CodigoLoginAD
        {
            get { return _codigoLoginAD; }
            set { _codigoLoginAD = value; }
        }

        public String CodigoLogin
        {
            get { return _codigoLogin; }
            set { _codigoLogin = value; }
        }

        public DateTime DataSenha
        {
            get { return _dtSenha; }
            set { _dtSenha = value; }
        }

        public Int32 QtdeDias_DataSenha
        {
            get
            {
                TimeSpan tsDiasSenha;

                tsDiasSenha = DateTime.Now - DataSenha;

                return tsDiasSenha.Days;
            }
        }

        public DateTime DataLoginUltimo
        {
            get { return _dtLoginUltimo; }
            set { _dtLoginUltimo = value; }
        }

        public String EMail
        {
            get { return _email; }
            set { _email = value; }
        }

        public String CodigoIdentificacaoFuncionario
        {
            get { return _codigoIdentificacaoFuncionario; }
            set { _codigoIdentificacaoFuncionario = value; }
        }

        public Int32 IdMarcadorPermissao
        {
            get { return _idMarcadorPermissao; }
            set { _idMarcadorPermissao = value; }
        }

        public Boolean FlagLogin
        {
            get { return _flagLogin; }
            set { _flagLogin = value; }
        }

        public Boolean LoginBloqueadoporInatividade
        {
            get
            {
                if (IsLoaded)
                {
                    UsuarioLoginPolitica objLogPolitic = new UsuarioLoginPolitica(UsuarioManutencao.ID);
                    TimeSpan tsDiasLoginUltimo;

                    if (!objLogPolitic.IsLoaded) { throw new Exception("Não foi possível obter a atual parametrização da Politica de Login"); }

                    // Se zerado o parâmetro, significa que Bloqueio está OFF
                    if (objLogPolitic.QtdeDiasBloqueioLoginInativo.Equals(0))
                    {
                        objLogPolitic = null;
                        return false;
                    }

                    tsDiasLoginUltimo = DateTime.Now - DataLoginUltimo;

                    // Days = Whole Days
                    // TotalDays = Delta T
                    if (tsDiasLoginUltimo.Days >= objLogPolitic.QtdeDiasBloqueioLoginInativo)
                    {
                        objLogPolitic = null;
                        return true;
                    }
                    else
                    {
                        objLogPolitic = null;
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        //public Boolean ExclusaoPermitida
        //{
        //    get
        //    {
        //        if (!IsLoaded)
        //        {
        //            _dtRestricoesExclusao = null;
        //            return false;
        //        }

        //        if (RestricoesExclusao == null)
        //        { return false; }
        //        else
        //        {
        //            if (_dtRestricoesExclusao.Rows.Count <= 0)
        //            { return true; }
        //            else
        //            { return false; }
        //        }
        //    }
        //}

        //public DataTable RestricoesExclusao
        //{
        //    get
        //    {
        //        if (_dtRestricoesExclusao == null)
        //        {
        //            _dtRestricoesExclusao = new DataTable();
        //            _dtRestricoesExclusao = JACA_ObterRestricoesExclusao();
        //        }

        //        return _dtRestricoesExclusao;
        //    }
        //}
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe Usuario - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public Usuario(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Usuario
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public Usuario()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Usuario
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Usuario(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe Usuario utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Usuario(Int32 idUsuarioManutencao,
                       CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }


        /// <summary>
        /// Construtor classe Usuario e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Usuario(Int32 idUsuario,
                       Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idUsuario);
        }


        /// <summary>
        /// Construtor classe Usuario utilizando uma Transação e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Usuario(Int32 idUsuario,
                       Int32 idUsuarioManutencao,
                       CaseBusiness.Framework.BancoDados.Transacao transacao)
            : this(idUsuarioManutencao, transacao)
        {
            Consultar(idUsuario);
        }
        #endregion Construtores

        #region Métodos

        public static String ObterDBValue(enumUsuarioSistemaNome enmUsuarioSistema)
        {
            String _dbvalue = String.Empty;

            switch (enmUsuarioSistema)
            {
                case enumUsuarioSistemaNome.Case03: _dbvalue = kUsuarioSistema_Case03_DBValue; break;
                case enumUsuarioSistemaNome.CaseMensageriaServico: _dbvalue = kUsuarioSistema_CaseMensageriaServico_DBValue; break;
            }

            return _dbvalue;
        }

        public static enumUsuarioSistemaId ObterEnum(String status)
        {
            enumUsuarioSistemaId _enmstatus = enumUsuarioSistemaId.EMPTY;

            switch (status.Trim().ToUpper().Substring(0, 1))
            {
                case kUsuarioSistema_Case03_DBValue: _enmstatus = enumUsuarioSistemaId.CASE03; break;
                case kUsuarioSistema_CaseMensageriaServico_DBValue: _enmstatus = enumUsuarioSistemaId.CASEMENSAGERIASERVICO; break;
            }

            return _enmstatus;
        }

        /// <summary>
        /// Inclui um Usuário Normal
        /// </summary>
        /// <param name="nome">Nome do Usuário</param>
        /// <param name="status">Status do Usuário</param>
        /// <param name="codigoLoginAD">Login AD  do Usuário</param>
        /// <param name="codigoLogin">Login do Usuário</param>
        /// <param name="senha_provisoria_decrypt">Senha Provisória (Não Criptograda)</param>
        /// <param name="email">eMail do Usuário</param>
        /// <param name="codigoIdentificacaoFuncionario">Código de Identificação</param>
        /// <param name="idMarcadorPermissao">Id Marcador Permissão</param>
        public void IncluirUsuarioNormal(String nome,
                                         String cpf,
                                         UsuarioStatus.enumStatus status,
                                         String codigoLoginAD,
                                         String codigoLogin,
                                         String senha_provisoria_decrypt,
                                         String email,
                                         String codigoIdentificacaoFuncionario,
                                         Int32 idMarcadorPermissao)
        {
            Incluir(nome,
                    cpf,
                    false,
                    status,
                    codigoLoginAD,
                    codigoLogin,
                    senha_provisoria_decrypt,
                    email,
                    codigoIdentificacaoFuncionario,
                    idMarcadorPermissao);
        }

        /// <summary>
        /// Inclui um Usuário Template de Permissão de Acesso em Lote
        /// </summary>
        /// <param name="nome">Nome do Usuário</param>
        public void IncluirUsuarioTemplate(String nome)
        {
            Incluir(nome,
                    String.Empty,
                    true,
                    UsuarioStatus.enumStatus.INATIVO,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    Int32.MinValue);
        }


        /// <summary>
        /// Inclui um Usuário
        /// </summary>
        /// <param name="nome">Nome do Usuário</param>
        /// <param name="status">Status do Usuário</param>
        /// <param name="flagTemplatePermissao">Flag Usuário Template Permissão</param>
        /// <param name="codigoLoginAD">Login AD  do Usuário</param>
        /// <param name="codigoLogin">Login do Usuário</param>
        /// <param name="senha_provisoria_decrypt">Senha Provisória (Não Criptograda)</param>
        /// <param name="email">eMail do Usuário</param>
        /// <param name="codigoIdentificacaoFuncionario">Código de Identificação</param>
        /// <param name="idMarcadorPermissao">Id Marcador Permissão</param>
        private void Incluir(String nome,
                             String cpf,
                             Boolean flagTemplatePermissao,
                             UsuarioStatus.enumStatus status,
                             String codigoLoginAD,
                             String codigoLogin,
                             String senha_provisoria_decrypt,
                             String email,
                             String codigoIdentificacaoFuncionario,
                             Int32 idMarcadorPermissao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                String _flagSolicNovaSenha = String.Empty;
                String _flagTemplatePermissao = String.Empty;
                DateTime _dtSenha = DateTime.MinValue;
                DateTime _dtLoginUltimo = DateTime.MinValue;


                if (flagTemplatePermissao)
                { _flagTemplatePermissao = "S"; }
                else
                { _flagTemplatePermissao = "N"; }


                if (flagTemplatePermissao)
                {
                    //
                    //  Usuario Template de Permissão
                    //
                    _flagSolicNovaSenha = "N";
                }
                else
                {
                    //
                    //  Usuario Normal
                    //
                    _dtSenha = DateTime.Now;
                    _dtLoginUltimo = DateTime.Now;  // É necessário senão o Novo Usuario já "nasce bloqueado por Inatividade

                    CaseBusiness.Framework.Criptografia.Cript pci = new CaseBusiness.Framework.Criptografia.Cript(PCI_ConfigPath, "");
                    senha_provisoria_decrypt = pci.Codificar(senha_provisoria_decrypt);
                    pci = null;

                    _flagSolicNovaSenha = "S";
                }

                acessoDadosBase.AddParameter("@USU_NM", nome.Trim());
                acessoDadosBase.AddParameter("@USU_NR_CPF", cpf.Trim());
                acessoDadosBase.AddParameter("@USU_ST", UsuarioStatus.ObterDBValue(status));
                acessoDadosBase.AddParameter("@USU_FG_SOLIC_NOVASENHA", _flagSolicNovaSenha);
                acessoDadosBase.AddParameter("@USU_FG_TEMPLATE_PERMISSAO", _flagTemplatePermissao);
                acessoDadosBase.AddParameter("@USU_CD_LOGIN_AD", codigoLoginAD.Trim());
                acessoDadosBase.AddParameter("@USU_CD_LOGIN", codigoLogin.Trim());
                acessoDadosBase.AddParameter("@USU_DT_LOGIN_ULTIMO", _dtLoginUltimo);
                acessoDadosBase.AddParameter("@USU_CD_SENHA_CRYPT", senha_provisoria_decrypt);
                acessoDadosBase.AddParameter("@USU_DT_SENHA", _dtSenha);
                acessoDadosBase.AddParameter("@USU_DS_EMAIL", email.Trim());
                acessoDadosBase.AddParameter("@USU_CD_IDENT_FUNC", codigoIdentificacaoFuncionario.Trim());
                acessoDadosBase.AddParameter("@MARCPERM_ID", idMarcadorPermissao);
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prUSU_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Exclui um Usuário
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        //public void JACA_Excluir(Int32 idUsuario)
        //{
        //    try
        //    {
        //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

        //        //acessoDadosBase.AddParameter("@USU_ID", idUsuario);

        //        //acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prUSU_DEL");
        //    }
        //    catch (Exception ex)
        //    {
        //        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
        //        throw;
        //    }
        //}



        /// <summary>
        /// Altera um Usuário Normal
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="nome">Nome do Usuário</param>
        /// <param name="status">Status do Usuário</param>
        /// <param name="codigoLoginAD">Login AD  do Usuário</param>
        /// <param name="codigoLogin">Login do Usuário</param>
        /// <param name="email">eMail do Usuário</param>
        /// <param name="codigoIdentificacaoFuncionario">Código de Identificação</param>
        /// <param name="idMarcadorPermissao">Id Marcador Permissão</param>
        public void AlterarUsuarioNormal(Int32 idUsuario,
                                         String nome,
                                         String cpf,
                                         UsuarioStatus.enumStatus status,
                                         String codigoLoginAD,
                                         String codigoLogin,
                                         String email,
                                         String codigoIdentificacaoFuncionario,
                                         Int32 idMarcadorPermissao)
        {
            Alterar(idUsuario,
                    nome,
                    cpf,
                    status,
                    codigoLoginAD,
                    codigoLogin,
                    email,
                    codigoIdentificacaoFuncionario,
                    idMarcadorPermissao);
        }

        /// <summary>
        /// Altera um Usuário Template
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="nome">Nome do Usuário</param>
        public void AlterarUsuarioTemplate(Int32 idUsuario,
                                           String nome)
        {
            Alterar(idUsuario,
                    nome,
                    String.Empty,
                    UsuarioStatus.enumStatus.INATIVO,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    Int32.MinValue);
        }


        /// <summary>
        /// Altera um Usuário
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="nome">Nome do Usuário</param>
        /// <param name="status">Status do Usuário</param>
        /// <param name="codigoLoginAD">Login AD  do Usuário</param>
        /// <param name="codigoLogin">Login do Usuário</param>
        /// <param name="email">eMail do Usuário</param>
        /// <param name="codigoIdentificacaoFuncionario">Código de Identificação</param>
        /// <param name="idMarcadorPermissao">Id Marcador Permissão</param>
        private void Alterar(Int32 idUsuario,
                             String nome,
                             String cpf,
                             UsuarioStatus.enumStatus status,
                             String codigoLoginAD,
                             String codigoLogin,
                             String email,
                             String codigoIdentificacaoFuncionario,
                             Int32 idMarcadorPermissao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@USU_NM", nome.Trim());
                acessoDadosBase.AddParameter("@USU_NR_CPF", cpf.Trim());
                acessoDadosBase.AddParameter("@USU_ST", UsuarioStatus.ObterDBValue(status));
                acessoDadosBase.AddParameter("@USU_CD_LOGIN_AD", codigoLoginAD.Trim());
                acessoDadosBase.AddParameter("@USU_CD_LOGIN", codigoLogin.Trim());
                acessoDadosBase.AddParameter("@USU_DS_EMAIL", email.Trim());
                acessoDadosBase.AddParameter("@USU_CD_IDENT_FUNC", codigoIdentificacaoFuncionario.Trim());
                acessoDadosBase.AddParameter("@MARCPERM_ID", idMarcadorPermissao);
                acessoDadosBase.AddParameter("@USU_ID_UPD", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prUSU_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Buscar Usuários
        /// </summary>
        /// <param name="nome">Nome do Usuário</param>
        /// <param name="status">Status do Usuário</param>
        /// <param name="flagTemplatePermissao">Flag Usuário Template Permissão</param>
        /// <param name="codigoLoginAD">Login AD  do Usuário</param>
        /// <param name="codigoLogin">Login do Usuário</param>
        /// <param name="email">eMail do Usuário</param>
        /// <param name="codigoIdentificacaoFuncionario">Código de Identificação</param>
        /// <param name="idMarcadorPermissao">Id Marcador Permissão</param>
        public DataTable Buscar(String nome,
                                String cpf,
                                UsuarioStatus.enumStatus status,
                                Boolean? flagTemplatePermissao,
                                String codigoLoginAD,
                                String codigoLogin,
                                String codigoIdentificacaoFuncionario,
                                Int32 idMarcadorPermissao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_NM", nome.Trim());
                acessoDadosBase.AddParameter("@USU_NR_CPF", cpf.Trim());
                acessoDadosBase.AddParameter("@USU_ST", UsuarioStatus.ObterDBValue(status));
                if (flagTemplatePermissao.HasValue)
                {
                    acessoDadosBase.AddParameter("@USU_FG_TEMPLATE_PERMISSAO", flagTemplatePermissao == true ? "S" : "N");
                }
                acessoDadosBase.AddParameter("@USU_CD_LOGIN_AD", codigoLoginAD.Trim());
                acessoDadosBase.AddParameter("@USU_CD_LOGIN", codigoLogin.Trim());
                acessoDadosBase.AddParameter("@USU_CD_IDENT_FUNC", codigoIdentificacaoFuncionario.Trim());
                acessoDadosBase.AddParameter("@MARCPERM_ID", idMarcadorPermissao);

                // Habilita Usuarios ID Negativos 2RP.Net se o Logado for um deles
                if (this.UsuarioManutencao.ID < 0)
                {
                    acessoDadosBase.AddParameter("@FLAG_MOSTRA_USU_ID_NEGATIVO", "S");
                }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                            "prUSU_SEL_BUSCAR").Tables[0];

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
        /// Buscar Usuários
        /// </summary>
        /// <param name="nome">Nome do Usuário</param>
        /// <param name="status">Status do Usuário</param>
        /// <param name="flagTemplatePermissao">Flag Usuário Template Permissão</param>
        /// <param name="codigoLoginAD">Login AD  do Usuário</param>
        /// <param name="codigoLogin">Login do Usuário</param>
        /// <param name="email">eMail do Usuário</param>
        /// <param name="codigoIdentificacaoFuncionario">Código de Identificação</param>
        /// <param name="idMarcadorPermissao">Id Marcador Permissão</param>
        public DataTable BuscarComUsuariosSistema(String nome,
                                                  UsuarioStatus.enumStatus status,
                                                  Boolean? flagTemplatePermissao,
                                                  String codigoLoginAD,
                                                  String codigoLogin,
                                                  String codigoIdentificacaoFuncionario,
                                                  Int32 idMarcadorPermissao,
                                                  List<enumUsuarioSistemaNome> usuariosSistema)
        {
            DataTable dt = null;
            String listaUsuarios = "";
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                for (int i = 0; i<usuariosSistema.Count; i++)
                {
                    listaUsuarios += ObterDBValue(usuariosSistema[i]) + ",";
                }

                acessoDadosBase.AddParameter("@USU_NM", nome.Trim());
                acessoDadosBase.AddParameter("@USU_ST", UsuarioStatus.ObterDBValue(status));
                if (flagTemplatePermissao.HasValue)
                {
                    acessoDadosBase.AddParameter("@USU_FG_TEMPLATE_PERMISSAO", flagTemplatePermissao == true ? "S" : "N");
                }
                acessoDadosBase.AddParameter("@USU_CD_LOGIN_AD", codigoLoginAD.Trim());
                acessoDadosBase.AddParameter("@USU_CD_LOGIN", codigoLogin.Trim());
                acessoDadosBase.AddParameter("@USU_CD_IDENT_FUNC", codigoIdentificacaoFuncionario.Trim());
                acessoDadosBase.AddParameter("@MARCPERM_ID", idMarcadorPermissao);
                acessoDadosBase.AddParameter("@USUARIOSSISTEMA", listaUsuarios);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                            "prUSU_SEL_BUSCAR_ADD_USUARIOS").Tables[0];

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
        /// Consultar um Usuário por ID
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        private void Consultar(Int32 idUsuario)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSU_SEL_CONSULTARID").Tables[0];

                // Fill Object
                PreencherAtrubutos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Consultar um Usuário por Login
        /// </summary>
        /// <param name="login">Login do Usuário</param>
        public void Consultar(String login)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_CD_LOGIN", login.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSU_SEL_CONSULTARLOGIN").Tables[0];

                // Fill Object
                PreencherAtrubutos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consultar um Usuário por Login do AD
        /// </summary>
        /// <param name="login">Login do Usuário</param>
        public void ConsultarAD(String login)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_CD_LOGIN_AD", login.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSU_SEL_CONSULTARLOGINAD").Tables[0];

                // Fill Object
                PreencherAtrubutos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Consultar um Usuário por Login
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="login">Login do Usuário</param>
        /// <param name="loginAD">LoginAD do Usuário</param>
        public void Consultar(Int32 idUsuario, String login, String loginAD)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@USU_CD_LOGIN", login.Trim());
                acessoDadosBase.AddParameter("@USU_CD_LOGIN_AD", loginAD.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSU_SEL_CONSULTARLOGINS").Tables[0];

                // Fill Object
                PreencherAtrubutos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consultar um Usuário por Login
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="login">Login do Usuário</param>
        /// <param name="loginAD">LoginAD do Usuário</param>
        public void ConsultarUsuarioIdentificacao(Int32 idUsuario, String identificacaoFuncionario)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@USU_CD_IDENT_FUNC", identificacaoFuncionario.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSU_SEL_CONSULTARIDENT").Tables[0];

                // Fill Object
                PreencherAtrubutos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Consultar um Usuário por CPF
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="login">Login do Usuário</param>
        /// <param name="loginAD">LoginAD do Usuário</param>
        public void ConsultarUsuarioCpf(Int64 idUsuario,
                                        String cpf)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@USU_NR_CPF", cpf);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSU_SEL_CONSULTAR_CPF").Tables[0];

                // Fill Object
                PreencherAtrubutos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Marca o Flag de Solicitação de Nova Senha
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="senha_provisoria_decrypt">Senha Provisória (Não Criptograda)</param>
        public void SolicitarNovaSenha(Int32 idUsuario,
                                       String senha_provisoria_decrypt)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new CaseBusiness.Framework.Criptografia.Cript(PCI_ConfigPath, "");
                senha_provisoria_decrypt = pci.Codificar(senha_provisoria_decrypt);
                pci = null;

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@USU_CD_SENHA_CRYPT", senha_provisoria_decrypt.Trim());
                acessoDadosBase.AddParameter("@USU_ID_UPD", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prUSU_UPD_SOLICNOVASENHA");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Cria uma nova senha a um Usuário
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="nova_senha_decrypt">Nova Senha (não criptografada)</param>
        /// <returns>'False' siginifca que a Senha já foi utilizada no passado dentro do período parametrizado na Política de Login</returns>
        public Boolean CriarNovaSenha(Int32 idUsuario,
                                      String nova_senha_decrypt)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                Int16 _senha_ja_utilizada = Int16.MinValue;

                CaseBusiness.Framework.Criptografia.Cript pci = new CaseBusiness.Framework.Criptografia.Cript(PCI_ConfigPath, "");
                nova_senha_decrypt = pci.Codificar(nova_senha_decrypt);
                pci = null;

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@USU_CD_SENHA_CRYPT", nova_senha_decrypt.Trim());
                acessoDadosBase.AddParameter("@SENHA_JA_UTILIZADA", _senha_ja_utilizada, ParameterDirection.InputOutput);

                _senha_ja_utilizada = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure,
                                                        "prUSU_UPD_SENHACRYPT")[0]);

                if (_senha_ja_utilizada > 0)
                {
                    // Senha Já foi utilizada
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Validar um Usuário por Login e Senha
        /// </summary>
        /// <param name="login">Login do Usuário</param>
        /// <param name="senha">Senha do Usuário</param>
        public Boolean Autenticar(String login,
                                  String senha)
        {
            try
            {
                DataTable dt = null;
                Boolean _autenticado = false;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                CaseBusiness.Framework.Criptografia.Cript pci = new CaseBusiness.Framework.Criptografia.Cript(PCI_ConfigPath, "");
                senha = pci.Codificar(senha);
                pci = null;

                acessoDadosBase.AddParameter("@USU_CD_LOGIN", login);
                acessoDadosBase.AddParameter("@USU_CD_SENHA_CRYPT", senha);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSU_SEL_AUTENTICARLOGIN").Tables[0];

                if (dt.Rows.Count > 0)
                {
                    _autenticado = true;
                }
                return _autenticado;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Desbloqueia um Usuário com Login com Bloqueio por Inatividade Tempo
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        public void DesbloquearLoginInativo(Int32 idUsuario)
        {
            AlterarDataLoginUltimo(idUsuario, DateTime.MaxValue);
        }


        /// <summary>
        /// Bloqueia um Usuário por Qtde de Senhas Erradas em Sequencia
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        public void BloquearLoginporQtdeSenhasErradas(Int32 idUsuario)
        {
            AlterarDataLoginUltimo(idUsuario, new DateTime(1811, 11, 11, 11, 11, 11));
        }


        /// <summary>
        /// Registra o momento do Login do Usuário
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        public void RegistrarDataLogin(Int32 idUsuario)
        {
            AlterarDataLoginUltimo(idUsuario, DateTime.Now);
        }


        /// <summary>
        /// Altera a Data do Login no Usuário Informado
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="dtLoginUltimo">Data em que o Usuário fez um Login</param>
        private void AlterarDataLoginUltimo(Int32 idUsuario,
                                            DateTime dtLoginUltimo)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@USU_DT_LOGIN_ULTIMO", dtLoginUltimo);
                acessoDadosBase.AddParameter("@USU_ID_UPD", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prUSU_UPD_DTLOGINULTIMO");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


        /// <summary>
        /// Obtem as Restrições de Exclusão do Grupo carregado
        /// </summary>
        //private DataTable JACA_ObterRestricoesExclusao()
        //{
        //    DataTable dt = null;

        //    try
        //    {
        //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Usuário está operando em Modo Entidade Only"); }

        //        //acessoDadosBase.AddParameter("@USU_ID", IdUsuario);

        //        //dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
        //        //                                    "prUSU_SEL_RESTRIC_DEL").Tables[0];

        //        // Renomear Colunas
        //        RenomearColunas(ref dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
        //        throw;
        //    }

        //    return dt;
        //}


        /// <summary>
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtrubutos(ref DataTable dt)
        {

            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                IdUsuario = Convert.ToInt32(dt.Rows[0]["USU_ID"]);
                Nome = dt.Rows[0]["USU_NM"].ToString();
                CPF = dt.Rows[0]["USU_NR_CPF"].ToString();
                Status = UsuarioStatus.ObterEnum(dt.Rows[0]["USU_ST"].ToString());
                FlagSolicitaNovaSenha = dt.Rows[0]["USU_FG_SOLIC_NOVASENHA"].ToString().Equals("S") ? true : false;
                FlagTemplatePermissao = dt.Rows[0]["USU_FG_TEMPLATE_PERMISSAO"].ToString().Equals("S") ? true : false;
                if (dt.Columns.Contains("USU_FG_LOGIN"))
                {
                    FlagLogin = dt.Rows[0]["USU_FG_LOGIN"].ToString().Equals("S") ? true : false;
                }

                if (dt.Rows[0]["USU_CD_LOGIN_AD"] != DBNull.Value)
                {
                    CodigoLoginAD = dt.Rows[0]["USU_CD_LOGIN_AD"].ToString();
                }

                if (dt.Rows[0]["USU_CD_LOGIN"] != DBNull.Value)
                {
                    CodigoLogin = dt.Rows[0]["USU_CD_LOGIN"].ToString();
                }

                if (dt.Rows[0]["USU_DT_LOGIN_ULTIMO"] != DBNull.Value)
                {
                    DataLoginUltimo = Convert.ToDateTime(dt.Rows[0]["USU_DT_LOGIN_ULTIMO"]);
                }

                if (dt.Rows[0]["USU_DT_SENHA"] != DBNull.Value)
                {
                    DataSenha = Convert.ToDateTime(dt.Rows[0]["USU_DT_SENHA"]);
                }

                if (dt.Rows[0]["USU_DS_EMAIL"] != DBNull.Value)
                {
                    EMail = dt.Rows[0]["USU_DS_EMAIL"].ToString();
                }

                if (dt.Rows[0]["USU_CD_IDENT_FUNC"] != DBNull.Value)
                {
                    CodigoIdentificacaoFuncionario = dt.Rows[0]["USU_CD_IDENT_FUNC"].ToString();
                }

                if (dt.Rows[0]["MARCPERM_ID"] != DBNull.Value)
                {
                    IdMarcadorPermissao = Convert.ToInt32(dt.Rows[0]["MARCPERM_ID"]);
                }

                __blnIsLoaded = true;
            }
        }


        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("USU_ID")) { dt.Columns["USU_ID"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioNome"; }
            if (dt.Columns.Contains("USU_NR_CPF")) { dt.Columns["USU_NR_CPF"].ColumnName = "nrCpf"; }
            if (dt.Columns.Contains("USU_ST")) { dt.Columns["USU_ST"].ColumnName = "UsuarioStatus"; }
            if (dt.Columns.Contains("USU_ST_TEXTO")) { dt.Columns["USU_ST_TEXTO"].ColumnName = "UsuarioStatusTexto"; }
            if (dt.Columns.Contains("USU_FG_TEMPLATE_PERMISSAO")) { dt.Columns["USU_FG_TEMPLATE_PERMISSAO"].ColumnName = "FlagTemplatePermissao"; }
            if (dt.Columns.Contains("USU_CD_LOGIN_AD")) { dt.Columns["USU_CD_LOGIN_AD"].ColumnName = "LoginAD"; }
            if (dt.Columns.Contains("USU_CD_LOGIN")) { dt.Columns["USU_CD_LOGIN"].ColumnName = "Login"; }
            if (dt.Columns.Contains("USU_DT_LOGIN_ULTIMO")) { dt.Columns["USU_DT_LOGIN_ULTIMO"].ColumnName = "DataLoginUltimo"; }
            if (dt.Columns.Contains("USU_DS_EMAIL")) { dt.Columns["USU_DS_EMAIL"].ColumnName = "EMail"; }
            if (dt.Columns.Contains("USU_CD_IDENT_FUNC")) { dt.Columns["USU_CD_IDENT_FUNC"].ColumnName = "CodigoIdentificacaoFuncionario"; }
            if (dt.Columns.Contains("MARCPERM_ID")) { dt.Columns["MARCPERM_ID"].ColumnName = "IdMarcadorPermissao"; }
            if (dt.Columns.Contains("MARCPERM_DS")) { dt.Columns["MARCPERM_DS"].ColumnName = "MarcadorPermissaoDescricao"; }
            if (dt.Columns.Contains("USU_FG_LOGIN")) { dt.Columns["USU_FG_LOGIN"].ColumnName = "FlagLogin"; }

            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "IdUsuarioManutencao"; }

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }
        #endregion Métodos
    }
}
