using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Vue.Splash_API.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(IMailable mailable)
        {
            var email = mailable.Build();
            email.Sender = MailboxAddress.Parse(_settings.MailUser);
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.MailUser, _settings.MailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}