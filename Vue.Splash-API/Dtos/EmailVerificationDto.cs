using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record EmailVerificationDto([Required] [EmailAddress] string Email, [Required] string Token);