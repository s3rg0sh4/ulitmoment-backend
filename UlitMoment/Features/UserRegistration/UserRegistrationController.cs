using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UlitMoment.Features.UserRegistration.Contracts;

namespace UlitMoment.Features.UserRegistration;

[ApiController]
[Route("user-registration")]
public class UserRegistrationController(UserRegistrationSerivce userRegistrationSerivce)
    : ControllerBase
{
    private readonly UserRegistrationSerivce _userRegistrationSerivce = userRegistrationSerivce;

    [HttpPost("invite-user")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser(InviteUserRequest request)
    {
        var result = await _userRegistrationSerivce.CreateUserAsync(request);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _userRegistrationSerivce.RegisterAsync(request);
        return Ok();
    }
}
