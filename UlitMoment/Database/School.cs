namespace UlitMoment.Database;

public class School
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }

    public List<Student>? Students { get; set; }
}
