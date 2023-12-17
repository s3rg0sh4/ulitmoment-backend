namespace UlitMoment.Database.Courses;

public class StudentLessonMark
{
    public Guid Id { get; set; }
    public required int Mark { get; set; }

    public required Guid StudentId { get; set; }
    public Student? Student { get; set; }
    public required Guid LessonId { get; set; }
    public Lesson? Lesson { get; set; }
}
