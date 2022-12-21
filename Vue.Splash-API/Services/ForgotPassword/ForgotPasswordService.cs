using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vue.Splash_API.Data;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.ForgotPassword;

public class ForgotPasswordService: IForgotPasswordService
{
    private readonly SplashContext _context;
    private readonly int _tokenLifetimeInMinutes;

    public ForgotPasswordService(SplashContext context,IConfiguration configuration)
    {
        _context = context;
        _tokenLifetimeInMinutes = int.Parse(configuration.GetSection("Code:Lifetime").Value!);
    }
    
    public async Task<string> CreateResetPasswordToken(ApplicationUser user)
    {
        var token = Guid.NewGuid().ToString().Replace("-", null);
        var passwordReset = new PasswordReset
        {
            ApplicationUserId = user.Id,
            Token = token
        };
        _context.PasswordResets.Add(passwordReset);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<bool> ResetUserPassword(ApplicationUser user, string token, string newPassword)
    {
        var passwordReset = await _context.PasswordResets
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id && x.Token == token);
        if (passwordReset == null || passwordReset.CreatedAt < DateTime.Now.AddMinutes(-_tokenLifetimeInMinutes))
        {
            return false;
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _context.PasswordResets.Remove(passwordReset);
        _context.ApplicationUsers.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }
}