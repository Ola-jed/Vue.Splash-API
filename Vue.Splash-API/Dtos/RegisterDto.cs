using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record RegisterDto([Required] string Username, [EmailAddress] [Required] string Email,
    [Required] [MinLength(6)] [MaxLength(50)] string Password);