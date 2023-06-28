namespace Authorization.Controllers;

using Application.Abstractions;
using Application.Models;
using Application.Models.Dtos;
using Application.Models.Enumerations;
using Application.Models.Results;
using Contracts.Requests;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILoginService<LoginResult> _loginService;
    private readonly IRegisterService<RegistrationResult> _registerService;
    private readonly IRefreshTokenService<ApplicationUser> _refreshTokenService;

    public AuthenticationController(ILoginService<LoginResult> loginService,
        IRegisterService<RegistrationResult> registerService,
        IRefreshTokenService<ApplicationUser> refreshTokenService)
    {
        this._loginService = loginService;
        this._registerService = registerService;
        this._refreshTokenService = refreshTokenService;
    }

    [HttpPost("register-user")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var authenticationResult = await this._registerService.RegisterUser(new RegisterDto(request.FirstName,
            request.LastName, request.Username, request.Email, request.Password));
        if (authenticationResult.Success)
        {
            return Redirect(request.RedirectUrl);
        }

        switch (authenticationResult.ErrorResult.FirstOrDefault().Reason)
        {
            case AuthenticationErrorReason.InvalidPassword: return this.BadRequest();
            case AuthenticationErrorReason.UserAlreadyExists: return this.Conflict();
            default: return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("login-user")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var authenticationResult =
            await this._loginService.LogIn(new(request.Username, request.Email, request.Password));
        if (authenticationResult.Success)
        {
            return this.Ok(authenticationResult.AuthenticationJwtToken);
        }

        switch (authenticationResult.ErrorResult.FirstOrDefault().Reason)
        {
            case AuthenticationErrorReason.UserNotExist: return this.NotFound();
            case AuthenticationErrorReason.WrongPassword: return this.Unauthorized("Password is Wrong");
            default: return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
        {
            return this.BadRequest();
        }

        var refreshTokenResponse =
            await this._refreshTokenService.RefreshToken(new AuthenticationJwtToken(request.Token, request.RefreshToken,
                request.ExpiresAt));
        if (refreshTokenResponse.Success)
        {
            return this.Ok(refreshTokenResponse.AuthenticationJwtToken);
        }

        switch (refreshTokenResponse.ErrorResult.First().Reason)
        {
            case JwtValidationErrorReason.TokenInvalid: return this.BadRequest();
            default: return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
