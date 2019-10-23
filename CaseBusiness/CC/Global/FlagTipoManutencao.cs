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
    public class FlagTipoManutencao
    {
        #region Enums e Constantes
        public enum enumTipoManutencao { EMPTY, ADICIONADO, REMOVIDO }
        private const string kFlag_ADICIONADO = "ADI";
        private const string kFlag_REMOVIDO = "REM";

        //private const string kFlag_Texto_ADICIONADO = "Adicionado";
        //private const string kFlag_Texto_REMOVIDO = "Removido";
        #endregion Enums e Constantes


        #region Métodos
        public static enumTipoManutencao ObterEnum(String flagTipoManutencao_DBValue)
        {
            enumTipoManutencao enm = enumTipoManutencao.EMPTY;

            switch (flagTipoManutencao_DBValue.Trim().ToUpper())
            {
                case kFlag_ADICIONADO: enm = enumTipoManutencao.ADICIONADO; break;
                case kFlag_REMOVIDO: enm = enumTipoManutencao.REMOVIDO; break;
            }

            return enm;
        }

                public static String ObterDBValue(enumTipoManutencao enmStatus)
        {
            String DBValue = String.Empty;

            switch (enmStatus)
            {
                case enumTipoManutencao.ADICIONADO: DBValue = kFlag_ADICIONADO; break;
                case enumTipoManutencao.REMOVIDO: DBValue = kFlag_REMOVIDO; break;
            }

            return DBValue;
        }


        public static String ObterTexto(enumTipoManutencao enmStatus)
        {
            String texto = String.Empty;
            System.Globalization.CultureInfo myCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Resources.ResourceManager myResManager = new System.Resources.ResourceManager("CaseBusiness.Resources.CC.Global.FlagTipoManutencao", System.Reflection.Assembly.GetExecutingAssembly());

            switch (enmStatus)
            {
                case enumTipoManutencao.ADICIONADO: texto =  myResManager.GetString("Adicionado_Texto", myCultureInfo); break;
                case enumTipoManutencao.REMOVIDO: texto = myResManager.GetString("Removido_Texto", myCultureInfo); break;
            }

            myCultureInfo = null;
            myResManager = null;

            return texto;
        }
        #endregion Métodos
    }
}
