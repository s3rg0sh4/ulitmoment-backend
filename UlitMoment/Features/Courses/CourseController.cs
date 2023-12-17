using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UlitMoment.Common.HttpResponseErrors;
using UlitMoment.Common.Services;
using UlitMoment.Database;
using UlitMoment.Features.Courses.Contracts;

namespace UlitMoment.Features.Courses;

[ApiController]
[Route("course")]
public class CourseController(CourseService courseService) : ControllerBase
{
    private readonly CourseService _courseService = courseService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateCourseRequest request)
    {
        var result = await _courseService.CreateCourseAsync(request);
        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var userId = new Guid(User.Claims.First(c => c.Type == "UserId").Value);
        var role = Enum.Parse<Role>(User.Claims.First(c => c.Type == ClaimTypes.Role).Value);

        var result = role switch
        {
            Role.Admin => await _courseService.GetCourseListAsync(),
            Role.Teacher => await _courseService.GetTeacherCourseListAsync(userId),
            Role.Student => await _courseService.GetStudentCourseListAsync(userId),
            _ => throw new InvalidRoleError(role),
        };

        return Ok(result);
    }
}
