using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Data.Repositories;
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers;

[Authorize]
[ApiController]
public class ThumbnailsController: ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly IApplicationUserService _userService;
    private readonly IPhotoRepository _photoRepository;

    public ThumbnailsController(IStorageService storageService,
        IApplicationUserService userService,
        IPhotoRepository photoRepository)
    {
        _storageService = storageService;
        _userService = userService;
        _photoRepository = photoRepository;
    }

    [HttpGet("Photos/{photoId:int}/Thumbnail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DownloadThumbnail(int photoId)
    {
        var photo = await _photoRepository.GetPhoto(photoId);
        if (photo == null)
        {
            return NotFound();
        }

        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
        return photo.ApplicationUserId != usr.Id
            ? Forbid()
            : File(await _storageService.GetStream(photo.Thumbnail), "image/*");
    }
}