﻿using System.Text.Json.Serialization;

namespace UlitMoment.Database.Courses;

public class StudentLessonMark
{
    public Guid Id { get; set; }
    public required int Mark { get; set; }
    public required DateTimeOffset Date { get; set; }

    public required Guid StudentId { get; set; }

    [JsonIgnore]
    public Student? Student { get; set; }

    public required Guid LessonId { get; set; }

    [JsonIgnore]
    public Lesson? Lesson { get; set; }
}
