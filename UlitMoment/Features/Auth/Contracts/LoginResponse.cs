namespace UlitMoment.Features.Auth.Contracts;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
