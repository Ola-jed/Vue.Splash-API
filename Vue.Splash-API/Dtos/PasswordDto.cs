using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record PasswordDto([Required] string Password);