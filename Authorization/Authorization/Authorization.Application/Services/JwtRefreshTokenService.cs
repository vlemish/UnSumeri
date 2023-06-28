namespace Authorization.Application.Services;

using Abstractions;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.Enumerations;
using Models.Results;
using Models.Validation;

public class JwtRefreshTokenService : IRefreshTokenService<ApplicationUser>
{
    private readonly AuthDbContext _authDbContext;
    private readonly IJwtTokenService _jwtTokenService;

    public JwtRefreshTokenService(AuthDbContext authDbContext, IJwtTokenService tokenService)
    {
        this._authDbContext = authDbContext;
        this._jwtTokenService = tokenService;
    }

    public async Task<string> Generate(string jwtTokenId, ApplicationUser applicationUser)
    {
        if (applicationUser is null)
        {
            throw new ArgumentNullException(nameof(applicationUser));
        }

        RefreshToken refreshToken = new()
        {
            JwtId = jwtTokenId,
            Token = $"{Guid.NewGuid()}_{Guid.NewGuid()}",
            DateAdded = DateTime.UtcNow,
            DateExpire = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            UserId = applicationUser.Id,
        };
        await this._authDbContext.RefreshTokens.AddAsync(refreshToken);
        await this._authDbContext.SaveChangesAsync();
        return refreshToken.Token;
    }

    public async Task Revoke(string userId)
    {
        RefreshToken refreshToken =
            await this._authDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.User.Id == userId);
        refreshToken.IsRevoked = true;
        this._authDbContext.RefreshTokens.Update(refreshToken);
        await this._authDbContext.SaveChangesAsync();
    }

    public async Task<RefreshTokenResult> RefreshToken(AuthenticationJwtToken jwtToken)
    {
        JwtValidationResult validateJwtToken = _jwtTokenService.ValidateJwtToken(jwtToken.Token);
        if (validateJwtToken.Valid)
        {
            return new RefreshTokenResult(true, jwtToken);
        }

        switch (validateJwtToken.ErrorResults.First().Reason)
        {
            case JwtValidationErrorReason.TokenExpired:
            {
                return await RefreshExpiredToken(jwtToken);
            }
            default:
                return new RefreshTokenResult(false, null,
                    new List<ErrorResult<JwtValidationErrorReason>>()
                    {
                        new(JwtValidationErrorReason.TokenInvalid,
                            "Invalid JWT token was sent!")
                    });
        }
    }

    private async Task<RefreshTokenResult> RefreshExpiredToken(AuthenticationJwtToken jwtToken)
    {
        JwtTokenDto newToken = await RegenerateExpiredToken(jwtToken);
        if (newToken is null)
        {
            return new RefreshTokenResult(false, null,
                new List<ErrorResult<JwtValidationErrorReason>>()
                {
                    new(JwtValidationErrorReason.TokenExpired,
                        "Refresh Token has expired!")
                });
        }

        return new RefreshTokenResult(true,
            new AuthenticationJwtToken(newToken.Token, jwtToken.RefreshToken, newToken.ExpiresAt));
    }

    private async Task<JwtTokenDto> RegenerateExpiredToken(AuthenticationJwtToken expiredJwtToken)
    {
        Infrastructure.Models.RefreshToken refreshToken =
            await this.GetRefreshToken(expiredJwtToken.RefreshToken);
        if (refreshToken is null || refreshToken.DateExpire <= DateTime.UtcNow || refreshToken.IsRevoked)
        {
            return null;
        }

        JwtTokenDto jwtTokenDto = this._jwtTokenService.Generate(refreshToken.User);
        return jwtTokenDto;
    }

    private async Task<RefreshToken> GetRefreshToken(string token) =>
        await this._authDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token);

    private RefreshTokenResult ReportSuccess(AuthenticationJwtToken authenticationJwtToken) =>
        new(true, authenticationJwtToken);

    private RefreshTokenResult ReportRefreshTokenExpired() =>
        new(false, null,
            new List<ErrorResult<JwtValidationErrorReason>>()
            {
                new(JwtValidationErrorReason.TokenExpired,
                    "Refresh Token has expired!")
            });

    private RefreshTokenResult ReportInvalidJwtToken() =>
        new(false, null,
            new List<ErrorResult<JwtValidationErrorReason>>()
            {
                new(JwtValidationErrorReason.TokenInvalid,
                    "Invalid JWT token was sent!")
            });
}
