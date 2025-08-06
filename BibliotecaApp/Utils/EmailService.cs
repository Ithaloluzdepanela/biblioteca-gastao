using System;
using System.Net;
using System.Net.Mail;

namespace BibliotecaApp.Services
{
    public static class EmailService
    {
        private static readonly string remetente = "biblioteca.gastaovalle@gmail.com"; // coloque o seu
        private static readonly string senha = "azwo xvrr ljar sttj"; // senha de app do Gmail

        
        public static void Enviar(string destinatario, string assunto, string mensagem)
        {
            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(remetente, senha);
                    smtp.EnableSsl = true;

                    var mail = new MailMessage(remetente, destinatario, assunto, mensagem)
                    {
                        IsBodyHtml = true
                    };

                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao enviar e-mail: " + ex.Message);
            }
        }
    }
}