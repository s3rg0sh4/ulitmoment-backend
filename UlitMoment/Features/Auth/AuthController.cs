using Microsoft.AspNetCore.Mvc;

namespace UlitMoment.Features.Auth;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost]
    public async Task<IActionResult> SignOn([FromQuery] string userEmail, [FromQuery] Role role)
    {
        var token = await _authService.CreateUser(userEmail, role);
        return Ok(token);
    }

    public async Task<IActionResult> SetPassword(
        string userEmail,
        string token,
        string password
    )
    {
        var result = await _authService.SetPassword(userEmail, token, password);
        if (!result.Succeeded)
            return BadRequest(result);
        return Ok();
    }
}
