namespace Authorization.Contracts.Requests;

using System.ComponentModel.DataAnnotations;

public class RefreshTokenRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }

    [Required]
    public DateTime ExpiresAt { get; set; }
}
