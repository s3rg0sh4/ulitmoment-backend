namespace UlitMoment.Features.Courses.Contracts;

public class CreateCourseRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}
