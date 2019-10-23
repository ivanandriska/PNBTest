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
    public class PreProcessamento : BusinessBase
    {
        #region Atributos
        private Int32 _idPreProcessamento = Int32.MinValue;
        private String _tipoModeloEstatistico = String.Empty;
        private String _flagOrigem = String.Empty;
        private DateTime _dataBase = DateTime.MinValue;
        private DateTime _dhInicioProcessamento = DateTime.MinValue;
        private DateTime _dhFimProcessamento = DateTime.MinValue;
        private Int32 _idUsuarioIns = Int32.MinValue;
        private DateTime _dhUsarioIns = DateTime.MinValue;
        #endregion Atributos

        #region Propriedades
        public Int32 IdPreProcessamento
        {
            get { return _idPreProcessamento; }
            set { _idPreProcessamento = value; }
        }

        public String TipoModeloEstatistico
        {
            get { return _tipoModeloEstatistico; }
            set { _tipoModeloEstatistico = value; }
        }

        public String FlagOrigem
        {
            get { return _flagOrigem; }
            set { _flagOrigem = value; }
        }

        public DateTime DataBase
        {
            get { return _dataBase; }
            set { _dataBase = value; }
        }

        public DateTime DhInicioProcessamento
        {
            get { return _dhInicioProcessamento; }
            set { _dhInicioProcessamento = value; }
        }

        public DateTime DhFimProcessamento
        {
            get { return _dhFimProcessamento; }
            set { _dhFimProcessamento = value; }
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
        #endregion Propriedades

        #region Construtores
        /// <summary>
        /// Construtor classe PreProcessamento - Modo Entidade Only
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="modoEntidadeOnly">Modo Entidade Only - Valor = true</param>
        /// <remarks>Construtor específico pra ser usado nas propriedades/classes que necessitam herdar essa Entidade completa 
        /// mas sem criar uma instãncia desnecessário do AcessoDados</remarks>
        public PreProcessamento(Int32 idUsuarioManutencao, Boolean modoEntidadeOnly)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            __modoEntidadeOnly = true;       // Força True por segurança (se o desenv passar False, não criará conexao com o DB, gerando um Erro)
        }

        /// <summary>
        /// Construtor classe PreProcessamento
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public PreProcessamento(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }

        /// <summary>
        /// Construtor classe PreProcessamento utilizando uma Transação
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        /// <param name="transacao">Transação Corrente</param>
        public PreProcessamento(Int32 idUsuarioManutencao,
                           CaseBusiness.Framework.BancoDados.Transacao transacao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(transacao);
        }

        /// <summary>
        /// Construtor classe PreProcessamento e já preenche as propriedades com os dados da chave informada
        /// </summary>
        /// <param name="codigoGrupo">Código do PreProcessamento</param>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        //public PreProcessamento(Int32 idPreProcessamento,
        //                 Int32 idUsuarioManutencao)
        //    : this(idUsuarioManutencao)
        //{
        //    Consultar(idPreProcessamento);
        //}

        #endregion Construtores

        #region Métodos
        /// <summary>
        /// Buscar PreProcessamento
        /// </summary>
        public DataTable Listar()
        {
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe PreProcessamento está operando em Modo Entidade Only"); }


                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMPPROC_SEL_LISTAR").Tables[0];

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
        /// Verifica se já ocorreu pre processamento para a Data Base informada
        /// </summary>
        /// <param name="dtBase">Data Base</param>
        /// <returns>True = Há pré processamento executado/ False = Não há pre processamento executado</returns>
        public Boolean VerificaPreProcessamentoExecutadoDataBase(DateTime dtBase)
        {
            Boolean isExecutado = false;
            DataTable dt = null;

            try
            {
                if (ModoEntidadeOnly) { throw new ApplicationException("Esta Instância da classe PreProcessamento está operando em Modo Entidade Only"); }

                acessoDadosBase.AddParameter("DT_BASE", dtBase);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                    "prCMPPROC_SEL_VERIF_DATABASE").Tables[0];

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["QTDE_EXECUTADO"]) > 0)
                            isExecutado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                CaseBusiness.Framework.Log.Log.Logar(CaseBusiness.Framework.TipoLog.Erro, ex.Message, ex.StackTrace, "", "", DateTime.Now, CaseBusiness.Framework.Configuracao.Configuracao.Aplicacao, CaseBusiness.Framework.Tela.Nenhum, 0);
                throw;
            }

            return isExecutado;
        }



        /// <summary>
        /// Renomeia colunas do DB com os nomes das Propriedades
        /// </summary>
        private void RenomearColunas(ref DataTable dt)
        {
            if (dt.Columns.Contains("CMPPROC_ID")) { dt.Columns["CMPPROC_ID"].ColumnName = "IdPreProcessamento"; }
            if (dt.Columns.Contains("CMPPROC_TP_MDL_ESTATISTIC")) { dt.Columns["CMPPROC_TP_MDL_ESTATISTIC"].ColumnName = "ModeloEstatistico"; }
            if (dt.Columns.Contains("CMPPROC_FL_ORIGEM")) { dt.Columns["CMPPROC_FL_ORIGEM"].ColumnName = "Origem"; }
		    if (dt.Columns.Contains("CMPPROC_DT_BASE")) { dt.Columns["CMPPROC_DT_BASE"].ColumnName = "DataBase"; }
		    if (dt.Columns.Contains("CMPPROC_DH_INICIO_PROC")) { dt.Columns["CMPPROC_DH_INICIO_PROC"].ColumnName = "DataHoraInicio"; }
		    if (dt.Columns.Contains("CMPPROC_DH_FIM_PROC")) { dt.Columns["CMPPROC_DH_FIM_PROC"].ColumnName = "DataHoraFim"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "UsuarioResponsavel"; }
            if (dt.Columns.Contains("CMPPROC_DH_USUARIO_INS")) { dt.Columns["CMPPROC_DH_USUARIO_INS"].ColumnName = "DataHoraInclusao"; }

            if (dt.Columns.Contains("CMDESTAT_QT_VP")) { dt.Columns["CMDESTAT_QT_VP"].ColumnName = "QtdVerdadeiroPositivo"; }
            if (dt.Columns.Contains("CMDESTAT_QT_FP")) { dt.Columns["CMDESTAT_QT_FP"].ColumnName = "QtdFalsoPositivo"; }
            if (dt.Columns.Contains("CMDESTAT_QT_FN")) { dt.Columns["CMDESTAT_QT_FN"].ColumnName = "QtdFalsoNegativo"; }
            if (dt.Columns.Contains("CMDESTAT_QT_VN")) { dt.Columns["CMDESTAT_QT_VN"].ColumnName = "QtdVerdadeiroNegativo"; }

            if (dt.Columns.Contains("STCMPPROC_ST")) { dt.Columns["STCMPPROC_ST"].ColumnName = "Status"; }
            if (dt.Columns.Contains("STCMPPROC_DS")) { dt.Columns["STCMPPROC_DS"].ColumnName = "DescricaoStatus"; }
        }
        #endregion Métodos
    }
}
