namespace Authorization.Application.Services;

using Abstractions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.Enumerations;
using Models.Results;

public class JwtLoginService : ILoginService<LoginResult>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService jwtTokenService;
    private readonly IRefreshTokenService<ApplicationUser> _refreshTokenService;

    public JwtLoginService(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService,
        IRefreshTokenService<ApplicationUser> refreshTokenService)
    {
        this._userManager = userManager;
        this.jwtTokenService = jwtTokenService;
        this._refreshTokenService = refreshTokenService;
    }

    #region Implementation of ILoginService

    public async Task<LoginResult> LogIn(LoginDto loginDto)
    {
        ApplicationUser user =
            await this._userManager.Users.FirstOrDefaultAsync(user =>
                user.UserName == loginDto.Username || user.Email == loginDto.Email);
        if (user is null)
        {
            return this.ReportUserNotFound();
        }

        bool isPasswordValid = await this._userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
        {
            return this.ReportWrongPassword();
        }

        JwtTokenDto jwtTokenDto = this.jwtTokenService.Generate(user);
        string refreshToken = await this._refreshTokenService.Generate(jwtTokenDto.JwtId, user);
        return this.ReportSuccess(new AuthenticationJwtToken(jwtTokenDto.Token, refreshToken, jwtTokenDto.ExpiresAt));
    }

    public Task LogOut() => throw new NotImplementedException();

    #endregion

    #region Private methods

    private LoginResult ReportUserNotFound() =>
        new(false, null,
            new List<ErrorResult<AuthenticationErrorReason>>()
            {
                new(AuthenticationErrorReason.UserNotExist, "User doesn't exist!")
            });

    private LoginResult ReportWrongPassword() =>
        new(false, null,
            new List<ErrorResult<AuthenticationErrorReason>>()
            {
                new(AuthenticationErrorReason.WrongPassword, "Password was wrong!")
            });

    private LoginResult ReportSuccess(AuthenticationJwtToken authenticationJwtToken) =>
        new(true, authenticationJwtToken, null);

    #endregion
}
