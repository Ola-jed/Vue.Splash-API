using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record PasswordResetDto
{
    [Required]
    public string Token { get; init; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; init; } = null!;
}