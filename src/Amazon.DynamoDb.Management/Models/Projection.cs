#nullable disable

namespace Amazon.DynamoDb.Models;

public sealed class Projection
{
    public Projection() { }

    public Projection(string[] nonKeyAttributes, ProjectionType type)
    {
        NonKeyAttributes = nonKeyAttributes ?? Array.Empty<string>();
        ProjectionType = type;
    }

    public string[] NonKeyAttributes { get; init; }

    public ProjectionType ProjectionType { get; init; }
}
