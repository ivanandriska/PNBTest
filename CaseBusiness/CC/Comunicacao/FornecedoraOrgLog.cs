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
    public class FornecedoraOrgLog : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Comunicacao_FornecedoraOrgLog_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
         #endregion Atributos

        #region Propriedades
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe FornecedoraOrgLog - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public FornecedoraOrgLog(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará FornecedoraOrgLog com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe FornecedoraOrgLog
        /// </summary>
        /// <remarks>Este Construtor "vazio" foi criado por conta da página de Login, método Validar onde ainda não Usuário logado</remarks>
        public FornecedoraOrgLog()
        {
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe FornecedoraOrgLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public FornecedoraOrgLog(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe FornecedoraOrgLog utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public FornecedoraOrgLog(Int32 idUsuarioManutencao, CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Inclui uma FornecedoraOrgLog
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="usuarioConexao">Usuário de Conexão</param>
        /// <param name="senhaConexao">Senha de Conexão</param>
        /// <param name="dataInclusao">Data de Inclusão</param>
        /// <param name="status">Status_</param>
        /// <param name="operacao">Operação</param>
        public void IncluirFornecedoraOrg(Int32 idFornecedora, 
                                          Int32 codigoOrganizacao, 
                                          Int32 prioridade, 
                                          String usuarioConexao, 
                                          String senhaConexao, 
                                          DateTime dataInclusao, 
                                          String status, 
                                          String operacao)
        {
            Incluir(idFornecedora, codigoOrganizacao, prioridade, usuarioConexao, senhaConexao, dataInclusao, status, operacao);
        }

        /// <summary>
        /// Inclui uma FornecedoraOrgLog
        /// </summary>
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="prioridade">Prioridade</param>
        /// <param name="usuarioConexao">Usuário de Conexão</param>
        /// <param name="senhaConexao">Senha de Conexão</param>
        /// <param name="dataInclusao">Data de Inclusão</param>
        /// <param name="status">Status_</param>
        /// <param name="operacao">Operação</param>
        private void Incluir(Int32 idFornecedora,
                             Int32 codigoOrganizacao,
                             Int32 prioridade,
                             String usuarioConexao,
                             String senhaConexao,
                             DateTime dataInclusao,
                             String status,
                             String operacao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Log de Conexão está operando em Modo Entidade Only"); }

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
                acessoDadosBase.AddParameter("@COMFORNORGLOG_PRIORIDADE", prioridade);
                acessoDadosBase.AddParameter("@COMFORNORGLOG_USU", usuarioConexao);
                acessoDadosBase.AddParameter("@COMFORNORGLOG_SENHA", senhaConexao);
                acessoDadosBase.AddParameter("@USU_ID_INS", UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@COMFORNORGLOG_DH_USUARIO_INS", dataInclusao);
                acessoDadosBase.AddParameter("@COMFORNORGLOG_ST", status);
                acessoDadosBase.AddParameter("@COMFORNORGLOG_OPERACAO", operacao);

                acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prCOMFORNORGLOG_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Buscar Log de 'ORG / Fornecedora / Conexão' 
        /// </summary>        
        /// <param name="idFornecedora">Id da Fornecedora</param>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        public DataTable Buscar(Int32 idFornecedora,
                                Int32 codigoOrganizacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe FornecedoraOrgLog está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@COMFORN_ID", idFornecedora);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prCOMFORNORGLOG_SEL_BUSCAR").Tables[0];

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
        /// Remove do Cache as Moedas
        /// </summary>
        public void RemoverCache()
        {
            MemoryCache.Default.Remove(kCacheKey);
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("COMCNL_ID")) { dt.Columns["COMCNL_ID"].ColumnName = "IdCanal"; }
            if (dt.Columns.Contains("COMCNL_NM")) { dt.Columns["COMCNL_NM"].ColumnName = "NomeCanal"; }
            if (dt.Columns.Contains("COMFORN_ID")) { dt.Columns["COMFORN_ID"].ColumnName = "IdFornecedora"; }
            if (dt.Columns.Contains("COMFORN_NM")) { dt.Columns["COMFORN_NM"].ColumnName = "Fornecedora"; }
            if (dt.Columns.Contains("COMFORNORGLOG_PRIORIDADE")) { dt.Columns["COMFORNORGLOG_PRIORIDADE"].ColumnName = "Prioridade"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "OrganizacaoCodigoNome"; }
            if (dt.Columns.Contains("COMFORNORGLOG_ST")) { dt.Columns["COMFORNORGLOG_ST"].ColumnName = "Status"; }
            if (dt.Columns.Contains("COMFORNORGLOG_USU")) { dt.Columns["COMFORNORGLOG_USU"].ColumnName = "UsuarioConexao"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            if (dt.Columns.Contains("USU_NM_INS")) { dt.Columns["USU_NM_INS"].ColumnName = "NomeUsuarioInclusao"; }
            if (dt.Columns.Contains("COMFORNORGLOG_DH_USUARIO_INS")) { dt.Columns["COMFORNORGLOG_DH_USUARIO_INS"].ColumnName = "DataInclusao"; }
            if (dt.Columns.Contains("COMFORNORGLOG_OPERACAO")) { dt.Columns["COMFORNORGLOG_OPERACAO"].ColumnName = "Operacao"; }
        }
        #endregion Métodos
    }
}