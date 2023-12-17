namespace UlitMoment.Database.Courses;

public class Course
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public List<Lesson>? Lessons { get; set; }
}
