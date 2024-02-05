using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UlitMoment.Features.Courses.Contracts;
using UlitMoment.Features.Lessons;

namespace UlitMoment.Features.Courses;

[ApiController]
[Route("course/{courseId}/lesson")]
public class LessonController(LessonService lessonService, LessonMarksService lessonMarksService)
    : ControllerBase
{
    private readonly LessonService _lessonService = lessonService;
    private readonly LessonMarksService _lessonMarksService = lessonMarksService;

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetLessonListAsync(Guid courseId)
    {
        var list = await _lessonService.GetLessonListAsync(courseId);
        return Ok(list);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateLessonAsync(Guid courseId, CreateLessonRequest request)
    {
        var list = await _lessonService.CreateLessonAsync(courseId, request);
        return Ok(list);
    }
}
