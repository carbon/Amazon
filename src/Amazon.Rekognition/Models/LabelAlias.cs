using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public readonly struct LabelAlias
{
    [JsonConstructor]
    public LabelAlias(string name)
    {
        Name = name;
    }

    public string Name { get; }
}