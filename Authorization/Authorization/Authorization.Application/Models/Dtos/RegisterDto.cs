namespace Authorization.Application.Models.Dtos;

public record RegisterDto(string FirstName, string LastName, string Username, string Email, string Password);
