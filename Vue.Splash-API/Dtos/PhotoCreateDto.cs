using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Vue.Splash_API.Dtos.Validation;

namespace Vue.Splash_API.Dtos
{
    public record PhotoCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Label { get; init; }
        [Required]
        public string Description { get; init; }
        [Required]
        [MaxFileSize(5*1024*1024)]
        [AllowedExtensions(".jpg,.jpeg,.png,.webp")]
        public IFormFile Photo { get; init; }
    }
}