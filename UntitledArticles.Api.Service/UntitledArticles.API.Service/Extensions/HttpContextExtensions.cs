namespace UntitledArticles.API.Service.Extensions;

using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

internal static class HttpContextExtensions
{
    internal static string GetUserId(this HttpContext httpContext)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;
        if (identity is null)
        {
            return String.Empty;
        }

        string userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (String.IsNullOrEmpty(userId))
        {
            throw new AuthenticationException("NameIdentifier wasn't provided in JWT token!");
        }

        return userId;
    }
}
