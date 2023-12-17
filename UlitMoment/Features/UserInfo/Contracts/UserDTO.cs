using UlitMoment.Database;

namespace UlitMoment.Features.UserInfo.Contracts;

public class UserDTO
{
    public required Guid Id { get; init; }
    public required string FullName { get; init; }
    public required Role Role { get; init; }
}
