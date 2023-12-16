using Microsoft.AspNetCore.Mvc;
using UlitMoment.Database;
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
        return Ok();
    }
}
