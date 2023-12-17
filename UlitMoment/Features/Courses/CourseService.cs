using Microsoft.EntityFrameworkCore;
using UlitMoment.Common.HttpResponseErrors;
using UlitMoment.Database;
using UlitMoment.Database.Courses;
using UlitMoment.Features.Courses.Contracts;

namespace UlitMoment.Features.Courses;

public class CourseService(UserContext userContext)
{
    private readonly UserContext _userContext = userContext;

    public async Task<Guid> CreateCourseAsync(CreateCourseRequest request)
    {
        var course = new Course { Name = request.Name, Description = request.Description };

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

    public async Task<List<Course>> GetCourseListAsync()
    {
        return await _userContext.Courses.ToListAsync();
    }

    public async Task<List<Course>> GetStudentCourseListAsync(Guid studentId)
    {
        var student =
            await _userContext
                .Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == studentId)
            ?? throw new NotFoundError(nameof(Student), studentId);

        return student.Courses!;
    }

    public async Task<List<Course>> GetTeacherCourseListAsync(Guid teacherId)
    {
        var teacher =
            await _userContext
                .Teachers
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == teacherId)
            ?? throw new NotFoundError(nameof(Teacher), teacherId);

        return teacher.Courses!;
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
