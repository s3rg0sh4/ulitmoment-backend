using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UlitMoment.Common.Services;
using UlitMoment.Configuration;
using UlitMoment.Database;
using UlitMoment.Features.Auth.Contracts;

namespace UlitMoment.Features.Auth;

public class AuthService(
    UserManager<User> userManager,
    UserContext userContext,
    UserService userService,
    TokenService tokenService,
    IOptions<ApplicationSettings> settings
)
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly UserContext _userContext = userContext;
    private readonly TokenService _tokenService = tokenService;
    private readonly UserService _userService = userService;
    private readonly int _refreshTokenLifetimeInDays = settings
        .Value
        .JWT
        .RefreshTokenValidityInDays;

    public async Task<LoginResponse> SignInAsync(SignInRequest request)
    {
        var user =
            await _userContext
                .Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == request.Email)
            ?? throw new UserNotFoundError(request.Email);

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            throw new WrongPasswordError();

        return await GenerateTokensAsync(user.Id);
    }

    public async Task<LoginResponse> UpdateTokenAsync(Guid userId, string refreshToken)
    {
        if (!await _userContext.Users.AnyAsync(u => u.Id == userId))
            throw new UserNotFoundError(userId);

        var currentToken =
            await _userContext
                .RefreshTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Value == refreshToken)
            ?? throw new InvalidTokenError();

        var newTokens = await GenerateTokensAsync(userId);

        _userContext.RefreshTokens.Remove(currentToken);
        await _userContext.SaveChangesAsync();
        return newTokens;
    }

    private async Task<LoginResponse> GenerateTokensAsync(Guid userId)
    {
        var role = await _userService.GetRoleByUserIdAsync(userId);
        var accessToken = _tokenService.CreateAccessToken(userId.ToString(), role);
        var refreshToken = _tokenService.CreateRefreshToken(userId.ToString());

        var tokensToRemove = await _userContext
            .RefreshTokens
            .Where(t => t.ExpireDate < DateTimeOffset.UtcNow)
            .ToListAsync();

        foreach (var expiredToken in tokensToRemove)
        {
            _userContext.RefreshTokens.Remove(expiredToken);
        }

        _userContext
            .RefreshTokens
            .Add(
                new RefreshToken
                {
                    Value = refreshToken,
                    ExpireDate = DateTimeOffset.UtcNow.AddDays(_refreshTokenLifetimeInDays),
                    UserId = userId
                }
            );

        await _userContext.SaveChangesAsync();
        return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }
}
