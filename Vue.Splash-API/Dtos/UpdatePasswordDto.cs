using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record UpdatePasswordDto
    {
        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; init; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; init; }
    }
}