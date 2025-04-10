using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using BookShop.Auth.ServicesAuth.Interfaces;

namespace BookShop.Auth.ServicesAuth.Classes
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        // Внедрение конфигурации через IOptions
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.EmailAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = int.Parse(_emailSettings.SmtpPort),
                Credentials = new NetworkCredential(_emailSettings.EmailAddress, _emailSettings.EmailPassword),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}