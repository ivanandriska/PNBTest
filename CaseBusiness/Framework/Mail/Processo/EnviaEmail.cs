using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Collections;
using Framework.Mail.Entidade;

namespace Framework.Mail.Processo
{
    public class EnviaEmail
    {
        SmtpClient smtpClient;

        public SmtpClient SmtpClient
        {
            get { return smtpClient; }
            set { smtpClient = value; }
        }

        public MailMessage CriarEmail(ConfiguraSMTP configuraSMTP, ConfiguraEmail configuraEmail)
        {
            Smtp smtp = new Smtp();
            MailMessage message = new MailMessage();
            SmtpClient = smtp.CriarSmtp(configuraSMTP);
            //Console.WriteLine(configuraEmail.De + " - " + configuraEmail.DeNome);
            //Console.WriteLine(configuraEmail.Para + " - " + configuraEmail.ParaNome);
            MailAddress enderecoDe = new MailAddress(configuraEmail.De, configuraEmail.DeNome);
            MailAddress enderecoPara = new MailAddress(configuraEmail.Para, configuraEmail.ParaNome);

            //Parametros
            message.Headers.Add(configuraEmail.De, configuraEmail.DeNome);
            message.From = enderecoDe;
            message.To.Add(enderecoPara);
            message.Priority = configuraEmail.Prioridade;
            message.Subject = configuraEmail.Assunto;
            message.IsBodyHtml = configuraEmail.EnableHTML;
            if (configuraEmail.EnableHTML)
                message.Body = configuraEmail.CorpoHTML;
            else
                message.Body = configuraEmail.Corpo;
            
            //Verfica se existe anexo
            if (configuraEmail.Anexo.Length > 0)
            {
                Attachment att = new Attachment(configuraEmail.Anexo);
                message.Attachments.Add(att);
            }
            
            return message;
        }
    }
}
