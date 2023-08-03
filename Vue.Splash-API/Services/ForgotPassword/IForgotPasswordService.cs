using System.Threading.Tasks;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.ForgotPassword;

public interface IForgotPasswordService
{
    Task<string> CreateResetPasswordToken(ApplicationUser user);
    Task<bool> ResetUserPassword(string token, string newPassword);
}