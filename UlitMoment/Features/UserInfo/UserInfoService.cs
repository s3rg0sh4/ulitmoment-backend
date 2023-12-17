using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UlitMoment.Common.HttpResponseErrors;
using UlitMoment.Database;
using UlitMoment.Features.UserInfo.Contracts;

namespace UlitMoment.Features.UserInfo;

public class UserInfoService(UserContext userContext, UserManager<User> userManager)
{
    private readonly UserContext _userContext = userContext;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<List<UserDTO>> GetUserListAsync()
    {
        var list = new List<UserDTO>();
        var users = await _userContext.Users.Where(u => u.PasswordHash != null).ToListAsync();
        foreach (var user in users)
        {
            var role = (await _userManager.GetRolesAsync(user)).Single();
            list.Add(
                new()
                {
                    Id = user.Id,
                    Role = Enum.Parse<Role>(role),
                    FullName = string.Join(' ', user.Surname, user.Forename, user.Patronymic),
                }
            );
        }
        return list;
    }

    public async Task<UserInfoDTO> GetUserInfoAsync(Guid userId)
    {
        var user =
            await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new NotFoundError(nameof(User), userId);

        var role = (await _userManager.GetRolesAsync(user)).Single();

        return new()
        {
            UserId = user.Id,
            Email = user.Email!,
            Forename = user.Forename!,
            Surname = user.Surname!,
            Patronymic = user.Patronymic,
            Role = Enum.Parse<Role>(role)
        };
    }
}
