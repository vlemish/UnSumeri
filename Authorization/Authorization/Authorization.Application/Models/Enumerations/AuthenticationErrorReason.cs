namespace Authorization.Application.Models.Enumerations;

public enum AuthenticationErrorReason
{
    UserNotExist,
    UserAlreadyExists,
    WrongPassword,
    InvalidPassword
}
