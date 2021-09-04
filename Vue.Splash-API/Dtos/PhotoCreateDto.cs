using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Vue.Splash_API.Dtos.Validation;

namespace Vue.Splash_API.Dtos
{
    public record PhotoCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Label { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxFileSize(5*1024*1024)]
        [AllowedExtensions(".jpg,.jpeg,.png,.webp")]
        public IFormFile Photo { get; set; }
    }
}