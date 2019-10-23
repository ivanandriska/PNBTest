#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.Suporte
{
    public class ArquivoSuporte : BusinessBase
    {
        #region Atributos
        private String _nomeProcedure = String.Empty;
        private String _codigoTabela = String.Empty;
        private Boolean _flagAtivo;
        private Boolean _flagSuporte;
        private Boolean _flagMelhoria;
        #endregion Atributos

        #region Propriedades
        public String NomeProcedure
        {
            get { return _nomeProcedure; }
            set { _nomeProcedure = value.Trim(); }
        }

        public String CodigoTabela
        {
            get { return _codigoTabela; }
            set { _codigoTabela = value.Trim(); }
        }

        public Boolean FlagAtivo
        {
            get { return _flagAtivo; }
            set { _flagAtivo = value; }
        }

        public Boolean FlagSuporte
        {
            get { return _flagSuporte; }
            set { _flagSuporte = value; }
        }

        public Boolean FlagMelhoria
        {
            get { return _flagMelhoria; }
            set { _flagMelhoria = value; }
        }
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe ArquivoSuporte - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public ArquivoSuporte(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

		/// <summary>
		/// Construtor classe ArquivoSuporte
		/// </summary>
		/// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
		public ArquivoSuporte(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

		/// <summary>
		/// Construtor classe ArquivoSuporte utilizando uma Transação
		/// </summary>
		/// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
		/// <param name="transacao">Transação Corrente</param>
		public ArquivoSuporte(Int32 idUsuarioManutencao,
                             CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        #endregion Construtores

        #region Métodos

        /// <summary>
        /// Listar Procedures
        /// </summary>
        /// <param name="tipoSuporte"></param>
        /// <returns></returns>
        public DataTable ListarProcedures(String tipoSuporte)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe Transacao está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("@TIPO", tipoSuporte.Trim());

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prSUP_SEL").Tables[0];

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
        /// Listar Procedures
        /// </summary>
        /// <param name="tipoSuporte"></param>
        /// <returns></returns>
        public DataTable ExecutarProcedure(String nomeProcedure,
                                           String dataInicial,
                                           String dataFinal,
                                           String horaInicial,
                                           String horaFinal)
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe ArquivoSuporte está operando em Modo Entidade Only"); }

                    acessoDadosBase.AddParameter("@DT_MANUT_INI", dataInicial);
                    acessoDadosBase.AddParameter("@DT_MANUT_FIM", dataFinal);
                    acessoDadosBase.AddParameter("@HR_MANUT_INI", horaInicial);
                    acessoDadosBase.AddParameter("@HR_MANUT_FIM", horaFinal);

                    dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                            nomeProcedure.Trim().ToUpper()).Tables[0];
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return dt;
        }


        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("SUP_NM_PROCEDURE")) { dt.Columns["SUP_NM_PROCEDURE"].ColumnName = "NomeProcedure"; }
            if (dt.Columns.Contains("SUP_CD_TABELA")) { dt.Columns["SUP_CD_TABELA"].ColumnName = "CodigoTabela"; }
            if (dt.Columns.Contains("SUP_FL_ATIVO")) { dt.Columns["SUP_FL_ATIVO"].ColumnName = "FlagAtivo"; }
        }
        #endregion Métodos
    }
}
