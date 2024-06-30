using Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmailAsync(string receiver, string subject, string message)
        {
            var smtpClient = new SmtpClient(_emailSettings.SmtpSettings.Server)
            {
                Port = _emailSettings.SmtpSettings.Port,
                Credentials = new NetworkCredential(
                    _emailSettings.SmtpSettings.Username,
                    _emailSettings.SmtpSettings.Password),
                EnableSsl = true
            };

            var email = new MailMessage
            {
                From = new MailAddress(
                    _emailSettings.SenderEmail,
                    _emailSettings.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            email.To.Add(receiver);

            await smtpClient.SendMailAsync(email);
        }
    }
}