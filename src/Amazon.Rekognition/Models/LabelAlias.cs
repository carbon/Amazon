using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public readonly struct LabelAlias(string name)
{
    public string Name { get; } = name;
}