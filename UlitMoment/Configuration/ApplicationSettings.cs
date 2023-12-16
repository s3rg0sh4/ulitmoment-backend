using System.ComponentModel.DataAnnotations;

namespace UlitMoment.Configuration;

public class ApplicationSettings
{
    public const string SectionName = "Settings";

    public JWTSettings JWT { get; init; } = null!;

    public string DbConnectionString { get; init; } = null!;
}

public class JWTSettings
{
    [Required]
    public required string AccessSecret { get; init; }

    [Required]
    public required string RefreshSecret { get; init; }

    [Required]
    public required string ValidAudience { get; init; }

    [Required]
    public required string ValidIssuer { get; init; }

    [Range(1, 120)]
    public int AccessTokenValidityInMinutes { get; init; } = 30;

    [Range(1, 7)]
    public int RefreshTokenValidityInDays { get; init; } = 7;
}
