using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record LoginDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}