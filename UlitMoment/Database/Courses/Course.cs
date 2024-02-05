using System.Text.Json.Serialization;

namespace UlitMoment.Database.Courses;

public class Course
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    [JsonIgnore]
    public List<Student>? Students { get; set; }

    [JsonIgnore]
    public List<Teacher>? Teachers { get; set; }

    [JsonIgnore]
    public List<Lesson>? Lessons { get; set; }
}
