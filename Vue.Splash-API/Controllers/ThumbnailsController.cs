using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.User;
using Vue.Splash_API.Services.UserPhotos;

namespace Vue.Splash_API.Controllers;

[Authorize]
[ApiController]
public class ThumbnailsController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly IApplicationUserService _userService;
    private readonly IUserPhotosService _userPhotosService;

    public ThumbnailsController(IStorageService storageService,
        IApplicationUserService userService,
        IUserPhotosService userPhotosService)
    {
        _storageService = storageService;
        _userService = userService;
        _userPhotosService = userPhotosService;
    }

    [HttpGet("Photos/{photoId:int}/Thumbnail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DownloadThumbnail(int photoId)
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name!);
        var thumbnail = await _userPhotosService.GetUserPhotoThumbnail(photoId, usr!.Id);
        return thumbnail == null
            ? NotFound()
            : File(await _storageService.GetStream(thumbnail), "image/*");
    }
}