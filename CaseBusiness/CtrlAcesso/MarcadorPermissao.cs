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
    public class MarcadorPermissao : BusinessBase
    {
        #region Atributos
        private Int32 _idMarcadorPermissao = Int32.MinValue;
        private String _descricao = "";
        private DataTable _dtRestricoesExclusao;
        #endregion Atributos


        #region Propriedades
        public Int32 IdMarcadorPermissao
        {
            get { return _idMarcadorPermissao; }
            set { _idMarcadorPermissao = value; }
        }

        public String Descricao
        {
            get { return _descricao; }
            set { _descricao = value; }
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
                { return false; }
                else
                {
                    if (_dtRestricoesExclusao.Rows.Count <= 0)
                    { return true; }
                    else
                    { return false; }
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
        /// Construtor classe MarcadorPermissao - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public MarcadorPermissao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe MarcadorPermissao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public MarcadorPermissao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe MarcadorPermissao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public MarcadorPermissao(Int32 idUsuarioManutencao,
                                 CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe MarcadorPermissao e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="IdGrupo">Id Marcador Permissão</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public MarcadorPermissao(Int32 idMarcadorPermissao,
                                 Int32 idUsuarioManutencao)
            : this(idUsuarioManutencao)
        {
            Consultar(idMarcadorPermissao);
        }

        #endregion Construtores


        #region Métodos

        /// <summary>
        /// Buscar Marcador Permissão
        /// </summary>
        /// <param name="IdMarcadorPermissao">Id Marcador Permissão</param>
        /// <param name="nomeMarcadorPermissao">Nome Marcador Permissão</param>
        public DataTable Buscar(String nomeMarcadorPermissao)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MarcadorPermissao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MARCPERM_DS", nomeMarcadorPermissao.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMARCPERM_SEL_BUSCAR").Tables[0];

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
        /// Consultar um Marcador Permissão
        /// </summary>
        /// <param name="IdMarcadorPermissao">Id Marcador Permissão</param>
        private void Consultar(Int32 idMarcadorPermissao)
        {
            try
            {
                DataTable dt;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MarcadorPermissao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MARCPERM_ID", idMarcadorPermissao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMARCPERM_SEL_CONSULTAR").Tables[0];

                // Fill Object
                __blnIsLoaded = false;

                if (dt.Rows.Count > 0)
                {
                    IdMarcadorPermissao = Convert.ToInt32(dt.Rows[0]["MARCPERM_ID"]);
                    Descricao = Convert.ToString(dt.Rows[0]["MARCPERM_DS"]);

                    __blnIsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Inclui um Marcador Permissão
        /// </summary>
        /// <param name="IdMarcadorPermissao">Código do Marcador Permissão</param>
        /// <param name="nomeMarcadorPermissao">Nome do Marcador Permissão</param>
        public void Incluir(String nomeMarcadorPermissao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MarcadorPermissao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MARCPERM_DS", nomeMarcadorPermissao.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMARCPERM_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Alterar um Marcador Permissão
        /// </summary>
        /// <param name="IdMarcadorPermissao">Código do Marcador Permissão</param>
        /// <param name="nomeMarcadorPermissao">Nome do Marcador Permissão</param>
        public void Alterar(Int32 IdMarcadorPermissao,
                            String nomeMarcadorPermissao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MarcadorPermissao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MARCPERM_ID", IdMarcadorPermissao);
                acessoDadosBase.AddParameter("@MARCPERM_DS", nomeMarcadorPermissao.Trim());
                acessoDadosBase.AddParameter("@USU_ID_UPD", base.UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMARCPERM_UPD");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        ///// <summary>
        ///// Exclui um Grupo 
        ///// </summary>
        ///// <param name="IdMarcadorPermissao">Código do Marcador Permissão</param>
        public void Excluir(Int32 IdMarcadorPermissao)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MarcadorPermissao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MARCPERM_ID", IdMarcadorPermissao);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure, "prMARCPERM_DEL");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Obtem as Restrições de Exclusão da Regra carregada
        /// </summary>
        private DataTable ObterRestricoesExclusao()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe MarcadorPermissao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MARCPERM_ID", IdMarcadorPermissao);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prMARCPERM_SEL_RESTRIC_DEL").Tables[0];

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
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("MARCPERM_ID")) { dt.Columns["MARCPERM_ID"].ColumnName = "IdMarcadorPermissao"; }
            if (dt.Columns.Contains("MARCPERM_DS")) { dt.Columns["MARCPERM_DS"].ColumnName = "Descricao"; }

            if (dt.Columns.Contains("ROWS_COUNT")) { dt.Columns["ROWS_COUNT"].ColumnName = "RestricoesExclusaoQtde"; }
            if (dt.Columns.Contains("RESTRICAO_TABELA")) { dt.Columns["RESTRICAO_TABELA"].ColumnName = "RestricoesExclusaoTabela"; }
        }
        #endregion Métodos
    }
}
