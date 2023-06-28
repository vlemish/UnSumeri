namespace Authorization.Application;

using Abstractions;
using Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Enumerations;
using Models.Results;
using Services;

public static class BootstrapApplication
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection serviceCollection)
    {
        // serviceCollection.AddScoped<IAuthenticationService, JwtAuthenticationService>();
        serviceCollection.AddTransient<IJwtTokenService, JwtTokenService>();
        serviceCollection.AddScoped<IRefreshTokenService<ApplicationUser>, JwtRefreshTokenService>();
        serviceCollection.AddScoped<ILoginService<LoginResult>, JwtLoginService>();
        serviceCollection
            .AddScoped<IRegisterService<RegistrationResult>, JwtRegisterService>();

        return serviceCollection;
    }
}

// {
// "firstName": "Vladislav Test1",
// "lastName": "Vladislav Test2",
// "username": "VladislavLemish",
// "email": "vlad.lemish123123@gmail.com",
// "password": "test1",
// "redirectUrl": "Url"
// }
