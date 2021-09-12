using System;
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
        public ActionResult SendPasswordResetMail(ForgotPasswordDto forgotPasswordDto)
        {
            throw new NotImplementedException();
        }

        [HttpPost("reset")]
        public ActionResult ResetUserPassword(PasswordResetDto passwordResetDto)
        {
            throw new NotImplementedException();
        }
    }
}