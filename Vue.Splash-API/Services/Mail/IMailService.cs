using System.Threading.Tasks;
using Vue.Splash_API.Services.Mail.Mailable;

namespace Vue.Splash_API.Services.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(IMailable mailable);
    }
}