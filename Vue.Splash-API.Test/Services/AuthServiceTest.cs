using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Vue.Splash_API.Data;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.Auth;
using Vue.Splash_API.Services.User;
using Vue.Splash_API.Test.Setup;
using Xunit;

namespace Vue.Splash_API.Test.Services;

public class AuthServiceTest
{
    [Fact]
    public async Task TestRegisterExistingUser()
    {
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            CreatedAt = DateTime.Now,
            Password = BCrypt.Net.BCrypt.HashPassword("password")
        };
        var context = SplashContextBuilder.Build();
        context.ApplicationUsers.Add(user);
        context.SaveChanges();
        var service = GetService(context);
        var registerDto = new RegisterDto("John Doe", "johndoe@splash.com", "password");
        var result = await service.RegisterUser(registerDto);
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task TestRegisterNonExistingUser()
    {
        var service = GetService();
        var registerDto = new RegisterDto("John Doe", "johndoe@splash.com", "password");
        var result = await service.RegisterUser(registerDto);
        result.Should().NotBeNull();
        result.UserName.Should().Be("John Doe");
        result.Email.Should().Be("johndoe@splash.com");
    }

    [Fact]
    public async Task TestValidateCorrectUserNameAndPassword()
    {
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            CreatedAt = DateTime.Now,
            Password = BCrypt.Net.BCrypt.HashPassword("password")
        };
        var context = SplashContextBuilder.Build();
        context.ApplicationUsers.Add(user);
        context.SaveChanges();
        var service = GetService(context);
        var loginDto = new LoginDto(user.UserName, "password");
        var result = await service.ValidateUserCredentials(loginDto);
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task TestValidateCorrectEmailAndPassword()
    {
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            CreatedAt = DateTime.Now,
            Password = BCrypt.Net.BCrypt.HashPassword("password")
        };
        var context = SplashContextBuilder.Build();
        context.ApplicationUsers.Add(user);
        context.SaveChanges();
        var service = GetService(context);
        var loginDto = new LoginDto(user.Email, "password");
        var result = await service.ValidateUserCredentials(loginDto);
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task TestValidateIncorrectPassword()
    {
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            CreatedAt = DateTime.Now,
            Password = BCrypt.Net.BCrypt.HashPassword("password")
        };
        var context = SplashContextBuilder.Build();
        context.ApplicationUsers.Add(user);
        context.SaveChanges();
        var service = GetService(context);
        var loginDto = new LoginDto(user.UserName, "invalid_password");
        var result = await service.ValidateUserCredentials(loginDto);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task TestGenerateJwtWithValidCredentials()
    {
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            CreatedAt = DateTime.Now,
            Password = BCrypt.Net.BCrypt.HashPassword("password")
        };
        var context = SplashContextBuilder.Build();
        context.ApplicationUsers.Add(user);
        context.SaveChanges();
        var service = GetService(context);
        var loginDto = new LoginDto(user.UserName, "password");
        var result = await service.GenerateJwt(loginDto);
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task TestGenerateJwtWithInvalidCredentials()
    {
        var user = new ApplicationUser
        {
            Id = 1,
            UserName = "John Doe",
            Email = "johndoe@splash.com",
            CreatedAt = DateTime.Now,
            Password = BCrypt.Net.BCrypt.HashPassword("password")
        };
        var context = SplashContextBuilder.Build();
        context.ApplicationUsers.Add(user);
        context.SaveChanges();
        var service = GetService(context);
        var loginDto = new LoginDto(user.UserName, "invalid_password");
        var result = await service.GenerateJwt(loginDto);
        result.Should().BeNull();
    }

    private static IAuthService GetService(SplashContext? ctx = null)
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "JWT:Secret", "702357898AD899DA" },
            { "JWT:ValidIssuer", "Vue.Splash" },
            { "JWT:ValidAudience", "Vue.Splash" }
        };
        var context = ctx ?? SplashContextBuilder.Build();
        var appUserService = new ApplicationUserService(context);
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        return new AuthService(configuration,appUserService,context);
    }
}