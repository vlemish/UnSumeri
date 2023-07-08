namespace Authorization.Application.Models.Dtos;

public class JwtTokenDto
{
    public string JwtId { get; }

    public string Token { get; }

    public DateTime ExpiresAt { get; }

    public JwtTokenDto(string token, DateTime expiresAt, string jwtId)
    {
        this.JwtId = jwtId;
        this.Token = token;
        this.ExpiresAt = expiresAt;
    }
}
