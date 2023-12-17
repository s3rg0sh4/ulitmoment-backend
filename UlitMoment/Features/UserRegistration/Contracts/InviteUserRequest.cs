using UlitMoment.Database;

namespace UlitMoment.Features.UserRegistration.Contracts;

public class InviteUserRequest
{
    public required string Email { get; init; }
    public required Role Role { get; init; }
}
