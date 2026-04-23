using System;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;

namespace BibliotecaApp.Services
{
    public static class EmailService
    {
        private static readonly string remetente = "biblioteca.gastaovalle@gmail.com";
        private static readonly string senha = "kbvv piip qtrs wpmm"; // Senha de app do Gmail (específica para aplicações)

        public static void Enviar(string destinatario, string assunto, string mensagem, string caminhoAnexo = null)
        {
            try
            {
                // Validar inputs
                if (string.IsNullOrWhiteSpace(destinatario))
                {
                    LogarErro("Destinatário vazio");
                    return;
                }

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(remetente, senha);
                    smtp.EnableSsl = true;
                    smtp.Timeout = 30000; // 30 segundos timeout

                    using (var mail = new MailMessage(remetente, destinatario, assunto, mensagem))
                    {
                        mail.IsBodyHtml = true;

                        if (!string.IsNullOrEmpty(caminhoAnexo) && File.Exists(caminhoAnexo))
                            mail.Attachments.Add(new Attachment(caminhoAnexo));

                        smtp.Send(mail);
                        LogarSucesso($"Email enviado para {destinatario}");
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                LogarErro($"Erro SMTP ao enviar email para {destinatario}: {smtpEx.Message}");
                // Não lançar exception - apenas registrar
            }
            catch (Exception ex)
            {
                LogarErro($"Erro ao enviar email para {destinatario}: {ex.GetType().Name} - {ex.Message}");
            }
        }

        private static void LogarSucesso(string mensagem)
        {
            Trace.WriteLine($"[EmailService] ✓ {mensagem} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            try
            {
                var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(logDir);
                File.AppendAllText(
                    Path.Combine(logDir, "email_service.log"),
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ✓ {mensagem}\n");
            }
            catch { }
        }

        private static void LogarErro(string mensagem)
        {
            Trace.WriteLine($"[EmailService] ✗ {mensagem} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            try
            {
                var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(logDir);
                File.AppendAllText(
                    Path.Combine(logDir, "email_service.log"),
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ✗ {mensagem}\n");
            }
            catch { }
        }
    }
}