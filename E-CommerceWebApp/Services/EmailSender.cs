using E_CommerceWebApp.Models.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace E_CommerceWebApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailServerSettings _emailServerSettings;

        public EmailSender(IOptions<EmailServerSettings> emailServerSettings)
        {
            _emailServerSettings = emailServerSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient
            {
                Port = _emailServerSettings.Port,
                Host = _emailServerSettings.Host,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailServerSettings.Username, _emailServerSettings.Password)
            };

            MailMessage message = new MailMessage
            {
                From = new MailAddress(_emailServerSettings.SenderEmail, _emailServerSettings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(email));


            return client.SendMailAsync(message);
        }
    }
}
