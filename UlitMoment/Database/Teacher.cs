using UlitMoment.Database.Courses;

namespace UlitMoment.Database;

public class Teacher(string email) : User(email)
{
    public List<Course>? Courses { get; set; }
}
