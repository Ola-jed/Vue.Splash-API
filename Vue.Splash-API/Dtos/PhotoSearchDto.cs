using System.ComponentModel.DataAnnotations;

namespace Vue.Splash_API.Dtos;

public record PhotoSearchDto([Required] string Search, int PageNumber = 1, int PageSize = 10);