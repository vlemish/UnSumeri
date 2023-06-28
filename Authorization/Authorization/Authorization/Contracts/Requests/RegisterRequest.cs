namespace Authorization.Contracts.Requests;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }

    [Required]
    public string RedirectUrl { get; set; }
}
