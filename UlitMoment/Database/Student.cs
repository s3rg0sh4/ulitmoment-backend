using UlitMoment.Database.Courses;

namespace UlitMoment.Database;

public class Student(string email) : User(email)
{
    public School? School { get; set; }

    public List<Course>? Courses { get; set; }
    public List<StudentLessonMark>? LessonMarks { get; set; }
}
