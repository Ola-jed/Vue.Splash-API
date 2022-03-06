using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Vue.Splash_API.Services.Auth;

namespace Vue.Splash_API.Extensions;

public static class ControllerBaseExtensions
{
    public static string GetCleanUrl<T>(this T controller) where T : ControllerBase
    {
        return controller.Request.GetDisplayUrl().Split('?')[0];
    }

    public static int GetUserId<T>(this T controller) where T: ControllerBase
    {
        return int.Parse(controller.User.FindFirst(CustomClaims.Id)?.Value!);
    }
}