namespace Authorization.Application.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Abstractions;
using Authorization.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Dtos;
using Models.Enumerations;
using Models.Results;
using Models.Validation;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

public class JwtTokenService : IJwtTokenService
{
    private const int TimeToLiveMinutes = 5;

    private readonly IConfiguration _configuration;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtTokenService(IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
    {
        this._configuration = configuration;
        this._tokenValidationParameters = tokenValidationParameters;
    }

    public JwtTokenDto Generate(ApplicationUser applicationUser)
    {
        if (applicationUser is null)
        {
            throw new ArgumentNullException(nameof(applicationUser));
        }

        List<Claim> authClaims = this.GetApplicationUserClaims(applicationUser);
        SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(this._configuration["JWT:Secret"]));

        JwtSecurityToken jwtSecurityToken = new(
            issuer: this._configuration["JWT:Issuer"],
            audience: this._configuration["JWT:Audience"],
            expires: DateTime.UtcNow.AddMinutes(TimeToLiveMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

        string jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return new(jwtToken, jwtSecurityToken.ValidTo, jwtSecurityToken.Id);
    }

    public JwtValidationResult ValidateJwtToken(string token)
    {
        try
        {
            JwtSecurityTokenHandler tokenHandler = new();
            tokenHandler.ValidateToken(token, this._tokenValidationParameters, out var validatedToken);

            return new();
        }
        catch (SecurityTokenExpiredException)
        {
            return new(new List<ErrorResult<JwtValidationErrorReason>>()
            {
                new(JwtValidationErrorReason.TokenExpired,
                    "Token has already expired")
            });
        }
        catch (SecurityTokenValidationException ex)
        {
            return new JwtValidationResult(
                new List<ErrorResult<JwtValidationErrorReason>>()
                {
                    new(JwtValidationErrorReason.TokenInvalid,
                        "Token is invalid")
                }
            );
        }
    }

    private List<Claim> GetApplicationUserClaims(ApplicationUser applicationUser) =>
        new()
        {
            new Claim(ClaimTypes.Name, applicationUser.UserName),
            new Claim(ClaimTypes.NameIdentifier, applicationUser.Id),
            new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
            new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
}
