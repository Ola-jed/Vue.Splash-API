using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Vue.Splash_API.Dtos.Validation;

namespace Vue.Splash_API.Dtos;

public record PhotoCreateDto([Required] [MaxLength(100)] string Label, [Required] string Description,
    [Required] [MaxFileSize(5 * 1024 * 1024)] [AllowedExtensions(".jpg,.jpeg,.png,.bmp")]
    IFormFile Photo);