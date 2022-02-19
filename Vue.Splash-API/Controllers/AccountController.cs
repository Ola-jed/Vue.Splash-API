using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Services.Auth;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IApplicationUserService _userService;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public AccountController(IApplicationUserService userService,
        IMapper mapper,
        IAuthService authService)
    {
        _userService = userService;
        _mapper = mapper;
        _authService = authService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Get()
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!);
        return Ok(_mapper.Map<UserReadDto>(usr));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Put(AccountUpdateDto accountUpdateDto)
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!);
        if (usr == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var loginDto = new LoginDto(usr.UserName, accountUpdateDto.Password);
        var validCredentials = await _authService.ValidateUserCredentials(loginDto);
        if (!validCredentials)
        {
            return Unauthorized();
        }

        var res = await _userService.UpdateUser(usr, accountUpdateDto);
        return res.Succeeded
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, new
            {
                res.Errors
            });
    }

    [HttpPut("Password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdatePassword(UpdatePasswordDto passwordDto)
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!);
        if (usr == null || !await _userService.CheckPassword(usr.UserName, passwordDto.CurrentPassword))
        {
            return Unauthorized();
        }

        var res = await _userService.UpdatePassword(usr, passwordDto.CurrentPassword, passwordDto.NewPassword);
        return res.Succeeded
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, new
            {
                res.Errors
            });
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(PasswordDto passwordDto)
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!);
        if (usr == null)
        {
            return Unauthorized();
        }

        var loginDto = new LoginDto(usr.UserName!, passwordDto.Password);
        var validCredentials = await _authService.ValidateUserCredentials(loginDto);
        if (!validCredentials)
        {
            return Unauthorized();
        }

        var res = await _userService.DeleteUser(usr);
        return res.Succeeded
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError, new
            {
                res.Errors
            });
    }
}