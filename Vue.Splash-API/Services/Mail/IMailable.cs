using MimeKit;

namespace Vue.Splash_API.Services.Mail
{
    public interface IMailable
    {
        MimeMessage Build();
    }
}