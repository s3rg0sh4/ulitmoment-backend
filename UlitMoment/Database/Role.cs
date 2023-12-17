using System.Text.Json.Serialization;

namespace UlitMoment.Database;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Admin,
    Curator,
    Teacher,
    Student
}
