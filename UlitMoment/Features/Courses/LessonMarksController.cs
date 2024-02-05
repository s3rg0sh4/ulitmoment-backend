using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UlitMoment.Features.Courses;

[ApiController]
[Route("[controller]")]
public class LessonMarksController(LessonMarksService lessonMarksService) : ControllerBase
{
	private readonly LessonMarksService _lessonMarksService = lessonMarksService;

	[HttpGet("lesson/{lessonId}/all")]
	[Authorize("Teacher")]
	public async Task<IActionResult> GetAllLessonMarks(Guid lessonId)
	{
		var result = await _lessonMarksService.GetLessonMarks(lessonId);
		return Ok(result);
	}

	[HttpGet("lesson/{lessonId}")]
	[Authorize("Student")]
	public async Task<IActionResult> GetLessonMark(Guid lessonId)
	{
		var studentId = User.Claims.First(c => c.Type == "UserId").Value;
		var result = await _lessonMarksService.GetStudentLessonMark(new Guid(studentId), lessonId);
		return Ok(result);
	}

	[HttpGet("all")]
	[Authorize("Student")]
	public async Task<IActionResult> GetAllLessonMarks()
	{
		var studentId = User.Claims.First(c => c.Type == "UserId").Value;
		var result = await _lessonMarksService.GetStudentMarks(new Guid(studentId));
		return Ok(result);
	}


}
