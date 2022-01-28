using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record PasswordResetDto
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string Token { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; init; } = string.Empty;
}