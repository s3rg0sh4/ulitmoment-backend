using Microsoft.EntityFrameworkCore;
using UlitMoment.Common.HttpResponseErrors;
using UlitMoment.Database;
using UlitMoment.Database.Courses;

namespace UlitMoment.Features.Courses;

public class LessonMarksService(UserContext userContext)
{
    private readonly UserContext _userContext = userContext;

    public async Task<List<StudentLessonMark>> GetLessonMarks(Guid lessonId)
    {
        var lesson =
            await _userContext
                .Lessons
                .Include(l => l.StudentMarks)
                .FirstOrDefaultAsync(l => l.Id == lessonId)
            ?? throw new NotFoundError(nameof(Lesson), lessonId);

        return lesson.StudentMarks!;
    }

    public async Task<List<StudentLessonMark>> GetStudentMarks(Guid studentId)
    {
        var student =
            await _userContext
                .Students
                .Include(s => s.LessonMarks)
                .FirstOrDefaultAsync(s => s.Id == studentId)
            ?? throw new NotFoundError(nameof(Student), studentId);

        return student.LessonMarks!;
    }

    public async Task SetStudentMark(Guid studentId, Guid lessonId, int mark)
    {
        if (!await _userContext.Lessons.AnyAsync(l => l.Id == lessonId))
            throw new NotFoundError(nameof(Lesson), lessonId);

        if (!await _userContext.Students.AnyAsync(s => s.Id == studentId))
            throw new NotFoundError(nameof(Student), studentId);

        _userContext.Add(
            new StudentLessonMark
            {
                Mark = mark,
                StudentId = studentId,
                LessonId = lessonId,
                Date = DateTimeOffset.UtcNow
            }
        );
        await _userContext.SaveChangesAsync();
    }
}
