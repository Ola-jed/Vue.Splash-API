using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers
{
    [ApiController]
    [Route("api/password")]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IApplicationUserService _userService;
        private readonly IMailService _mailService;

        public ForgotPasswordController(IApplicationUserService userService,
            IMailService mailService)
        {
            _userService = userService;
            _mailService = mailService;
        }

        [HttpPost("forgot")]
        public async Task<ActionResult> SendPasswordResetMail(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userService.FindUserByEmail(forgotPasswordDto.Email);
            if (user == null)
            {
                return NotFound();
            }

            var token = await _userService.GenerateResetPasswordToken(null);
            await _mailService.SendEmailAsync(new ForgotPasswordMail(user.UserName,user.Email,token));
            return Ok();
        }

        [HttpPost("reset")]
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
}