using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record PasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }
    }
}