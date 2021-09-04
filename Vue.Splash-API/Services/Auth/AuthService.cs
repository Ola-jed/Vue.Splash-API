using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Services.Auth
{
    public class AuthService: IAuthService
    {
        private readonly IApplicationUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, IApplicationUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<bool> ValidateUserCredentials(LoginDto loginDto)
        {
            var user = await _userService.FindUserByUserName(loginDto.Username);
            return user != null && await _userService.CheckPassword(loginDto.Username, loginDto.Password);
        }

        public async Task<(IdentityResult, ApplicationUser)> RegisterUser(RegisterDto registerDto)
        {
            var userExists = await _userService.FindUserByUserName(registerDto.Username);
            if (userExists != null)
            {
                return (null, null);
            }
            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Username,
                RegisterDate = DateTime.Now
            };
            var result = await _userService.CreateUser(user, registerDto.Password);
            return (result, user);
        }

        public async Task<JwtSecurityToken> GenerateJwt(LoginDto loginDto)
        {
            if (! await ValidateUserCredentials(loginDto))
            {
                return null;
            }
            var user = await _userService.FindUserByUserName(loginDto.Username);
            var userRoles = await _userService.GetUserRoles(user);
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience:_configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}