using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos
{
    public record PhotoSearchDto
    {
        [Required] public string Search { get; init; }
    }
}