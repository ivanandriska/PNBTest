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
    public class GrupoTesteDestinatario : BusinessBase
    {
        #region MemoryCache
        private const String kCacheKey = "CaseBusiness_CC_Mensageria_GrupoTesteDestinatario_DataTable";
        private const Int16 kCache_ABSOLUTEEXPIRATION_MINUTES = 1440;  // 60 minutos * 24 horas
        #endregion MemoryCache

        #region Atributos
        private Int32 _idGrupoTesteDestinatario = Int32.MinValue;
        private Int32 _idGrupoTeste = Int32.MinValue;
        private String _nomeDestinatario = String.Empty;
        private String _descricaoDestinatario = String.Empty;
        private Int32 _idUsuarioInclusao = Int32.MinValue;
        private DateTime _dataInclusao = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdGrupoTesteDestinatario
        {
            get { return _idGrupoTesteDestinatario; }
            set { _idGrupoTesteDestinatario = value; }
        }

        public Int32 IdGrupoTeste
        {
            get { return _idGrupoTeste; }
            set { _idGrupoTeste = value; }
        }

        public String NomeDestinatario
        {
            get { return _nomeDestinatario; }
            set { _nomeDestinatario = value; }
        }

        public String DescricaoDestinatario
        {
            get { return _descricaoDestinatario; }
            set { _descricaoDestinatario = value; }
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
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe GrupoTesteDestinatario - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public GrupoTesteDestinatario(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe GrupoTesteDestinatario
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public GrupoTesteDestinatario(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe GrupoTesteDestinatario utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public GrupoTesteDestinatario(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }
        #endregion Construtores

        #region Metodos
        /// <summary>
        /// Incluir Destinatário do Grupo de Teste 
        /// </summary>
        /// <param name="idGrupoTeste">Id do Grupode Teste</param>
        /// <param name="nomeDestinatario">Nome do Destinatário</param>
        /// <param name="descricaoDestinatario">Descrição dop Destinatário</param>
        /// <param name="dataInclusao">Data Inclusão</param>
        /// <returns>Inclusão com Sucesso</returns>
        public Int32 IncluirGrupoTesteDestinatario(Int32 idGrupoTeste,
                                                         String nomeDestinatario,
                                                         String descricaoDestinatario,
                                                         DateTime dataInclusao)
        {
            _idGrupoTesteDestinatario = Incluir(idGrupoTeste, nomeDestinatario, descricaoDestinatario, dataInclusao);

            return _idGrupoTesteDestinatario;
        }

        /// <summary>
        /// Incluir Destinatário do Grupo de TEste 
        /// </summary>
        /// <param name="idGrupoTeste">Id do Grupode Teste</param>
        /// <param name="nomeDestinatario">Nome do Destinatário</param>
        /// <param name="descricaoDestinatario">Descrição do Destinatário</param>
        /// <param name="dataInclusao">Data Inclusão</param>
        /// <returns>Id do Grupo de Teste Destinatário</returns>
        private Int32 Incluir(Int32 idGrupoTeste,
                              String nomeDestinatario,
                              String descricaoDestinatario,
                              DateTime dataInclusao)
        {
            try
            {
                // Obs.: No momento o nomeDestinatario esta recebendo somente o ddd e número de celular, quando for outro tipo fazer o devido tratamento

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteDestinatario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPTST_ID", idGrupoTeste);
                acessoDadosBase.AddParameter("@MGGRPTSTDEST_NM", Util.RemoveFormat(nomeDestinatario.Trim()));
                acessoDadosBase.AddParameter("@MGGRPTSTDEST_DS", descricaoDestinatario.Trim());
                acessoDadosBase.AddParameter("@USU_ID_INS", base.UsuarioManutencao.ID);
                acessoDadosBase.AddParameter("@MGGRPTSTDEST_DH_USU_INS", dataInclusao);
                acessoDadosBase.AddParameter("@MGGRPTSTDEST_ID", _idGrupoTesteDestinatario, ParameterDirection.Output);

                _idGrupoTesteDestinatario = Convert.ToInt16(acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGGRPTSTDEST_INS")[0]);

                return _idGrupoTesteDestinatario;
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Exclui um Destinatário do Grupo de Teste 
        /// </summary>        
        /// <param name="idGrupoTesteDestinatario">Id do Destinatário do Grupo de Teste</param>
        public void ExcluirGrupoTesteDestinatario(Int32 idGrupoTesteDestinatario)
        {
            Excluir(idGrupoTesteDestinatario);
        }

        /// <summary>
        /// Exclui um Destinatário do Grupo de Teste 
        /// </summary>
        /// <param name="idGrupoTesteDestinatario">Id do Destinatário do Grupo de Teste</param>
        private void Excluir(Int32 idGrupoTesteDestinatario)
        {
            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteDestinatario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPTSTDEST_ID", idGrupoTesteDestinatario);

                acessoDadosBase.ExecuteNonQueryComRetorno(CommandType.StoredProcedure, "prMGGRPTSTDEST_DEL");

                // MemoryCache Clean
                RemoverCache();
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
        /// Buscar Todos os Destinatário do Grupo de Teste
        /// </summary>
        /// <param name="codigoOrganizacao">Código Organização</param>
        /// <param name="idGrupoTeste">Id Grupo de Teste</param>
        /// <param name="nomeDestinatario">Nome do Destinatário</param>
        /// <param name="descricaoDestinatario">Descrição do Destinatário</param>
        public DataTable Buscar(Int32 codigoOrganizacao,
                                Int32 idGrupoTeste,
                                String nomeDestinatario,
                                String descricaoDestinatario)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteDestinatario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@ORG_CD", codigoOrganizacao);
                acessoDadosBase.AddParameter("@MGGRPTST_ID", idGrupoTeste);
                acessoDadosBase.AddParameter("@MGGRPTSTDEST_NM", nomeDestinatario);
                acessoDadosBase.AddParameter("@MGGRPTSTDEST_DS", descricaoDestinatario);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPTSTDEST_SEL_BUSCAR").Tables[0];

                // Renomear Colunas
                RenomearColunas(ref dt);

                #region Mascara
                foreach (DataRow dr in dt.Rows)
                {
                    //Colocando máscara no ddd/celular do destinatário
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["NomeDestinatario"])))
                    {
                        _nomeDestinatario = Convert.ToString(dr["NomeDestinatario"]);
                        dr["NomeDestinatario"] = Util.Telefone_ComMascara(Util.RemoveFormat(_nomeDestinatario), Util.enumTipoTelefone.CELULAR);
                    }
                }
                #endregion Mascara
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Consultar uma Destinatário por ID
        /// </summary>
        /// <param name="idGrupoTesteDestinatario">Id do Destinatário do Grupo de Teste</param>
        public DataTable Consultar(Int32 idGrupoTesteDestinatario)
        {
            try
            {
                DataTable dt = null;

                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe GrupoTesteDestinatario está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@MGGRPTST_ID", idGrupoTesteDestinatario);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure, "prMGGRPTSTDEST_SEL_CONSULTAID").Tables[0];

                RenomearColunas(ref dt);

                return dt;
                
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }
        }

        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("MGGRPTST_ID")) { dt.Columns["MGGRPTST_ID"].ColumnName = "IdGrupoTeste"; }
            if (dt.Columns.Contains("MGGRPTST_NM")) { dt.Columns["MGGRPTST_NM"].ColumnName = "NomeGrupoTeste"; }
            if (dt.Columns.Contains("ORG_CD")) { dt.Columns["ORG_CD"].ColumnName = "CodigoOrganizacao"; }
            if (dt.Columns.Contains("ORG_CD_NM")) { dt.Columns["ORG_CD_NM"].ColumnName = "CodigoNomeOrganizacao"; }
            if (dt.Columns.Contains("MGGRPTSTDEST_ID")) { dt.Columns["MGGRPTSTDEST_ID"].ColumnName = "IdGrupoTesteDestinatario"; }
            if (dt.Columns.Contains("MGGRPTSTDEST_NM")) { dt.Columns["MGGRPTSTDEST_NM"].ColumnName = "NomeDestinatario"; }
            if (dt.Columns.Contains("MGGRPTSTDEST_DS")) { dt.Columns["MGGRPTSTDEST_DS"].ColumnName = "DescricaoDestinatario"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioInclusao"; }
            if (dt.Columns.Contains("MGGRPTSTDEST_DH_USU_INS")) { dt.Columns["MGGRPTSTDEST_DH_USU_INS"].ColumnName = "DataInclusao"; }
        }
        #endregion Metodos
    }
}