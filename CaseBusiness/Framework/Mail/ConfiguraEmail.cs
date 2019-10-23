using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Framework.Mail
{
    public class ConfiguraEmail
    {
        #region Atributos
        private String de = String.Empty;
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

        public String De
        {
            get { return de; }
            set { de = value; }
        }

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

        #region Construtores

        //public ConfiguraEmail(
        //    String de,
        //    String para,
        //    MailAddress paraLista,
        //    String cc,
        //    String co,
        //    String assunto,
        //    String corpo,
        //    String corpoHTML,
        //    String anexo,
        //    Boolean enableHTML,
        //    MailPriority prioridade
        //    )
        //{
        //    this.De = de;
        //    this.DeNome = deNome;
        //    this.Para = para;
        //    this.ParaLista = new MailAddress(para, paraNome);
        //    this.ParaNome = paraNome;
        //    this.Cc = cc;
        //    this.Co = co;
        //    this.Assunto = assunto;
        //    this.Corpo = corpo;
        //    this.CorpoHTML = corpoHTML;
        //    this.Anexo = anexo;
        //    this.EnableHTML = enableHTML;
        //    this.Prioridade = prioridade;
        //}
        //public ConfiguraEmail()
        //{
        //    this.De = String.Empty;
        //    this.DeNome = String.Empty;
        //    this.Para = String.Empty;
        //    this.ParaLista = new MailAddress(para, paraNome);
        //    this.ParaNome = String.Empty;
        //    this.Cc = String.Empty;
        //    this.Co = String.Empty;
        //    this.Assunto = String.Empty;
        //    this.Corpo = String.Empty;
        //    this.CorpoHTML = String.Empty;
        //    this.Anexo = String.Empty;
        //    this.EnableHTML = false;
        //    this.Prioridade = MailPriority.Normal;
        //}


        #endregion Construtores

    }
}
