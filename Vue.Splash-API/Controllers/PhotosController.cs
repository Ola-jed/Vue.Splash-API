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
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController: ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IApplicationUserService _userService;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMapper _mapper;

        public PhotosController(IStorageService storageService,
            IMapper mapper,
            IApplicationUserService userService,
            IPhotoRepository photoRepository)
        {
            _storageService = storageService;
            _mapper = mapper;
            _userService = userService;
            _photoRepository = photoRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
            var photos = _photoRepository.Find(p => p.ApplicationUserId == usr.Id);
            return Ok(_mapper.Map<IEnumerable<PhotoReadDto>>(photos));
        }

        [HttpGet("{id:int}",Name = "Get")]
        public async Task<ActionResult> Get(int id)
        {
            var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
            var photo = await _photoRepository.GetPhoto(id);
            if (photo == null)
            {
                return NotFound();
            }
            if (usr.Id != photo.ApplicationUserId)
            {
                return Forbid();
            }

            return Ok(_mapper.Map<PhotoReadDto>(photo));
        }

        [HttpGet("{id:int}/download")]
        public async Task<ActionResult> DownloadPhoto(int id)
        {
            var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
            var photo = await _photoRepository.GetPhoto(id);
            if (usr.Id != photo.ApplicationUserId)
            {
                return Forbid();
            }

            return File(_storageService.GetStream(photo.Path),"image/*");
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm]PhotoCreateDto photoCreateDto)
        {
            try
            {
                var path = await _storageService.SaveImage(photoCreateDto.Photo);
                var usr = await _userService.FindUserByUserName(HttpContext.User.Identity?.Name);
                var photo = _mapper.Map<Photo>(photoCreateDto);
                photo.Path = path;
                photo.ApplicationUserId = usr.Id;
                await _photoRepository.CreatePhoto(photo);
                await _photoRepository.SaveChanges();
                return CreatedAtRoute(nameof(Get),new {photo.Id},_mapper.Map<PhotoReadDto>(photo));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e);
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeletePhoto(int id)
        {
            throw new NotImplementedException();
        }
    }
}