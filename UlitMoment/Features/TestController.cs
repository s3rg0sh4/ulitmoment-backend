using Microsoft.AspNetCore.Mvc;
using UlitMoment.Features.Auth;

namespace UlitMoment.Features;

[ApiController]
[Route("[controller]")]
public class TestController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost]
    public async Task<IActionResult> Test()
    {
        var token = await _authService.CreateUserAsync(
            new() { Email = "s3rg0sh4@gmail.com", Role = Role.Admin }
        );

        await _authService.SetPasswordAsync(new()
        {
            Email = "s3rg0sh4@gmail.com",
            Password = "s3rg0sh4",
            Token = token
        });

        return Ok();
    }
}
