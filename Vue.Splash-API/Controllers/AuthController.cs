using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Services.Auth;

namespace Vue.Splash_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var (result, user) = await _authService.RegisterUser(registerDto);
        if (result == null || user == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Status = "Error", Message = "User already exists!" });
        }

        return !result.Succeeded
            ? StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    result.Errors
                })
            : NoContent();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TokenDto>> Login(LoginDto model)
    {
        var token = await _authService.GenerateJwt(model);
        if (token == null)
        {
            return BadRequest();
        }

        if (!await _authService.IsEmailConfirmed(model.Identifier))
        {
            return BadRequest(new
            {
                Message = "Email not verified"
            });
        }

        return new TokenDto(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }
}