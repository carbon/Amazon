using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public readonly struct LabelCategory
{
    [JsonConstructor]
    public LabelCategory(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
