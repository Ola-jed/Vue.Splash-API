using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record RegisterDto([Required] string Username, [EmailAddress] [Required] string Email,
    [Required] string Password);