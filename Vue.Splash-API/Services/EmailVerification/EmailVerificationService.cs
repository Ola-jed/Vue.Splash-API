using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vue.Splash_API.Data;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.EmailVerification;

public class EmailVerificationService : IEmailVerificationService
{
    private readonly SplashContext _context;
    private readonly int _tokenLifetimeInMinutes;

    public EmailVerificationService(SplashContext context,IConfiguration configuration)
    {
        _context = context;
        _tokenLifetimeInMinutes = int.Parse(configuration["Code.TokenLifetimeInMinutes"] ?? "30");
    }

    public async Task<bool> IsEmailConfirmed(string identifier)
    {
        return await _context.ApplicationUsers
            .AnyAsync(x => (x.Email == identifier || x.UserName == identifier) && x.EmailVerifiedAt != null);
    }

    public async Task<string> GenerateEmailVerificationToken(ApplicationUser user)
    {
        var token = Guid.NewGuid().ToString().Replace("-", null);
        var emailVerification = new Models.EmailVerification
        {
            ApplicationUserId = user.Id,
            Token = token
        };
        _context.EmailVerifications.Add(emailVerification);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<bool> VerifyEmail(ApplicationUser user, string token)
    {
        var emailVerification = await _context.EmailVerifications
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id && x.Token == token);
        if (emailVerification == null || emailVerification.CreatedAt < DateTime.Now.AddMinutes(-_tokenLifetimeInMinutes))
        {
            return false;
        }

        user.EmailVerifiedAt = DateTime.Now;
        _context.EmailVerifications.Remove(emailVerification);
        _context.ApplicationUsers.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }
}