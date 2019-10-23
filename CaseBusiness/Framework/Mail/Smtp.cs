using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace Framework.Mail
{
    public class Smtp
    {
        public SmtpClient CriarSmtp(ConfiguraSMTP configuraSMTP)
        {
            SmtpClient client = new SmtpClient(configuraSMTP.Smtp, configuraSMTP.Porta);
            client.EnableSsl = configuraSMTP.EnableSsl;
            
            client.Credentials = new NetworkCredential(configuraSMTP.Usuario, configuraSMTP.Senha);

            return client;
        }

        public void Enviar(SmtpClient smtpClient, MailMessage mailMessage)
        {
            try
            {
                smtpClient.Send(mailMessage);
                Thread.Sleep(3000);
                Console.WriteLine("Enviado com Sucesso");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
