namespace Authorization.Application.Abstractions;

using Microsoft.AspNetCore.Identity;
using Models;
using Models.Dtos;
using Models.Results;

public interface IRefreshTokenService<T> where T: IdentityUser
{
    Task<string> Generate(string jwtTokenId, T applicationUser);

    Task<RefreshTokenResult> RefreshToken(AuthenticationJwtToken jwtToken);
}
