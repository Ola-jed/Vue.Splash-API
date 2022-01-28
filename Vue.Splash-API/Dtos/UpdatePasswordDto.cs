using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record UpdatePasswordDto([Required] string CurrentPassword, [Required] string NewPassword);