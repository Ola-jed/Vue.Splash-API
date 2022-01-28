using System;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Data;

namespace Vue.Splash_API.Test.Setup;

public static class SplashContextBuilder
{
    public static SplashContext Build()
    {
        var context = new SplashContext(new DbContextOptionsBuilder<SplashContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        return context;
    }
}