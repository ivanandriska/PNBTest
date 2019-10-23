using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Collections;
using CaseBusiness.Framework.Mail.Entidade;
using System.Threading;

namespace CaseBusiness.Framework.Mail.Processo
{
    public class Email
    {
        SmtpClient smtpClient;

        public Email()
        {
            smtpClient = new SmtpClient(Entidade.Smtp.ProtocoloSmtp, Entidade.Smtp.Porta);
            smtpClient.EnableSsl = Entidade.Smtp.Ssl;

            smtpClient.Credentials = new NetworkCredential(Entidade.Smtp.Login, Entidade.Smtp.Senha);
        }

        public void Enviar(CaseBusiness.Framework.Mail.Entidade.Email configuraEmail)
        {
            try
            {
                MailMessage message = new MailMessage();
                MailAddress enderecoDe = new MailAddress(Entidade.Smtp.EmailDe, Entidade.Smtp.Nome);
                MailAddress enderecoPara = new MailAddress(configuraEmail.Para, configuraEmail.ParaNome);

                //Parametros
                message.Headers.Add(Entidade.Smtp.EmailDe, Entidade.Smtp.Nome);
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

                smtpClient.Send(message);
                Thread.Sleep(3000);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
