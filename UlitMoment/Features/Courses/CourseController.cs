using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UlitMoment.Common.Services;

namespace UlitMoment.Features.Courses;

[ApiController]
[Route("course")]
public class CourseController(CourseService courseService, UserService userService) : ControllerBase
{
    private readonly CourseService _courseService = courseService;
    private readonly UserService _userService = userService;

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCourseRequest request)
    {
        var result = await _courseService.CreateCourseAsync(request);
        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin, Teacher, Student")]
    public async Task<IActionResult> GetAll()
    {
        var userId = new Guid(User.Claims.First(c => c.Type == "UserId").Value);
        var role = await _userService.GetRoleByUserIdAsync(userId);

        var result = role switch
		{
			Database.Role.Admin => await _courseService.GetCourseListAsync(),
			Database.Role.Teacher => await _courseService.GetTeacherCourseListAsync(userId),
			Database.Role.Student => await _courseService.GetStudentCourseListAsync(userId),
			_ => throw new NotImplementedException(),
		};

        return Ok(result);
    }
}
