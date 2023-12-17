using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UlitMoment.Database;
using UlitMoment.Features.Auth;
using UlitMoment.Features.Auth.Errors;
using UlitMoment.Features.UserRegistration.Contracts;

namespace UlitMoment.Features.UserRegistration;

public class UserRegistrationSerivce(UserContext userContext, UserManager<User> userManager)
{
    private readonly UserContext _userContext = userContext;
    private readonly UserManager<User> _userManager = userManager;

    public async Task RegisterAsync(RegisterRequest request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email)
            ?? throw new UserNotFoundError(request.Email);

        user.Surname = request.Surname;
        user.Forename = request.Forename;
        user.Patronymic = request.Patronymic;

        var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!confirmEmailResult.Succeeded)
            throw new IdentityResponseError(confirmEmailResult.Errors);

        var addPasswordResult = await _userManager.AddPasswordAsync(user, request.Password);
        if (!addPasswordResult.Succeeded)
            throw new IdentityResponseError(addPasswordResult.Errors);
    }

    public async Task<string> CreateUserAsync(InviteUserRequest request)
    {
        if (await _userContext.Users.AnyAsync(u => u.Email == request.Email))
            throw new UserAlreadyExistError(request.Email);

        var user = new User(request.Email);
        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
            throw new IdentityResponseError(createResult.Errors);

        var addToRoleResult = await _userManager.AddToRoleAsync(user, request.Role.ToString());
        if (!addToRoleResult.Succeeded)
            throw new IdentityResponseError(addToRoleResult.Errors);

        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }
}
