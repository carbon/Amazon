using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public readonly struct Parent(string name)
{
    public string Name { get; } = name;
}