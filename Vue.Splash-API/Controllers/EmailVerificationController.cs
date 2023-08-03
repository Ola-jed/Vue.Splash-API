using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Services.EmailVerification;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.Mail.Mailable;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers;

[Route("api/Email")]
[ApiController]
public class EmailVerificationController : ControllerBase
{
    private readonly IApplicationUserService _userService;
    private readonly IMailService _mailService;
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly string _frontUrl;

    public EmailVerificationController(IApplicationUserService userService,
        IMailService mailService,
        IConfiguration configuration,
        IEmailVerificationService emailVerificationService)
    {
        _userService = userService;
        _mailService = mailService;
        _emailVerificationService = emailVerificationService;
        _frontUrl = configuration["FrontUrl"]!;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SendVerificationMail(EmailDto emailDto)
    {
        var user = await _userService.FindUserByEmail(emailDto.Email);
        if (user == null)
        {
            return NotFound();
        }

        var token = await _emailVerificationService.GenerateEmailVerificationToken(user);
        await _mailService.SendEmailAsync(new EmailVerificationMail(user.UserName, user.Email, token, _frontUrl));
        return NoContent();
    }

    [HttpPost("Verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDto),StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> VerifyEmail(EmailVerificationDto emailVerificationDto)
    {
        var result = await _emailVerificationService.VerifyEmail(emailVerificationDto.Token);
        return result ? Ok() : BadRequest(new ErrorDto("Email verification failed"));
    }
}