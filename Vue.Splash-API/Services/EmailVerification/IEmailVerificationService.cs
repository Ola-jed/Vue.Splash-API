using System.Threading.Tasks;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.EmailVerification;

public interface IEmailVerificationService
{
    Task<bool> IsEmailConfirmed(string identifier);
    Task<string> GenerateEmailVerificationToken(ApplicationUser user);
    Task<bool> VerifyEmail(ApplicationUser user, string token);
}