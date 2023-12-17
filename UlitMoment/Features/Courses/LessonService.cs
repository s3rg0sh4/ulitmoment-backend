using Microsoft.EntityFrameworkCore;
using UlitMoment.Common.HttpResponseErrors;
using UlitMoment.Database;
using UlitMoment.Database.Courses;

namespace UlitMoment.Features.Lessons;

public class LessonService(UserContext userContext)
{
    private readonly UserContext _userContext = userContext;

    public async Task<Guid> CreateLessonAsync(Guid courseId, string name, string description)
    {
        var course = new Lesson
        {
            Name = name,
            Description = description,
            CourseId = courseId
        };

        _userContext.Add(course);
        await _userContext.SaveChangesAsync();
        return course.Id;
    }

    public async Task<Lesson> UpdateLessonAsync(Lesson lesson)
    {
        if (
            !await _userContext
                .Lessons
                .AnyAsync(l => l.Id == lesson.Id && l.CourseId == lesson.CourseId)
        )
            throw new NotFoundError(nameof(Lesson), lesson.Id);

        _userContext.Update(lesson);
        await _userContext.SaveChangesAsync();
        return lesson;
    }

    public async Task<List<Lesson>> GetLessonListAsync(Guid courseId)
    {
        return await _userContext.Lessons.Where(l => l.CourseId == courseId).ToListAsync();
    }

    public async Task<Lesson> GetLessonAsync(Guid courseId, Guid id)
    {
        var lesson =
            await _userContext
                .Lessons
                .FirstOrDefaultAsync(l => l.Id == id && l.CourseId == courseId)
            ?? throw new NotFoundError(nameof(Lesson), id);

        return lesson;
    }

    public async Task DeleteLesson(Guid courseId, Guid id)
    {
        var lesson =
            await _userContext
                .Lessons
                .FirstOrDefaultAsync(l => l.Id == id && l.CourseId == courseId)
            ?? throw new NotFoundError(nameof(Lesson), id);

        _userContext.Remove(lesson);
        await _userContext.SaveChangesAsync();
    }
}
