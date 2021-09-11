using System.Threading.Tasks;

namespace Vue.Splash_API.Services.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(IMailable mailable);
    }
}