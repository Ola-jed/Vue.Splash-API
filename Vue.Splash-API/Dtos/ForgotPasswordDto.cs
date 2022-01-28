using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record ForgotPasswordDto([Required] [EmailAddress] string Email);