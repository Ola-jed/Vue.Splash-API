using MimeKit;

namespace Vue.Splash_API.Services.Mail.Mailable
{
    public interface IMailable
    {
        MimeMessage Build();
    }
}