using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Vue.Splash_API.Profiles;

namespace Vue.Splash_API.Extensions;

public static class QueryableExtensions
{
    private static readonly MapperConfiguration Config = new(u =>
    {
        u.AddProfile<PhotoProfile>();
        u.AddProfile<UserProfile>();
    });

    public static IQueryable<T> To<T>(this IQueryable self)
    {
        return self.ProjectTo<T>(Config);
    }
}