using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record PhotoUpdateDto
{
    [Required] [MaxLength(100)] public string Label { get; init; }
    [Required] public string Description { get; init; }
}