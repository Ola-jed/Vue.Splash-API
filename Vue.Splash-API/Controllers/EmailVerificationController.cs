using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IApplicationUserService _userService;
        private readonly IMailService _mailService;

        public EmailVerificationController(IApplicationUserService userService,
            IMailService mailService)
        {
            _userService = userService;
            _mailService = mailService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SendVerificationMail(EmailDto emailDto)
        {
            var user = await _userService.FindUserByEmail(emailDto.Email);
            var token = await _userService.GenerateEmailVerificationToken(user);
            await _mailService.SendEmailAsync(new EmailVerificationMail(user.UserName, user.Email, token));
            return NoContent();
        }

        [HttpPost("verify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> VerifyEmail(EmailVerificationDto emailVerificationDto)
        {
            var user = await _userService.FindUserByEmail(emailVerificationDto.Email);
            var result = await _userService.VerifyEmail(user, emailVerificationDto.Token);
            return result.Succeeded
                ? Ok()
                : BadRequest(new
                {
                    result.Errors
                });
        }
    }
}