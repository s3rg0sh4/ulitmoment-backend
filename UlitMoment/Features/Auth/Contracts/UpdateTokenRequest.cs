namespace UlitMoment.Features.Auth.Contracts;

public class UpdateTokenRequest
{
    public required string RefreshToken { get; init; }
}
