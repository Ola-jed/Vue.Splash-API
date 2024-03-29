using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Services.ForgotPassword;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.Mail.Mailable;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers;

[ApiController]
[Route("api/Password")]
public class ForgotPasswordController : ControllerBase
{
    private readonly IForgotPasswordService _forgotPasswordService;
    private readonly string _frontUrl;
    private readonly IMailService _mailService;
    private readonly IApplicationUserService _userService;

    public ForgotPasswordController(IApplicationUserService userService,
        IForgotPasswordService forgotPasswordService,
        IMailService mailService,
        IConfiguration configuration)
    {
        _userService = userService;
        _forgotPasswordService = forgotPasswordService;
        _mailService = mailService;
        _frontUrl = configuration["FrontUrl"]!;
    }

    [HttpPost("Forgot")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SendPasswordResetMail(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _userService.FindUserByEmail(forgotPasswordDto.Email);
        if (user == null)
        {
            return NotFound();
        }

        var token = await _forgotPasswordService.CreateResetPasswordToken(user);
        await _mailService.SendEmailAsync(new ForgotPasswordMail(user.UserName, user.Email, token, _frontUrl));
        return NoContent();
    }

    [HttpPost("Reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResetUserPassword(PasswordResetDto passwordResetDto)
    {
        var result = await _forgotPasswordService.ResetUserPassword(passwordResetDto.Token, passwordResetDto.Password);
        return result ? NoContent() : BadRequest(new ErrorDto("Invalid or expired token"));
    }
}