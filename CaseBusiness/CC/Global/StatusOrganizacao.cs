#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using CaseBusiness.Framework.BancoDados;
#endregion Using

namespace CaseBusiness.CC.Global
{
    public class StatusOrganizacao
    {
        #region Enums e Constantes
        public enum enumStatus { EMPTY, ATIVA, INATIVA }
        private const string kStatus_ATIVA = "A";
        private const string kStatus_INATIVA = "I";

        //private const string kStatus_Texto_ATIVA_ptBR = "Ativa";
        //private const string kStatus_Texto_INATIVA_ptBR = "Inativa";
        #endregion Enums e Constantes

        #region Métodos
        public static enumStatus ObterEnum(String status_DBValue)
        {
            enumStatus enm = enumStatus.EMPTY;

            switch (status_DBValue.Trim().ToUpper())
            {
                case kStatus_ATIVA: enm = enumStatus.ATIVA; break;
                case kStatus_INATIVA: enm = enumStatus.INATIVA; break;
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
            }

            return DBValue;
        }


        public static String ObterTexto(enumStatus enmStatus)
        {
            String texto = String.Empty;
            System.Globalization.CultureInfo myCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Resources.ResourceManager myResManager = new System.Resources.ResourceManager("CaseBusiness.Resources.CC.Global.StatusOrganizacao", System.Reflection.Assembly.GetExecutingAssembly());


            switch (enmStatus)
            {
                case enumStatus.ATIVA: texto = myResManager.GetString("Status_Texto_ATIVA", myCultureInfo); break;
                case enumStatus.INATIVA: texto = myResManager.GetString("Status_Texto_INATIVA", myCultureInfo); break;
            }


            myCultureInfo = null;
            myResManager = null;

            return texto;
        }
        #endregion Métodos
    }
}
