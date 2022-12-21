using System.Threading.Tasks;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Extensions;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.Photos;
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.Thumbnail;
using Vue.Splash_API.Services.UserPhotos;

namespace Vue.Splash_API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PhotosController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly IPhotosService _photosService;
    private readonly IThumbnailService _thumbnailService;
    private readonly IUserPhotosService _userPhotosService;

    public PhotosController(IStorageService storageService,
        IPhotosService photosService,
        IThumbnailService thumbnailService,
        IUserPhotosService userPhotosService)
    {
        _storageService = storageService;
        _photosService = photosService;
        _thumbnailService = thumbnailService;
        _userPhotosService = userPhotosService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<UrlPage<PhotoReadDto>> GetAll([FromQuery] int pageSize = 20,
        [FromQuery] int pageNumber = 1)
    {
        var urlPaginationParameter = new UrlPaginationParameter(pageSize, pageNumber, this.GetCleanUrl());
        return await _photosService.Find(p => p.ApplicationUserId == this.GetUserId(), urlPaginationParameter);
    }

    [HttpGet("{id:int}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PhotoReadDto>> Get(int id)
    {
        var photo = await _userPhotosService.GetUserPhoto(id, this.GetUserId());
        return photo == null ? NotFound() : photo;
    }

    [HttpGet("{id:int}/Download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DownloadPhoto(int id)
    {
        var photoPath = await _userPhotosService.GetUserPhotoPath(id, this.GetUserId());
        return photoPath == null ? NotFound() : File(await _storageService.GetStream(photoPath), "image/*");
    }

    [HttpGet("Search")]
    public async Task<UrlPage<PhotoReadDto>> Search([FromQuery] PhotoSearchDto searchDto)
    {
        var urlPaginationParameter = new UrlPaginationParameter(searchDto.PageSize, searchDto.PageNumber,
            this.GetCleanUrl() + $"?Search={searchDto.Search}");
        return await _photosService.Find(
            p => EF.Functions.Like(p.Label, $"%{searchDto.Search}%") && p.ApplicationUserId == this.GetUserId(),
            urlPaginationParameter);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post([FromForm] PhotoCreateDto photoCreateDto)
    {
        var path = await _storageService.Save(photoCreateDto.Photo);
        var thumbnail = await _thumbnailService.ReduceQuality(photoCreateDto.Photo);
        var thumbnailFile = await _storageService.Save(thumbnail);
        var photo = new Photo
        {
            Label = photoCreateDto.Label,
            Description = photoCreateDto.Description,
            ApplicationUserId = this.GetUserId(),
            Path = path,
            Thumbnail = thumbnailFile
        };
        var photoReadDto = await _photosService.CreatePhoto(photo);
        return CreatedAtRoute(nameof(Get), new { photoReadDto.Id }, photoReadDto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdatePhoto(int id, PhotoUpdateDto photoUpdateDto)
    {
        if (!await _userPhotosService.PhotoExistsAndOwnedByUser(id, this.GetUserId()))
        {
            return NotFound();
        }

        await _photosService.UpdatePhoto(id, photoUpdateDto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePhoto(int id)
    {
        if (!await _userPhotosService.PhotoExistsAndOwnedByUser(id, this.GetUserId()))
        {
            return NotFound();
        }

        await _photosService.DeletePhoto(id);
        return NoContent();
    }
}