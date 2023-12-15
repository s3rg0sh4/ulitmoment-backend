using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UlitMoment.Database;

namespace UlitMoment.Features.Auth;

public class AuthService(UserManager<User> userManager, UserContext userContext)
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly UserContext _userContext = userContext;

    public async Task<string> CreateUser(string email, Role role)
    {
        if (await _userContext.Users.AnyAsync(u => u.Email == email))
            throw new UserAlreadyExistException(email);

        var user = new User(email);
        await _userManager.CreateAsync(user);
        await _userManager.AddToRoleAsync(user, role.ToString());

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return confirmationToken;
    }

    public async Task<IdentityResult> SetPassword(string email, string token, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return IdentityResult.Failed(
                new IdentityError { Code = "UserNotFound", Description = "User not found" }
            );
        ;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return result;

        return await _userManager.AddPasswordAsync(user, password);
    }
}
