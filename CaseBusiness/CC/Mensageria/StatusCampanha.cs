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
    public class StatusCampanha
    {
        #region Enums e Constantes
        public enum enumStatus { EMPTY, ATIVA, INATIVA, ELABORACAO, TESTE, TSTEXECUTADO, EMEXECUCAO, TSTEXECUTANDO }
        private const string kStatus_ATIVA = "ATIV";
        private const string kStatus_INATIVA = "INAT";
        private const string kStatus_ELABORACAO = "ELAB";
        private const string kStatus_TESTE = "TEST";
        private const string kStatus_TSTEXECUTADO = "TSTEX";
        private const string kStatus_EMEXECUCAO = "EMEXE";
        private const string kStatus_TSTEXECUTANDO = "TSTEC";

        #endregion Enums e Constantes

        #region Métodos
        public static enumStatus ObterEnum(String status_DBValue)
        {
            enumStatus enm = enumStatus.EMPTY;

            switch (status_DBValue.Trim().ToUpper())
            {
                case kStatus_ATIVA: enm = enumStatus.ATIVA; break;
                case kStatus_INATIVA: enm = enumStatus.INATIVA; break;
                case kStatus_ELABORACAO: enm = enumStatus.ELABORACAO; break;
                case kStatus_TESTE: enm = enumStatus.TESTE; break;
                case kStatus_TSTEXECUTADO: enm = enumStatus.TSTEXECUTADO; break;
                case kStatus_EMEXECUCAO: enm = enumStatus.EMEXECUCAO; break;
                case kStatus_TSTEXECUTANDO: enm = enumStatus.TSTEXECUTANDO; break;
            }

            return enm;
        }


        public static String ObterDBValue(enumStatus enmStatus)
        {
            String DBValue = String.Empty;

            switch (enmStatus)
            {
                case enumStatus.ATIVA: DBValue = kStatus_ATIVA; break;
                case enumStatus.INATIVA: DBValue = kStatus_INATIVA; break;
                case enumStatus.ELABORACAO: DBValue = kStatus_ELABORACAO; break;
                case enumStatus.TESTE: DBValue = kStatus_TESTE; break;
                case enumStatus.TSTEXECUTADO: DBValue = kStatus_TSTEXECUTADO; break;
                case enumStatus.EMEXECUCAO: DBValue = kStatus_EMEXECUCAO; break;
                case enumStatus.TSTEXECUTANDO: DBValue = kStatus_TSTEXECUTANDO; break;
            }

            return DBValue;
        }


        public static String ObterTexto(enumStatus enmStatus)
        {
            String texto = String.Empty;
            //System.Globalization.CultureInfo myCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Resources.ResourceManager myResManager = new System.Resources.ResourceManager("CaseBusiness.Resources.CC.ALM.StatusFila", System.Reflection.Assembly.GetExecutingAssembly());

            switch (enmStatus)
            {
                case enumStatus.ATIVA: texto = "Ativa"; break;
                case enumStatus.INATIVA: texto = "Inativa"; break;
                case enumStatus.ELABORACAO: texto = "Em Elaboração"; break;
                case enumStatus.TESTE: texto = "Em Teste"; break;
                case enumStatus.TSTEXECUTADO: texto = "Teste Executado"; break;
                case enumStatus.EMEXECUCAO: texto = "Em Execução"; break;
                case enumStatus.TSTEXECUTANDO: texto = "Teste Executando"; break;
            }

            //myCultureInfo = null;
            // myResManager = null;

            return texto;
        }
        #endregion Métodos
    }
}

