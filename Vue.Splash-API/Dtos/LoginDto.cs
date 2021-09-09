using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record LoginDto
    {
        [Required(ErrorMessage = "The identifier is required")]
        public string Identifier { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}