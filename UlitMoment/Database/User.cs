using Microsoft.AspNetCore.Identity;

namespace UlitMoment.Database;

public class User : IdentityUser<Guid>
{
    public string? Surname { get; set; }
    public string? Forename { get; set; }
    public string? Patronymic { get; set; }

    public List<RefreshToken>? RefreshTokens { get; set; }

    public User(string email)
        : base(email)
    {
        base.Email = email;
    }
}
