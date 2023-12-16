using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UlitMoment.Features.Auth.Contracts;

namespace UlitMoment.Features.Auth;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost]
    public async Task<IActionResult> SignOn(SignOnRequest request)
    {
        var token = await _authService.CreateUserAsync(request);
        return Ok(token);
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        var response = await _authService.SignInAsync(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> SetPassword(SetPasswordRequest request)
    {
        await _authService.SetPasswordAsync(request);
        return Ok();
    }

    [HttpPost]
    [Authorize("RefreshToken")]
    public async Task<IActionResult> UpdateToken()
    {
        var userId = User.Claims.First(c => c.Type == "UserId").Value;
        var token = HttpContext.Request.Headers.Authorization.First()!["Bearer ".Length..];

        var response = await _authService.UpdateTokenAsync(new Guid(userId), token!);
        return Ok(response);
    }
}
