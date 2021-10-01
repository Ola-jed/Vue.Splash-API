using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record EmailDto
    {
        [Required] [EmailAddress] public string Email { get; init; }
    }
}