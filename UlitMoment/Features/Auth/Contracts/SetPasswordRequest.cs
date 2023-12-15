namespace UlitMoment.Features.Auth.Contracts;

public class SetPasswordRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Token { get; init; }
}
