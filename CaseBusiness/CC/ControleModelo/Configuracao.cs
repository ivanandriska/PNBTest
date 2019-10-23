#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.ControleModelo
{
    public class Configuracao : BusinessBase
    {
        #region Atributos
        private String _statusPreProcessamento = String.Empty;
        private String _statusSincronizar = String.Empty;
        private DateTime _dhInicioPreProcessamento = DateTime.MinValue;
        private Int32 _idUsuarioIns = Int32.MinValue;
        private DateTime _dhUsarioIns = DateTime.MinValue;
        private Int32 _idUsuarioUpd = Int32.MinValue;
        private DateTime _dhUsarioUpd = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public String StatusPreProcessamento
        {
            get { return _statusPreProcessamento; }
            set { _statusPreProcessamento = value; }
        }

        public String StatusSincronizar
        {
            get { return _statusSincronizar; }
            set { _statusSincronizar = value; }
        }

        public DateTime DhInicioPreProcessamento
        {
            get { return _dhInicioPreProcessamento; }
            set { _dhInicioPreProcessamento = value; }
        }

        public Int32 IdUsuarioIns
        {
            get { return _idUsuarioIns; }
            set { _idUsuarioIns = value; }
        }

        public DateTime DhUsarioIns
        {
            get { return _dhUsarioIns; }
            set { _dhUsarioIns = value; }
        }

        public Int32 IdUsuarioUpd
        {
            get { return _idUsuarioUpd; }
            set { _idUsuarioUpd = value; }
        }

        public DateTime DhUsarioUpd
        {
            get { return _dhUsarioUpd; }
            set { _dhUsarioUpd = value; }
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
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe Configuracao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public Configuracao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
            Consultar();
        }

        /// <summary>
        /// Construtor classe Configuracao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public Configuracao(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar Configuracao
        /// </summary>
        public DataTable Consultar()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }


                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMPCONF_SEL_CONSULTAR").Tables[0];

                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    StatusPreProcessamento = dt.Rows[0]["CMCONF_ST"].ToString();
                    StatusSincronizar = dt.Rows[0]["CMCONF_ST_SINCRONIZAR"].ToString();
                    DhInicioPreProcessamento = Convert.ToDateTime(dt.Rows[0]["CMCONF_DH_INICIO"]);
                    __blnIsLoaded = true;
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
        /// Inclui uma Configuracao
        /// </summary>
        /// <param name="status">Status_</param>
        /// <param name="statusSincronizacao">Status Sincronizacao</param>
        /// <param name="dhPreProcessamento">Data Hora Pre-Processamento</param>
        /// <param name="dhInclusao">Data Hora Inclusao</param>
        public void Incluir(String status,
                            String statusSincronizar,
                            DateTime dhPreProcessamento,
                            DateTime dhInclusao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMCONF_ST", status);
                acessoDadosBase.AddParameter("@CMCONF_ST_SINCRONIZAR", statusSincronizar);
                acessoDadosBase.AddParameter("@CMCONF_DH_INICIO", dhPreProcessamento);
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@CMCONF_DH_USUARIO_INS", dhInclusao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPCONF_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// Alterar uma Configuracao
        /// </summary>
        /// <param name="status">Status_</param>
        /// <param name="statusSincronizacao">Status Sincronizacao</param>
        /// <param name="dhPreProcessamento">Data Hora Pre-Processamento</param>
        /// <param name="dhInclusao">Data Hora Inclusao</param>
        public void Alterar(String status,
                            String statusSincronizar,
                            DateTime dhPreProcessamento,
                            DateTime dhInclusao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Configuracao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@CMCONF_ST", status);
                acessoDadosBase.AddParameter("@CMCONF_ST_SINCRONIZAR", statusSincronizar);
                acessoDadosBase.AddParameter("@CMCONF_DH_INICIO", dhPreProcessamento);
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@CMCONF_DH_USUARIO_UPD", dhInclusao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prCMPCONF_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        #endregion Métodos
    }
}
