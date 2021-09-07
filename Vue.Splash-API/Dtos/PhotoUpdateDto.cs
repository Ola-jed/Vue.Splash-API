using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record PhotoUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Label { get; set; }
        [Required]
        public string Description { get; set; }
    }
}