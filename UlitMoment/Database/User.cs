using Microsoft.AspNetCore.Identity;

namespace UlitMoment.Database;

public class User : IdentityUser<Guid>
{
    public User(string email) : base(email)
    {
        base.Email = email;
    }

    public List<RefreshToken>? RefreshTokens { get; set; }
}
