using Microsoft.AspNetCore.Identity;
using UlitMoment.Common.HttpResponseErrors;
using UlitMoment.Database;

namespace UlitMoment.Common.Services;

public class UserService(UserManager<User> userManager)
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<Role> GetRoleByUserIdAsync(Guid userId)
    {
        var user = await GetUserAsync(userId);
        var role = await GetUserRoleAsync(user);
        return role;
    }
    
    public async Task<Role> GetUserRoleAsync(User user)
    {
        var role = (await _userManager.GetRolesAsync(user)).Single();
        return Enum.Parse<Role>(role);
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        var user =
            await _userManager.FindByIdAsync(id.ToString())
            ?? throw new NotFoundError(nameof(User), id);
        return user;
    }
}
