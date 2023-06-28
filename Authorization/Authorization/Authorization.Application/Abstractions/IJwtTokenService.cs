namespace Authorization.Application.Abstractions;

using Infrastructure.Models;
using Models;
using Models.Dtos;
using Models.Results;
using Models.Validation;

public interface IJwtTokenService
{
    JwtTokenDto Generate(ApplicationUser applicationUser);

    JwtValidationResult ValidateJwtToken(string token);
}
