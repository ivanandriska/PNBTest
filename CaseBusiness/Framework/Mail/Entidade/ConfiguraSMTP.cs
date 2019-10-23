using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Mail.Entidade
{
    internal static class ConfiguraSMTP
    {
        #region Atributos
        private static String smtp = String.Empty;
        private static Int32 porta = Int32.MinValue;
        private static Boolean habilitarSsl = false;
        private static String dominio = String.Empty;
        private static String nome = String.Empty;
        private static String emailDe = String.Empty;
        private static String login = String.Empty;
        private static String senha = String.Empty;                

        #endregion Atributos

        #region Propriedades

        internal static String Smtp
        {
            get { return smtp; }
            set { smtp = value; }
        }

        internal static Int32 Porta
        {
            get { return porta; }
            set { porta = value; }
        }

        internal static Boolean HabilitarSsl
        {
            get { return habilitarSsl; }
            set { habilitarSsl = value; }
        }

        internal static String Dominio
        {
            get { return dominio; }
            set { dominio = value; }
        }

        internal static String Nome
        {
            get { return ConfiguraSMTP.nome; }
            set { ConfiguraSMTP.nome = value; }
        }
        
        internal static String EmailDe
        {
            get { return ConfiguraSMTP.emailDe; }
            set { ConfiguraSMTP.emailDe = value; }
        }

        public static String Login
        {
            get { return ConfiguraSMTP.login; }
            set { ConfiguraSMTP.login = value; }
        }

        internal static String Senha
        {
            get { return senha; }
            set { senha = value; }
        }







        #endregion Propriedades
    }
}
