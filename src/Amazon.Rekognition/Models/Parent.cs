using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public readonly struct Parent
{
    [JsonConstructor]
    public Parent(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
