using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.Auth;

public interface IAuthService
{
    Task<ApplicationUser?> RegisterUser(RegisterDto registerDto);
    Task<bool> ValidateUserCredentials(LoginDto loginDto);
    Task<JwtSecurityToken?> GenerateJwt(LoginDto loginDto);
}