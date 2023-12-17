using System.Text.Json.Serialization;

namespace UlitMoment.Database.Courses;

public class Lesson
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public required Guid CourseId { get; set; }

    [JsonIgnore]
    public Course? Course { get; set; }

    [JsonIgnore]
    public List<StudentLessonMark>? StudentMarks { get; set; }
}
