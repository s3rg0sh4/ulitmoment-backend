using Microsoft.AspNetCore.Identity;

namespace UlitMoment.Database;

public class User(string email) : IdentityUser<Guid>(email) { }
