using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public readonly struct Point(double x, double y)
{
    [JsonPropertyName("X")]
    public double X { get; } = x;

    [JsonPropertyName("Y")]
    public double Y { get; } = y;
}
