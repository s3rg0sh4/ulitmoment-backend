using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UlitMoment.Configuration;
using UlitMoment.Database;
using UlitMoment.Features.Auth.Contracts;

namespace UlitMoment.Features.Auth;

public class AuthService(
    ILogger<AuthService> logger,
    UserManager<User> userManager,
    UserContext userContext,
    TokenService tokenService,
    IOptions<ApplicationSettings> settings
)
{
    private readonly ILogger<AuthService> _logger = logger;
    private readonly UserManager<User> _userManager = userManager;
    private readonly UserContext _userContext = userContext;
    private readonly TokenService _tokenService = tokenService;
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
        {
            throw new WrongPasswordError();
        }

        //переписать
        return await GenerateTokensAsync(user);
    }

    public async Task<string> CreateUserAsync(SignOnRequest request)
    {
        if (await _userContext.Users.AnyAsync(u => u.Email == request.Email))
            throw new UserAlreadyExistError(request.Email);

        var user = new User(request.Email);
        await _userManager.CreateAsync(user);
        await _userManager.AddToRoleAsync(user, request.Role.ToString());

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return confirmationToken;
    }

    public async Task<IdentityResult> SetPasswordAsync(SetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new UserNotFoundError(request.Email);

		var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
            return result;

        return await _userManager.AddPasswordAsync(user, request.Password);
    }

    public async Task<UpdateTokenResponse> UpdateTokenAsync(UpdateTokenRequest request)
    {
        var (tokenIsValid, claimsPrincipal) = _tokenService.ValidateRefreshToken(
            request.RefreshToken
        );

        if (!tokenIsValid)
        {
            throw new InvalidTokenError();
        }

        var userId = new Guid(claimsPrincipal!.Claims.Single(c => c.Type == "UserId").Value);

        var user = await _userContext
            .Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            _logger.LogWarning(
                "user with id: {userId} has valid refresh token: {token} but there is no user with such id in database",
                userId,
                request.RefreshToken
            );
            throw new UserNotFoundError(userId);
        }
        ArgumentNullException.ThrowIfNull(user.RefreshTokens);

        var userHasToken = user.RefreshTokens.Any(t => t.Value == request.RefreshToken);

        if (!userHasToken)
        {
            _logger.LogWarning(
                "user with id: {userId} has valid refresh token: {token} but this token is not presented in user's RefreshTokens table",
                userId,
                request.RefreshToken
            );
            throw new InvalidTokenError();
        }

        var newAccessToken = _tokenService.CreateAccessToken(userId.ToString());
        var newRefreshToken = _tokenService.CreateRefreshToken(userId.ToString());

        user.RefreshTokens.Remove(user.RefreshTokens.Single(t => t.Value == request.RefreshToken));
        user.RefreshTokens.Add(
            new RefreshToken
            {
                Value = newRefreshToken,
                ExpireDate = DateTimeOffset.UtcNow.AddDays(_refreshTokenLifetimeInDays),
                UserId = user.Id
            }
        );

        await _userContext.SaveChangesAsync();

        return new UpdateTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    private async Task<LoginResponse> GenerateTokensAsync(User user)
    {
        var accessToken = _tokenService.CreateAccessToken(user.Id.ToString());
        var refreshToken = _tokenService.CreateRefreshToken(user.Id.ToString());

        ArgumentNullException.ThrowIfNull(user.RefreshTokens);
        var tokensToRemove = user.RefreshTokens
            .Where(t => t.ExpireDate < DateTimeOffset.UtcNow)
            .ToList();

        foreach (var expiredToken in tokensToRemove)
        {
            user.RefreshTokens.Remove(expiredToken);
        }

        user.RefreshTokens.Add(
            new RefreshToken
            {
                Value = refreshToken,
                ExpireDate = DateTimeOffset.UtcNow.AddDays(_refreshTokenLifetimeInDays),
                UserId = user.Id
            }
        );

        await _userManager.UpdateAsync(user);

        return new LoginResponse { AccessToken = accessToken, RefreshToken = refreshToken };
    }
}
