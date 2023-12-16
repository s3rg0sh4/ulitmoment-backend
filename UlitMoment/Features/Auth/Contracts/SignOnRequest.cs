using UlitMoment.Database;

namespace UlitMoment.Features.Auth.Contracts;

public class SignOnRequest
{
    public required string Email { get; init; }
    public required Role Role { get; init; }
}
