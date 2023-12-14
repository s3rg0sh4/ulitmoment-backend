using System.ComponentModel.DataAnnotations.Schema;

namespace UlitMoment.Database;

public class RefreshToken
{
    public string Id { get; set; } = null!;
    public required string Value { get; set; }
    public required DateTimeOffset ExpireDate { get; set; }

    public required Guid UserId { get; set; }
    public User? User { get; set; }
}
