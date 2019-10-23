using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace CaseBusiness.Framework.Mail.Entidade
{
    public class Email
    {
        #region Atributos
        private String deNome = String.Empty;
        private String para = String.Empty;
        private String paraNome = String.Empty;
        private String cc = String.Empty;
        private String co = String.Empty;
        private String assunto = String.Empty;
        private String corpo = String.Empty;
        private String corpoHTML = String.Empty;
        private String anexo = String.Empty;
        private Boolean enableHTML = false;
        private MailPriority prioridade = MailPriority.Normal;
        #endregion Atributos

        #region Propriedades
    
        public String Para
        {
            get { return para; }
            set { para = value; }
        }

        public String ParaNome
        {
            get { return paraNome; }
            set { paraNome = value; }
        }

        public String Cc
        {
            get { return cc; }
            set { cc = value; }
        }

        public String Co
        {
            get { return co; }
            set { co = value; }
        }


        public String Assunto
        {
            get { return assunto; }
            set { assunto = value; }
        }

        public String Corpo
        {
            get { return corpo; }
            set { corpo = value; }
        }

        public String CorpoHTML
        {
            get { return corpoHTML; }
            set { corpoHTML = value; }
        }

        public String Anexo
        {
            get { return anexo; }
            set { anexo = value; }
        }

        public Boolean EnableHTML
        {
            get { return enableHTML; }
            set { enableHTML = value; }
        }

        public MailPriority Prioridade
        {
            get { return prioridade; }
            set { prioridade = value; }
        }

        public String DeNome
        {
            get { return deNome; }
            set { deNome = value; }
        }
        #endregion Propriedades

    }
}
