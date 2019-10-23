using System;
using System.Collections.Generic;
using System.Text;

namespace CaseBusiness.Framework.Mail.Entidade
{
    internal static class Smtp
    {
        #region Atributos
        private static String protocoloSmtp = String.Empty;
        private static Int32 porta = Int32.MinValue;
        private static Boolean ssl = false;
        private static String dominio = String.Empty;
        private static String nome = String.Empty;
        private static String emailDe = String.Empty;
        private static String login = String.Empty;
        private static String senha = String.Empty;                

        #endregion Atributos

        #region Propriedades

        internal static String ProtocoloSmtp
        {
            get { return protocoloSmtp; }
            set { protocoloSmtp = value; }
        }

        internal static Int32 Porta
        {
            get { return porta; }
            set { porta = value; }
        }

        internal static Boolean Ssl
        {
            get { return ssl; }
            set { ssl = value; }
        }

        internal static String Dominio
        {
            get { return dominio; }
            set { dominio = value; }
        }

        internal static String Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        
        internal static String EmailDe
        {
            get { return emailDe; }
            set { emailDe = value; }
        }

        internal static String Login
        {
            get { return login; }
            set { login = value; }
        }

        internal static String Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        #endregion Propriedades
    }
}
