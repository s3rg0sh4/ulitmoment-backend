namespace UlitMoment.Features.UserRegistration.Contracts;

public class RegisterRequest
{
    public required string Surname { get; init; }
    public required string Forename { get; init; }
    public string? Patronymic { get; init; }

    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Token { get; init; }
}
