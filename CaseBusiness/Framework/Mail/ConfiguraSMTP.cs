using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Mail
{
    public class ConfiguraSMTP
    {
        #region Atributos
        private String smtp = String.Empty;
        private String usuario = String.Empty;
        private String senha = String.Empty;
        private String dominio = String.Empty;
        private Int32 porta = Int32.MinValue;
        private Boolean enableSsl = false;

        #endregion Atributos

        #region Propriedades
        public String Smtp
        {
            get { return smtp; }
            set { smtp = value; }
        }

        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public String Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        public Boolean EnableSsl
        {
            get { return enableSsl; }
            set { enableSsl = value; }
        }


        public Int32 Porta
        {
            get { return porta; }
            set { porta = value; }
        }

        public String Dominio
        {
            get { return dominio; }
            set { dominio = value; }
        }

        #endregion Propriedades

        public ConfiguraSMTP(
            String smtp,
            String usuario,
            String senha,
            String dominio,
            Boolean enableSsl,
            Int32 porta
            )
        {
            this.Smtp = smtp;
            this.Usuario = usuario;
            this.Senha = senha;
            this.Dominio = dominio;
            this.EnableSsl = enableSsl;
            this.Porta = porta;
        }


        public ConfiguraSMTP()
        {
            this.Smtp = String.Empty;
            this.Usuario = String.Empty;
            this.Senha = String.Empty;
            this.Dominio = String.Empty;
            this.EnableSsl = false;
            this.Porta = 0;

        }

    }
}
