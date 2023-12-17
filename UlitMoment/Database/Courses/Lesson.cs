namespace UlitMoment.Database.Courses;

public class Lesson
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public required Guid CourseId { get; set; }
    public Course? Course { get; set; }

    public List<StudentLessonMark>? StudentMarks { get; set; }
}
