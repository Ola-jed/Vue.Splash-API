using System.Threading.Tasks;
using MimeKit;

namespace Vue.Splash_API.Services.Mail.Mailable
{
    public interface IMailable
    {
        Task<MimeMessage> Build();
        Task<string> GetHtmlBody();
        Task<string> GetPlainTextBody();
    }
}