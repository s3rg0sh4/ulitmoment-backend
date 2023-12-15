using Microsoft.AspNetCore.Mvc;

using UlitMoment.Common.Exceptions;
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
        try
        {
            var token = await _authService.CreateUserAsync(request);
            return Ok(token);
        }
        catch (HttpResponseError err)
        {
            return StatusCode(err.StatusCode, err.Message);
        }
    }

	[HttpPost]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        try
        {
            await _authService.SignInAsync(request);
            return Ok();
        }
        catch (HttpResponseError err)
        {
            return StatusCode(err.StatusCode, err.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SetPassword(SetPasswordRequest request)
    {
        try
        {
            var result = await _authService.SetPasswordAsync(request);

		    if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok();
        }
        catch (HttpResponseError err)
        {
			return StatusCode(err.StatusCode, err.Message);
		}
    }

    [HttpPost]
    public async Task<IActionResult> UpdateToken(UpdateTokenRequest request)
    {
        try
        {
            var result = await _authService.UpdateTokenAsync(request);
            return Ok(result);
        }
        catch (HttpResponseError err)
        {
            return StatusCode(err.StatusCode, err.Message);
        }
    }
}
