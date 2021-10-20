using System.Collections.Generic;
using System.Linq;

namespace Vue.Splash_API.Services.Paginator
{
    public record Paginator
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int Offset => (PageNumber - 1) * PageSize;
    }

    public static class PaginatorUtils
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> self, Paginator paginator)
        {
            return self.Skip(paginator.Offset).Take(paginator.PageSize);
        }
    }
}