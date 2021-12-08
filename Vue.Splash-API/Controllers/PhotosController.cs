using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Data.Repositories;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.Paginator;
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.Thumbnail;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PhotosController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly IApplicationUserService _userService;
    private readonly IPhotoRepository _photoRepository;
    private readonly IThumbnailService _thumbnailService;
    private readonly IMapper _mapper;

    public PhotosController(IStorageService storageService,
        IMapper mapper,
        IApplicationUserService userService,
        IPhotoRepository photoRepository,
        IThumbnailService thumbnailService)
    {
        _storageService = storageService;
        _mapper = mapper;
        _userService = userService;
        _photoRepository = photoRepository;
        _thumbnailService = thumbnailService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<PhotoReadDto>> GetAll([FromQuery] int pageSize = 20,
        [FromQuery] int pageNumber = 1)
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
        var paginator = new Paginator
        {
            PageSize = pageSize,
            PageNumber = pageNumber
        };
        var photos = _photoRepository.Find(p => p.ApplicationUserId == usr.Id)
            .Paginate(paginator);
        return _mapper.Map<IEnumerable<PhotoReadDto>>(photos);
    }

    [HttpGet("{id:int}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(int id)
    {
        var photo = await _photoRepository.GetPhoto(id);
        if (photo == null)
        {
            return NotFound();
        }

        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
        return usr.Id != photo.ApplicationUserId ? Forbid() : Ok(_mapper.Map<PhotoReadDto>(photo));
    }

    [HttpGet("{id:int}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DownloadPhoto(int id)
    {
        var photo = await _photoRepository.GetPhoto(id);
        if (photo == null)
        {
            return NotFound();
        }

        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
        return photo.ApplicationUserId != usr.Id
            ? Forbid()
            : File(await _storageService.GetStream(photo.Path), "image/*");
    }

    [HttpGet("search")]
    public async Task<IEnumerable<PhotoReadDto>> Search([FromQuery] PhotoSearchDto searchDto)
    {
        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
        var predicate = new Func<Photo, bool>(photo =>
            photo.Label.Contains(searchDto.Search,StringComparison.OrdinalIgnoreCase) && photo.ApplicationUserId == usr.Id);
        return _mapper.Map<IEnumerable<PhotoReadDto>>(_photoRepository.Find(predicate));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post([FromForm] PhotoCreateDto photoCreateDto)
    {
        try
        {
            var path = await _storageService.Save(photoCreateDto.Photo);
            var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
            var photo = _mapper.Map<Photo>(photoCreateDto);
            photo.Path = path;
            photo.ApplicationUserId = usr.Id;
            var thumbnail = await _thumbnailService.ReduceQuality(photoCreateDto.Photo);
            var thumbnailFile = await _storageService.Save(thumbnail);
            photo.Thumbnail = thumbnailFile;
            await _photoRepository.CreatePhoto(photo);
            await _photoRepository.SaveChanges();
            return CreatedAtRoute(nameof(Get), new { photo.Id }, _mapper.Map<PhotoReadDto>(photo));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdatePhoto(int id, PhotoUpdateDto photoUpdateDto)
    {
        var photo = await _photoRepository.GetPhoto(id);
        if (photo == null)
        {
            return NotFound();
        }

        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
        if (usr.Id != photo.ApplicationUserId)
        {
            return Forbid();
        }

        _mapper.Map(photoUpdateDto, photo);
        await _photoRepository.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePhoto(int id)
    {
        var photo = await _photoRepository.GetPhoto(id);
        if (photo == null)
        {
            return NotFound();
        }

        var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
        if (usr.Id != photo.ApplicationUserId)
        {
            return Forbid();
        }

        await _storageService.Delete(photo.Thumbnail);
        await _storageService.Delete(photo.Path);
        _photoRepository.DeletePhoto(photo);
        return NoContent();
    }
}