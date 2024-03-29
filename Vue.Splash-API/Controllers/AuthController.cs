using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Services.Auth;
using Vue.Splash_API.Services.EmailVerification;

namespace Vue.Splash_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailVerificationService _emailVerificationService;
    
    public AuthController(IAuthService authService, IEmailVerificationService emailVerificationService)
    {
        _authService = authService;
        _emailVerificationService = emailVerificationService;
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = await _authService.RegisterUser(registerDto);
        if (user == null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorDto("User already exists"));
        }
        
        return NoContent();
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenDto>> Login(LoginDto model)
    {
        var token = await _authService.GenerateJwt(model);
        if (token == null)
        {
            return Unauthorized();
        }

        if (!await _emailVerificationService.IsEmailConfirmed(model.Identifier))
        {
            return StatusCode(StatusCodes.Status403Forbidden,new ErrorDto("Email not verified"));
        }

        return new TokenDto(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }
}