namespace UlitMoment.Features.Auth.Contracts;

public class SignInRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
