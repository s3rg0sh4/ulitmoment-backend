using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UlitMoment.Database;

public class UserContext : IdentityDbContext<User, IdentityRole<Guid>, Guid> { }
