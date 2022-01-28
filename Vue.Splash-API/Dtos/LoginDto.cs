using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record LoginDto([Required] string Identifier, [Required] string Password);