using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record EmailVerificationDto([Required] string Token);