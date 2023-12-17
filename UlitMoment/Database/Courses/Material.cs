namespace UlitMoment.Database.Courses;

public class Material
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    // Key to minio file
    public required string PresentationId { get; set; }
}
