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
    public class UsuarioStatus : BusinessBase
    {
        #region Enums e Constantes
        public enum enumStatus { EMPTY, ATIVO, INATIVO, EXCLUIDO }
        public const string kStatus_ATIVO_DBValue = "A";
        public const string kStatus_INATIVO_DBValue = "I";
        public const string kStatus_EXCLUIDO_DBValue = "E";

        //public enum enumStatusTexto { Ativo, Inativo, Excluido }
        //public const string kStatus_ATIVO_Texto = "Ativo";
        //public const string kStatus_INATIVO_Texto = "Inativo";
        //public const string kStatus_EXCLUIDO_Texto = "Excluído";
        #endregion Enums e Constantes


        #region Construtores
        /// <summary>
        /// Construtor classe UsuarioStatusLog
        /// </summary>
        /// <param name="idUsuarioManutencao">ID do Usuário Logado</param>
        public UsuarioStatus(Int32 idUsuarioManutencao)
        {
            UsuarioManutencao.ID = idUsuarioManutencao;
            acessoDadosBase = new AcessoDadosBase(CaseBusiness.Framework.BancoDeDados.Case);
        }
        #endregion Construtores


        #region Métodos
        /// <summary>
        /// Lista Log de Alteração de Status de um Usuário
        /// </summary>
        /// <param name="idUsuario">ID do Usuário</param>
        public DataTable ListarLogAlteracao(Int32 idUsuario)
        {
            DataTable dt = null; 

            try
            {
                acessoDadosBase.AddParameter("@USU_ID", idUsuario);

                dt = acessoDadosBase.ExecuteDataSet(CommandType.StoredProcedure,
                                                            "prUSUSTLOG_SEL_BUSCAR").Tables[0];

                //// Renomear Colunas
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
            if (dt.Columns.Contains("USU_ID")) { dt.Columns["USU_ID"].ColumnName = "IdUsuario"; }
            if (dt.Columns.Contains("USUSTLOG_DH")) { dt.Columns["USUSTLOG_DH"].ColumnName = "DataAlteracao"; }
            if (dt.Columns.Contains("USU_ST")) { dt.Columns["USU_ST"].ColumnName = "UsuarioStatus"; }
            if (dt.Columns.Contains("USU_ST_TEXTO")) { dt.Columns["USU_ST_TEXTO"].ColumnName = "UsuarioStatusTexto"; }
            if (dt.Columns.Contains("USU_ID_INS")) { dt.Columns["USU_ID_INS"].ColumnName = "IdUsuarioManutencao"; }
            if (dt.Columns.Contains("USU_NM_INS")) { dt.Columns["USU_NM_INS"].ColumnName = "UsuarioManutencaoNome"; }
        }

        public static String ObterDBValue(enumStatus enmstatus)
        {
            String _dbvalue = String.Empty;

            switch (enmstatus)
            {
                case enumStatus.ATIVO: _dbvalue = kStatus_ATIVO_DBValue; break;
                case enumStatus.INATIVO: _dbvalue = kStatus_INATIVO_DBValue; break;
                case enumStatus.EXCLUIDO: _dbvalue = kStatus_EXCLUIDO_DBValue; break;
            }

            return _dbvalue;
        }

        public static enumStatus ObterEnum(String status)
        {
            enumStatus _enmstatus = enumStatus.EMPTY;

            switch (status.Trim().ToUpper().Substring(0, 1))
            {
                case kStatus_ATIVO_DBValue: _enmstatus = enumStatus.ATIVO; break;
                case kStatus_INATIVO_DBValue: _enmstatus = enumStatus.INATIVO; break;
                case kStatus_EXCLUIDO_DBValue: _enmstatus = enumStatus.EXCLUIDO; break;
            }

            return _enmstatus;
        }

        public static String ObterTexto(enumStatus enmStatus)
        {
            String texto = String.Empty;
            System.Globalization.CultureInfo myCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Resources.ResourceManager myResManager = new System.Resources.ResourceManager("CaseBusiness.Resources.CtrlAcesso.UsuarioStatus", System.Reflection.Assembly.GetExecutingAssembly());

            switch (enmStatus)
            {
                case enumStatus.ATIVO: texto = myResManager.GetString("Ativo", myCultureInfo); break;
                case enumStatus.INATIVO: texto = myResManager.GetString("Inativo", myCultureInfo); break;
                case enumStatus.EXCLUIDO: texto = myResManager.GetString("Excluído", myCultureInfo); break;
            }

            myCultureInfo = null;
            myResManager = null;

            return texto;
        }
        #endregion Métodos
    }
}
