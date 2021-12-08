using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record AccountUpdateDto
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; init; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; init; }
}