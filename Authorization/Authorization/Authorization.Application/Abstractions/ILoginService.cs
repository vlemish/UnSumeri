namespace Authorization.Application.Abstractions;

using Models;
using Models.Dtos;

public interface ILoginService<TPayload>
{
    Task<TPayload> LogIn(LoginDto loginDto);

    Task LogOut();
}
