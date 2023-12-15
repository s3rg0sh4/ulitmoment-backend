using Microsoft.AspNetCore.Mvc;
using UlitMoment.Features.Auth.Contracts;

namespace UlitMoment.Features.Auth;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        await _authService.SignInAsync(request);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SignOn(SignOnRequest request)
    {
        var token = await _authService.CreateUserAsync(request);
        return Ok(token);
    }

    [HttpPost]
    public async Task<IActionResult> SetPassword(SetPasswordRequest request)
    {
        var result = await _authService.SetPasswordAsync(request);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateToken(UpdateTokenRequest request)
    {
        var result = await _authService.UpdateTokenAsync(request);
        return Ok(result);
    }
}
