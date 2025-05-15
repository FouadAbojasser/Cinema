using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cinema.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettingsGmail:SmtpServer"], int.Parse(_configuration["EmailSettingsGmail:Port"]))
            {
                Credentials = new NetworkCredential(
                    _configuration["EmailSettingsGmail:SenderEmail"],
                    _configuration["EmailSettingsGmail:SenderPassword"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(
                from: _configuration["EmailSettingsGmail:SenderEmail"],
                to: email,
                subject,
                htmlMessage
            )
            {
                IsBodyHtml = true
            };

            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}
