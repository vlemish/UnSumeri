namespace Authorization.Application.Services;

using Abstractions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.Enumerations;
using Models.Results;
using Models.Validation;

public class JwtRegisterService : IRegisterService<RegistrationResult>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtRegisterService(UserManager<ApplicationUser> userManager)
    {
        this._userManager = userManager;
    }

    public async Task<RegistrationResult> RegisterUser(RegisterDto registerDto)
    {
        if (registerDto is null)
        {
            throw new ArgumentNullException(nameof(registerDto));
        }

        AuthenticationValidationResult validationResult = await ValidateUser(registerDto);
        if (!validationResult.Valid)
        {
            var res = validationResult.ErrorResults.ToList();
            return new(validationResult.Valid, validationResult.ErrorResults.ToList());
        }

        ApplicationUser user =
            await this._userManager.Users.FirstOrDefaultAsync(user =>
                user.UserName == registerDto.Username || user.Email == registerDto.Email);
        if (user is not null)
        {
            return this.ReportConfilct();
        }

        IdentityResult result =
            await this._userManager.CreateAsync(this.CreateAuthenticationUser(registerDto), registerDto.Password);
        return new RegistrationResult(result.Succeeded);
    }

    private async Task<AuthenticationValidationResult> ValidateUser(RegisterDto registerDto)
    {
        IdentityResult identityResult = new();
        foreach (var validator in this._userManager.PasswordValidators)
        {
            identityResult = await validator.ValidateAsync(this._userManager,
                this.CreateAuthenticationUser(registerDto),
                registerDto.Password);
        }

        return identityResult.Succeeded
            ? new()
            : new(identityResult.Errors.Select(error =>
                new ErrorResult<AuthenticationErrorReason>(AuthenticationErrorReason.InvalidPassword,
                    error.Description)).ToList());
    }

    private ApplicationUser CreateAuthenticationUser(RegisterDto registerDto) =>
        new()
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
        };

    private RegistrationResult ReportConfilct() =>
        new(false,
            new List<ErrorResult<AuthenticationErrorReason>>()
            {
                new(AuthenticationErrorReason.UserAlreadyExists,
                    "The user with this username and email already exists!")
            });
}
