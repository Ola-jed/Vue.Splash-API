using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.Mail.Mailable;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers;

[ApiController]
[Route("api/password")]
public class ForgotPasswordController : ControllerBase
{
    private readonly IApplicationUserService _userService;
    private readonly IMailService _mailService;
    private readonly string _frontUrl;

    public ForgotPasswordController(IApplicationUserService userService,
        IMailService mailService,
        IConfiguration configuration)
    {
        _userService = userService;
        _mailService = mailService;
        _frontUrl = configuration["FrontUrl"];
    }

    [HttpPost("forgot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SendPasswordResetMail(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userService.FindUserByEmail(forgotPasswordDto.Email);
        if (user == null)
        {
            return NotFound();
        }

        var token = await _userService.GenerateResetPasswordToken(user);
        await _mailService.SendEmailAsync(new ForgotPasswordMail(user.UserName, user.Email, token, _frontUrl));
        return Ok();
    }

    [HttpPost("reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ResetUserPassword(PasswordResetDto passwordResetDto)
    {
        var user = await _userService.FindUserByEmail(passwordResetDto.Email);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userService.ResetUserPassword(user, passwordResetDto.Token, passwordResetDto.Password);
        return result.Succeeded
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
    }
}