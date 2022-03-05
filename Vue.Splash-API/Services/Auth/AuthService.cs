using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vue.Splash_API.Data;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IApplicationUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly SplashContext _context;

    public AuthService(IConfiguration configuration,
        IApplicationUserService userService,
        SplashContext context)
    {
        _configuration = configuration;
        _userService = userService;
        _context = context;
    }

    public async Task<bool> ValidateUserCredentials(LoginDto loginDto)
    {
        var user = await _userService.FindUserByIdentifier(loginDto.Identifier);
        return user != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
    }

    public async Task<ApplicationUser?> RegisterUser(RegisterDto registerDto)
    {
        var similarUserExists = await _context.ApplicationUsers
            .AnyAsync(x => x.Email == registerDto.Email || x.UserName == registerDto.Username);
        if (similarUserExists)
        {
            return null;
        }

        var user = new ApplicationUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Username
        };
        return await _userService.CreateUser(user, registerDto.Password);
    }

    public async Task<JwtSecurityToken?> GenerateJwt(LoginDto loginDto)
    {
        if (!await ValidateUserCredentials(loginDto))
        {
            return null;
        }

        var user = await _userService.FindUserByIdentifier(loginDto.Identifier);
        if (user == null)
        {
            return null;
        }

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("Id", user.Id.ToString())
        };
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        return new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(8),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}