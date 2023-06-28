namespace Authorization.Application.Abstractions;

using Models;
using Models.Dtos;
using Models.Enumerations;

public interface IRegisterService<TPayload>
{
    Task<TPayload> RegisterUser(RegisterDto registerDto);
}
