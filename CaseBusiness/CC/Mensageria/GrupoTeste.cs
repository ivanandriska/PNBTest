#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Mensageria
{
    public class GrupoTeste : BusinessBase
    {
        #region Atributos
        private Int32 _idGrupoTeste = Int32.MinValue;
        private Int32 _codigoOrganizacao = Int32.MinValue;
        private String _nomeGrupoTeste = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private DateTime _dataInclusao = DateTime.MinValue;
        private DataTable _dtRestricoesExclusao = null;
        #endregion Atributos

        #region Propriedades
        public Int32 IdGrupoTeste
        {
            get { return _idGrupoTeste; }
            set { _idGrupoTeste = value; }
        }

        public Int32 CodigoOrganizacao
        {
            get { return _codigoOrganizacao; }
            set { _codigoOrganizacao = value; }
        }

        public String NomeGrupoTeste
        {
            get { return _nomeGrupoTeste; }
            set { _nomeGrupoTeste = value; }
        }

        public Int32 IdUsuarioInclusao
        {
            get { return _idUsuarioInclusao; }
            set { _idUsuarioInclusao = value; }
        }

        public DateTime DataInclusao
        {
            get { return _dataInclusao; }
            set { _dataInclusao = value; }
        }

        public Boolean ExclusaoPermitida
        {
            get
            {
                if (!IsLoaded)
                {
                    _dtRestricoesExclusao = null;
                    return false;
                }

                if (RestricoesExclusao == null)
                { 
                    return false; 
                }
                else
                {
                    if (_dtRestricoesExclusao.Rows.Count <= 0)
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

        public DataTable RestricoesExclusao
        {
            get
            {
                if (_dtRestricoesExclusao == null)
                {
                    _dtRestricoesExclusao = new DataTable();
                    _dtRestricoesExclusao = ObterRestricoesExclusao();
                }

                return _dtRestricoesExclusao;
            }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe GrupoTeste - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public GrupoTeste(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe GrupoTeste
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public GrupoTeste(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe GrupoTeste utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public GrupoTeste(Int32 idUsuarioManutencao,
                              CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe GrupoTeste e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="idGrupoTeste">Id do Grupo de Teste</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public GrupoTeste(Int32 idGrupoTeste,
                         Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idGrupoTeste);
        }
        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar Grupo de Teste
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="nomeGrupoTeste">Nome do Grupo de Teste</param>
        public DataTable Buscar(Int32 codigoOrganizacao,
                                String nomeGrupoTeste)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTeste está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGGRPTST_NM", nomeGrupoTeste.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPTST_SEL_BUSCAR").Tables[0];

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
        /// Busca a Grupo de Teste por Org
        /// </summary>
        /// <param name="codigoOrganizacao"></param>
        /// <returns></returns>
        public DataTable BuscarPorOrg(Int32 codigoOrganizacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTeste está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMGGRPTST_SEL_BUSCAR_POR_ORG").Tables[0];

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
        /// Consultar um Grupo de Teste
        /// </summary>
        /// <param name="idGrupoTeste">ID do Grupo de Teste</param>
        private void Consultar(Int32 idGrupoTeste)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTeste está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPTST_ID", idGrupoTeste);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPTST_SEL_CONSULTARID").Tables[0];

                //// Fill Object
                __blnIsLoaded = false;

                // Fill Object
                PreencherAtributos(ref dt);
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <sumary>
        /// Excluir Grupo de Teste
        /// </sumary>
        /// <param name="idGrupoTeste">ID do Grupo de Teste</param>
        /// <returns>Confirmação de Exclusão</returns>
        public void ExcluirGrupoTeste(Int32 idGrupoTeste)
        {
            Excluir(idGrupoTeste);
        }

        /// <sumary>
        /// Excluir Grupo de Teste
        /// </sumary>
        /// <param name="idGrupoTeste">ID do Grupo de Teste</param>
        /// <returns>Confirmação de Exclusão</returns>
        private void Excluir(Int32 idGrupoTeste)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTeste está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPTST_ID", idGrupoTeste);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMGGRPTST_DEL");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Obtem as Restrições de Exclusão do Grupo de Teste carregada
        /// </summary>
        private DataTable ObterRestricoesExclusao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTeste está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPTST_ID", IdGrupoTeste);

                dt = new DataTable();
                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPTST_SEL_RESTRIC_DEL").Tables[0];

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
        /// Incluir Grupo de Teste 
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="nomeGrupoTeste">Nome do Grupo de Teste</param>
        /// <param name="dataInclusao">Data de Inclusão</param>
        /// <returns>Id do Grupo de Teste</returns>
        public Int32 IncluirGrupoTeste(Int32 codigoOrganizacao, 
                                       String nomeGrupoTeste, 
                                       DateTime dataInclusao)
        {
            _idGrupoTeste = Incluir(codigoOrganizacao, nomeGrupoTeste, dataInclusao);

            return _idGrupoTeste;
        }

        /// <summary>
        /// Incluir Grupo de Teste 
        /// </summary>
        /// <param name="codigoOrganizacao">Código da Organização</param>
        /// <param name="nomeGrupoTeste">Nome do Grupo de Teste</param>
        /// <param name="dataInclusao">Data de Inclusão</param>
        /// <returns>Id do Grupo de Teste</returns>
        private Int32 Incluir(Int32 codigoOrganizacao,
                              String nomeGrupoTeste,
                              DateTime dataInclusao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTeste está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGGRPTST_NM", nomeGrupoTeste.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGGRPTST_DH_USU_INS", dataInclusao);
                acessoDadosBase.AddParameter("@MGGRPTST_ID", _idGrupoTeste, ParameterDirection.Output);

                _idGrupoTeste = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGGRPTST_INS")[0]);

                return _idGrupoTeste;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Alterar um Grupo de Teste
        /// </summary>
        /// <param name="idGrupoTeste">ID do Grupo de Teste</param>
        /// <param name="codigoOrganizacao">Código da ORG</param>
        /// <param name="nomeGrupoTeste">Nome do Grupo de Teste</param>
        public Boolean AlterarGrupoTeste(Int32 idGrupoTeste, 
                                         Int32 codigoOrganizacao, 
                                         String nomeGrupoTeste)
        {
            Boolean _nome_grupo_teste_ja_existente = false;

            _nome_grupo_teste_ja_existente = Alterar(idGrupoTeste, codigoOrganizacao, nomeGrupoTeste);

            return _nome_grupo_teste_ja_existente;
        }

        /// <summary>
        /// Alterar um Grupo de Teste
        /// </summary>
        /// <param name="idGrupoTeste">ID do Grupo de Teste</param>
        /// <param name="codigoOrganizacao">Código da ORG</param>
        /// <param name="nomeGrupoTeste">Nome do Grupo de Teste</param>
        private Boolean Alterar(Int32 idGrupoTeste,
                                Int32 codigoOrganizacao,
                                String nomeGrupoTeste)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTeste está operando em Modo Entidade Only"); }

                Int32 _nome_grupo_teste_ja_existente = Int16.MinValue;

                acessoDadosBase.AddParameter("@MGGRPTST_ID", idGrupoTeste);
                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGGRPTST_NM", nomeGrupoTeste.Trim());
                acessoDadosBase.AddParameter("@MGGRPTST_RETORNO", _nome_grupo_teste_ja_existente, ParameterDirection.Output);

                _nome_grupo_teste_ja_existente = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGGRPTST_UPD")[0]);

                if (_nome_grupo_teste_ja_existente > 0)
                {
                    // Nome da Grupo de Teste já existe para outra Organização
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
        /// Listar Grupo de Teste
        /// </summary>
        /// <param name="codigoOrganizacao">Código da ORG</param>
        public DataTable Listar(Int32 codigoOrganizacao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTeste está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPTST_SEL_LISTAR").Tables[0];

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
        /// Preenche os Atributos da classe
        /// </summary>
        private void PreencherAtributos(ref DataTable dt)
        {
            __blnIsLoaded = false;

            if (dt.Rows.Count > 0)
            {
                IdGrupoTeste = Convert.ToInt32(dt.Rows[0]["MGGRPTST_ID"]);
                CodigoOrganizacao = Convert.ToInt32(dt.Rows[0]["ORG_CD"]);
                NomeGrupoTeste = dt.Rows[0]["MGGRPTST_NM"].ToString();
                IdUsuarioInclusao = Convert.ToInt32(dt.Rows[0]["USU_ID_INS"]);
                DataInclusao = Convert.ToDateTime(dt.Rows[0]["MGGRPTST_DH_USU_INS"]);

                __blnIsLoaded = true;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("MGGRPTST_ID")) { dt.Columns["MGGRPTST_ID"].ColumnName = "IdGrupoTeste"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("MGGRPTST_NM")) { dt.Columns["MGGRPTST_NM"].ColumnName = "NomeGrupoTeste"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioCadastro"; }
            if (dt.Columns.Contains("MGGRPTST_DH_USU_INS")) { dt.Columns["MGGRPTST_DH_USU_INS"].ColumnName = "DataInclusao"; }

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }
        #endregion Métodos
    }
}
