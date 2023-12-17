using UlitMoment.Database;

namespace UlitMoment.Features.UserInfo.Contracts;

public class UserInfoDTO
{
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required Role Role { get; set; }

    public required string Surname { get; init; }
    public required string Forename { get; init; }
    public required string? Patronymic { get; init; }
}
