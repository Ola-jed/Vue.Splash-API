using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record LoginDto
{
    [Required(ErrorMessage = "The identifier is required")]
    public string Identifier { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; init; }
}