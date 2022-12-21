using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Vue.Splash_API.Data;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.EmailVerification;
using Vue.Splash_API.Test.Setup;
using Xunit;

namespace Vue.Splash_API.Test.Services;

public class EmailVerificationTest
{
    [Fact]
    public async Task TestNonExistingUserDoesNotHaveConfirmedEmail()
    {
        var service = GetService();
        var result = await service.IsEmailConfirmed("John Doe");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task TestExistingUserWithoutEmailConfirmedDoesNotHaveConfirmedEmail()
    {
        var ctx = SplashContextBuilder.Build();
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            Password = "pwd"
        };
        ctx.ApplicationUsers.Add(user);
        await ctx.SaveChangesAsync();
        var service = GetService(ctx);
        var result = await service.IsEmailConfirmed("John Doe");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task TestEmailVerificationTokenIsGenerated()
    {
        var ctx = SplashContextBuilder.Build();
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            Password = "pwd"
        };
        ctx.ApplicationUsers.Add(user);
        await ctx.SaveChangesAsync();
        var service = GetService(ctx);
        var result = await service.GenerateEmailVerificationToken(user);
        result.Should().NotBeNullOrWhiteSpace();
        ctx.EmailVerifications.Count().Should().Be(1);
    }

    [Fact]
    public async Task TestVerifyEmailWithInvalidToken()
    {
        var ctx = SplashContextBuilder.Build();
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            Password = "pwd"
        };
        ctx.ApplicationUsers.Add(user);
        await ctx.SaveChangesAsync();
        var service = GetService(ctx);
        var result = await service.VerifyEmail(user, "token");
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task TestVerifyEmailWithValidToken()
    {
        var ctx = SplashContextBuilder.Build();
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            Password = "pwd"
        };
        ctx.ApplicationUsers.Add(user);
        await ctx.SaveChangesAsync();
        var service = GetService(ctx);
        var token = await service.GenerateEmailVerificationToken(user);
        var result = await service.VerifyEmail(user, token);
        result.Should().BeTrue();
    }
    
    private static IEmailVerificationService GetService(SplashContext? ctx = null)
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Code:Lifetime", "10" }
        };
        var context = ctx ?? SplashContextBuilder.Build();
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        return new EmailVerificationService(context,configuration);
    }
}