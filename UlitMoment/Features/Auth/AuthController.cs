using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UlitMoment.Features.Auth.Contracts;

namespace UlitMoment.Features.Auth;

[ApiController]
[Route("auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost("sign-on")]
    public async Task<IActionResult> SignOn(SignOnRequest request)
    {
        var token = await _authService.CreateUserAsync(request);
        return Ok(token);
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        var response = await _authService.SignInAsync(request);
        return Ok(response);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SetPassword(SetPasswordRequest request)
    {
        await _authService.SetPasswordAsync(request);
        return Ok();
    }

    [Authorize(AuthenticationSchemes = "Refresh")]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> UpdateToken()
    {
        var userId = User.Claims.First(c => c.Type == "UserId").Value;
        string authorizationHeader = HttpContext.Request.Headers.Authorization!;
        var token = authorizationHeader["Bearer ".Length..];

        var response = await _authService.UpdateTokenAsync(new Guid(userId), token!);
        return Ok(response);
    }
}
