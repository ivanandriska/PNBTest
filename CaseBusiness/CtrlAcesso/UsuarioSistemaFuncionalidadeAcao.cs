#if NET472
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
    public class UsuarioSistemaFuncionalidadeAcao : BusinessBase
    {
        #region Construtores
        ///// <summary>
        ///// Construtor classe UsuarioSistemaFuncionalidadeAcao - Modo Entidade Only
        ///// </summary>
        ///// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        ///// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        ///// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        ///// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        //public UsuarioSistemaFuncionalidadeAcao(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        //{
        //    UsuarioManutencao.ID = idUsuarioManutencao;
        //    __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        //}

        /// <summary>
        /// Construtor classe UsuarioSistemaFuncionalidadeAcao
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public UsuarioSistemaFuncionalidadeAcao(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe UsuarioSistemaFuncionalidadeAcao utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public UsuarioSistemaFuncionalidadeAcao(Int32 idUsuarioManutencao,
                                                CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores


        #region Métodos
        /// <summary>
        /// Aplicar as novas Permissões de Acesso a um Usuário
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="dtNovasPermissoes">DataTable contendo todas as Ações e suas respectivas novas Permissões</param>
        public void AplicarPermissoes(Int32 idUsuario,
                                      List<SistemaFuncionalidadeAcao_TreeViewNodeAcao> nodesAcao, 
                                      ref Int32 totalPermissoesAdicionadas ,
                                      ref Int32 totalPermissoesRemovidas)
        {
            DataView dvAntigasPermissoes;

            // Obtem as atuais Permissões do Usuário
            dvAntigasPermissoes = Buscar(idUsuario,
                                         Int32.MinValue,
                                         Int32.MinValue,
                                         String.Empty).DefaultView;

            totalPermissoesAdicionadas = 0;
            totalPermissoesRemovidas= 0;

            foreach (SistemaFuncionalidadeAcao_TreeViewNodeAcao tvna in nodesAcao)
            {
                if (tvna.NodeChecked.HasValue)
                {
                    if (tvna.NodeChecked.Value)
                    {
                        //
                        // ADICIONAR PERMISSAO
                        //

                        // Verifica se o Usuário já possui esta Permissão.
                        // Se não possuir, insere.
                        dvAntigasPermissoes.RowFilter = "IdSistema = " + tvna.IdSistema.ToString()
                                                      + " and IdSistemaFuncionalidade = " + tvna.IdSistemaFuncionalidade.ToString()
                                                      + " and CodigoSistemaFuncionalidadeAcao = '" + tvna.CodigoSistemaFuncionalidadeAcao + "'";
                        if (dvAntigasPermissoes.Count <=0)
                        {
                            Incluir(idUsuario,
                                    tvna.IdSistema,
                                    tvna.IdSistemaFuncionalidade,
                                    tvna.CodigoSistemaFuncionalidadeAcao);

                            totalPermissoesAdicionadas++;
                        }
                    }
                    else
                    {
                        //
                        // REMOVER PERMISSAO
                        //

                        // Verifica se o Usuário possui esta Permissão.
                        // Se possuir, deleta.
                        dvAntigasPermissoes.RowFilter = "IdSistema = " + tvna.IdSistema.ToString()
                                                      + " and IdSistemaFuncionalidade = " + tvna.IdSistemaFuncionalidade.ToString()
                                                      + " and CodigoSistemaFuncionalidadeAcao = '" + tvna.CodigoSistemaFuncionalidadeAcao + "'";
                        if (dvAntigasPermissoes.Count > 0)
                        {
                            Excluir(idUsuario,
                                    tvna.IdSistema,
                                    tvna.IdSistemaFuncionalidade,
                                    tvna.CodigoSistemaFuncionalidadeAcao);

                            totalPermissoesRemovidas++;
                        }
                    }
                }
            }

            dvAntigasPermissoes = null;
        }



        /// <summary>
        /// Inclui uma Permissão de Acesso para um Usuário a uma Ação
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="idSistema">ID do Sistema</param>
        /// <param name="idSistemaFuncionalidade">ID da Funcionalidade do Sistema</param>
        /// <param name="codigoSistemaFuncionalidadeAcao">Código da Ação na Funcionalidade do Sistema</param>
        public void Incluir(Int32 idUsuario,
                            Int32 idSistema,
                            Int32 idSistemaFuncionalidade,
                            String codigoSistemaFuncionalidadeAcao)
        {
            try
            {
                //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioSistemaFuncionalidadeAcao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SISTFCN_ID", idSistemaFuncionalidade);
                acessoDadosBase.AddParameter("@SISTFCNACAO_CD", codigoSistemaFuncionalidadeAcao.Trim().ToUpper());
                acessoDadosBase.AddParameter("@USU_ID_OPER", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prUSUFCNACAO_INS");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Exclui uma Permissão de Acesso para um Usuário a uma Ação
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="idSistema">ID do Sistema</param>
        /// <param name="idSistemaFuncionalidade">ID da Funcionalidade do Sistema</param>
        /// <param name="codigoSistemaFuncionalidadeAcao">Código da Ação na Funcionalidade do Sistema</param>
        public void Excluir(Int32 idUsuario,
                            Int32 idSistema,
                            Int32 idSistemaFuncionalidade,
                            String codigoSistemaFuncionalidadeAcao)
        {
            try
            {
                //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioSistemaFuncionalidadeAcao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SISTFCN_ID", idSistemaFuncionalidade);
                acessoDadosBase.AddParameter("@SISTFCNACAO_CD", codigoSistemaFuncionalidadeAcao.Trim().ToUpper());
                acessoDadosBase.AddParameter("@USU_ID_OPER", UsuarioManutencao.ID);

                acessoDadosBase.ExecuteNonQuerySemRetorno(CommandType.StoredProcedure,
                                                          "prUSUFCNACAO_DEL");
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }


                /// <summary>
        /// Buscar Permissões de Usuários
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="idSistema">ID do Sistema</param>
        /// <param name="idSistemaFuncionalidade">ID da Funcionalidade do Sistema</param>
        /// <param name="codigoSistemaFuncionalidadeAcao">Código da Ação na Funcionalidade do Sistema</param>
        public DataTable Buscar(Int32 idUsuario,
                                Int32 idSistema,
                                Int32 idSistemaFuncionalidade,
                                String codigoSistemaFuncionalidadeAcao)
        {
            return Buscar(idUsuario, 
                          idSistema, 
                          idSistemaFuncionalidade, 
                          codigoSistemaFuncionalidadeAcao, 
                          CaseBusiness.CtrlAcesso.UsuarioStatus.enumStatus.EMPTY);
        }


        /// <summary>
        /// Buscar Permissões de Usuários
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        /// <param name="idSistema">ID do Sistema</param>
        /// <param name="idSistemaFuncionalidade">ID da Funcionalidade do Sistema</param>
        /// <param name="codigoSistemaFuncionalidadeAcao">Código da Ação na Funcionalidade do Sistema</param>
        /// <param name="enmStatusUsuario">Status do Usuário</param>
        public DataTable Buscar(Int32 idUsuario,
                                Int32 idSistema,
                                Int32 idSistemaFuncionalidade,
                                String codigoSistemaFuncionalidadeAcao,
                                CaseBusiness.CtrlAcesso.UsuarioStatus.enumStatus enmStatusUsuario)
        {
            DataTable dt = null;
            String _statusUsuario = String.Empty;

            try
            {
                //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioSistemaFuncionalidadeAcao está operando em Modo Entidade Only"); }

                _statusUsuario = UsuarioStatus.ObterDBValue(enmStatusUsuario);
                //switch (enmStatusUsuario)
                //{
                //    case UsuarioStatusLog.enumStatus.EMPTY: _statusUsuario = String.Empty; break;
                //    case UsuarioStatusLog.enumStatus.ATIVO: _statusUsuario = CaseBusiness.CtrlAcesso.UsuarioStatusLog.kStatus_ATIVO_DBValue; break;
                //    case UsuarioStatusLog.enumStatus.INATIVO: _statusUsuario = CaseBusiness.CtrlAcesso.UsuarioStatusLog.kStatus_INATIVO_DBValue; break;
                //}

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);
                acessoDadosBase.AddParameter("@SIST_ID", idSistema);
                acessoDadosBase.AddParameter("@SISTFCN_ID", idSistemaFuncionalidade);
                acessoDadosBase.AddParameter("@SISTFCNACAO_CD", codigoSistemaFuncionalidadeAcao.Trim().ToUpper());
                acessoDadosBase.AddParameter("@USU_ST", _statusUsuario);

                // Habilita Usuarios ID Negativos 2RP.Net se o Logado for um deles
                if (this.UsuarioManutencao.ID < 0)
                {
                    acessoDadosBase.AddParameter("@FLAG_MOSTRA_USU_ID_NEGATIVO", "S");
                }

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSUFCNACAO_SEL_BUSCAR").Tables[0];

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
        /// Montar TreeView de Permissões de um Usuário
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        public DataTable MontarUsuarioTreeView(Int32 idUsuario)
        {
            DataTable dt = null;

            try
            {
                //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioSistemaFuncionalidadeAcao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSUFCNACAO_SEL_USUTREEVIEW").Tables[0];

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
        /// Montar Menu restringindo pelas Permissões de um Usuário
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        public DataTable MontarUsuarioMenu(Int32 idUsuario)
        {
            DataTable dt = null;

            try
            {
                //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioSistemaFuncionalidadeAcao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSUFCNACAO_SEL_USUMENU").Tables[0];

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
        /// Listar Permissões do Usuário Logado à Funcionalidade/Ação
        /// </summary>
        public DataTable MontarUsuarioFuncionalidadeAcao()
        {
            return MontarUsuarioFuncionalidadeAcao(UsuarioManutencao.ID);
        }


        /// <summary>
        /// Listar Permissões do Usuário à Funcionalidade/Ação
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        public DataTable MontarUsuarioFuncionalidadeAcao(Int32 idUsuario)
        {
            DataTable dt = null;

            try
            {
                //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioSistemaFuncionalidadeAcao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@USU_ID", idUsuario);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prUSUFCNACAO_SEL_USUACAO").Tables[0];

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


        ///// <summary>
        ///// Consultar um Qualificador Definição por Rótulo
        ///// </summary>
        ///// <param name="rotulo">Rótulo do Qualificador</param>
        //private void Consultar(String rotulo)
        //{
        //    try
        //    {
        //        DataTable dt;

        //        if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe UsuarioSistemaFuncionalidadeAcao está operando em Modo Entidade Only"); }

        //        acessoDadosBase.AddParameter("@QLFDEF_SG_ROTULO", rotulo.Trim().ToUpper());

        //        dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
        //                                            "prQLFDEF_SEL_ROTULO").Tables[0];

        //        // Fill Object
        //        __blnIsLoaded = false;

        //        if (dt.Rows.Count > 0)
        //        {
        //            IdQualificador = Convert.ToInt32(dt.Rows[0]["QLFDEF_ID"]);
        //            Rotulo = Convert.ToString(dt.Rows[0]["QLFDEF_SG_ROTULO"]);
        //            Nome = Convert.ToString(dt.Rows[0]["QLFDEF_NM"]);
        //            Palavra = Convert.ToString(dt.Rows[0]["QLFDEF_CD_PALAVRA"]);
        //            TipoDado = ObterEnum_TipoDado(Convert.ToString(dt.Rows[0]["QLFDEF_CD_TIPO_DADO"]));
        //            ValidadeQtde = Convert.ToInt32(dt.Rows[0]["QLFDEF_NR_VALID_QTDE"]);
        //            ValidadePeriodo = ObterEnum_ValidadePeriodo(Convert.ToString(dt.Rows[0]["QLFDEF_NM_VALID_PERIODO"]));

        //            __blnIsLoaded = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
        //        throw; 
        //    }
        //}

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("USU_ID")) { dt.Columns["USU_ID"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("USU_NM")) { dt.Columns["USU_NM"].ColumnName = "UsuarioNome"; }
            if (dt.Columns.Contains("USU_CD_LOGIN")) { dt.Columns["USU_CD_LOGIN"].ColumnName = "UsuarioLogin"; }
            if (dt.Columns.Contains("USU_ST")) { dt.Columns["USU_ST"].ColumnName = "UsuarioStatus"; }

            if (dt.Columns.Contains("SIST_ID")) { dt.Columns["SIST_ID"].ColumnName = "IdSistema"; }
            if (dt.Columns.Contains("SIST_NM")) { dt.Columns["SIST_NM"].ColumnName = "SistemaNome"; }

            if (dt.Columns.Contains("SISTFCN_ID")) { dt.Columns["SISTFCN_ID"].ColumnName = "IdSistemaFuncionalidade"; }
            if (dt.Columns.Contains("SISTFCN_MODULO_SG")) { dt.Columns["SISTFCN_MODULO_SG"].ColumnName = "SistemaFuncionalidadeModulo"; }
            if (dt.Columns.Contains("SISTFCN_NM")) { dt.Columns["SISTFCN_NM"].ColumnName = "SistemaFuncionalidadeNome"; }
            if (dt.Columns.Contains("SISTFCN_ST")) { dt.Columns["SISTFCN_ST"].ColumnName = "SistemaFuncionalidadeStatus"; }

            if (dt.Columns.Contains("SISTFCNACAO_CD")) { dt.Columns["SISTFCNACAO_CD"].ColumnName = "CodigoSistemaFuncionalidadeAcao"; }
            if (dt.Columns.Contains("SISTFCNACAO_DS")) { dt.Columns["SISTFCNACAO_DS"].ColumnName = "SistemaFuncionalidadeAcaoDescricao"; }
            if (dt.Columns.Contains("SISTFCNACAO_FG_MENU")) { dt.Columns["SISTFCNACAO_FG_MENU"].ColumnName = "SistemaFuncionalidadeAcaoFlagMenu"; }
            if (dt.Columns.Contains("SISTFCNACAO_ST")) { dt.Columns["SISTFCNACAO_ST"].ColumnName = "SistemaFuncionalidadeAcaoStatus"; }
        }

        #endregion Métodos
    }
}
#endif