namespace UlitMoment.Database;

public class RefreshToken
{
    public Guid Id { get; set; }
    public required string Value { get; set; }
    public required DateTimeOffset ExpireDate { get; set; }

    public required Guid UserId { get; set; }
    public User? User { get; set; }
}
