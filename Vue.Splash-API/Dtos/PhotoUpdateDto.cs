using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record PhotoUpdateDto([Required] [MaxLength(100)] string Label, [Required] string Description);