namespace Authorization.Application.Models;

public class AuthenticationJwtToken
{
    public string Token { get; }

    public string RefreshToken { get; }

    public DateTime ExpiresAt { get; }

    public AuthenticationJwtToken(string token, string refreshToken, DateTime expiresAt)
    {
        this.RefreshToken = refreshToken;
        this.Token = token;
        this.ExpiresAt = expiresAt;
    }
}
