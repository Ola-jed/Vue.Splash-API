using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record EmailVerificationDto
    {
        [Required] [EmailAddress] public string Email { get; init; }
        [Required] public string Token { get; set; }
    }
}