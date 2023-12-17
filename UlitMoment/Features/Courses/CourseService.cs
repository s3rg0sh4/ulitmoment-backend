using Microsoft.EntityFrameworkCore;
using UlitMoment.Common.HttpResponseErrors;
using UlitMoment.Database;
using UlitMoment.Database.Courses;

namespace UlitMoment.Features.Courses;

public class CourseService(UserContext userContext)
{
    private readonly UserContext _userContext = userContext;

    public async Task<Guid> CreateCourseAsync(string name, string description)
    {
        var course = new Course { Name = name, Description = description };

        _userContext.Add(course);
        await _userContext.SaveChangesAsync();
        return course.Id;
    }

    public async Task<Course> UpdateCourseAsync(Course course)
    {
        if (!await _userContext.Courses.AnyAsync(c => c.Id == course.Id))
            throw new NotFoundError(nameof(Course), course.Id);

        _userContext.Update(course);
        await _userContext.SaveChangesAsync();
        return course;
    }

    public async Task<Course> GetCourseAsync(Guid id)
    {
        var course =
            await _userContext.Courses.FindAsync(id) ?? throw new NotFoundError(nameof(Course), id);

        return course;
    }

    public async Task DeleteCourse(Guid id)
    {
        var course =
            await _userContext.Courses.FindAsync(id) ?? throw new NotFoundError(nameof(Course), id);

        _userContext.Remove(course);
        await _userContext.SaveChangesAsync();
    }
}
