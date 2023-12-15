namespace UlitMoment.Features.Auth.Contracts;

public class UpdateTokenResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}
