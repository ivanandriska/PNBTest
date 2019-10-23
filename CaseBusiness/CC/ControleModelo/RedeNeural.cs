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
    public class RedeNeural : BusinessBase
    {
        #region Atributos
        private Int32 _cdRedeNeural = Int32.MinValue;
        private String _nmRedeNeural = String.Empty;
        private Int32 _cdRedeNeuralSRT = Int32.MinValue;
        private Int32 _idUsuarioIns = Int32.MinValue;
        private DateTime _dhUsarioIns = DateTime.MinValue;
        private Int32 _idUsuarioUpd = Int32.MinValue;
        private DateTime _dhUsarioUpd = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 CdRedeNeural
        {
            get { return _cdRedeNeural; }
            set { _cdRedeNeural = value; }
        }

        public String NmRedeNeural
        {
            get { return _nmRedeNeural; }
            set { _nmRedeNeural = value; }
        }

        public Int32 CdRedeNeuralSRT
        {
            get { return _cdRedeNeuralSRT; }
            set { _cdRedeNeuralSRT = value; }
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
        /// Construtor classe RedeNeural - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public RedeNeural(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe RedeNeural
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public RedeNeural(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe RedeNeural utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public RedeNeural(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe RedeNeural e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoGrupo">Código do RedeNeural</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        //public RedeNeural(Int32 idRedeNeural,
        //                 Int32 idUsuarioManutencao)
        //    : this(idUsuarioManutencao)
        //{
        //    Consultar(idRedeNeural);
        //}

        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar RedeNeural
        /// </summary>
        public DataTable Listar()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe RedeNeural está operando em Modo Entidade Only"); }


                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prSORD_SEL_LISTAR").Tables[0];

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
            if (dt.Columns.Contains("SORD_CD")) { dt.Columns["SORD_CD"].ColumnName = "CodigoRedeNeural"; }
            if (dt.Columns.Contains("SORD_NM")) { dt.Columns["SORD_NM"].ColumnName = "NomeRedeNeural"; }
            if (dt.Columns.Contains("SORD_CD_SRT")) { dt.Columns["SORD_CD_SRT"].ColumnName = "CodigoRedeNeuralSRT"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "UsuarioResponsavel"; }
            if (dt.Columns.Contains("CMPPROC_DH_USUARIO_INS")) { dt.Columns["CMPPROC_DH_USUARIO_INS"].ColumnName = "DataHoraInclusao"; }
            if (dt.Columns.Contains("USU_ID_UPD")) { dt.Columns["USU_ID_UPD"].ColumnName = "UsuarioAlteracao"; }
            if (dt.Columns.Contains("CMPPROC_DH_USUARIO_UPD")) { dt.Columns["CMPPROC_DH_USUARIO_UPD"].ColumnName = "DataHoraAlteracao"; }
        }
        #endregion Métodos
    }
}
