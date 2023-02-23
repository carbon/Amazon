using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public readonly struct Point
{
    [JsonConstructor]
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    [JsonPropertyName("X")]
    public double X { get; }

    [JsonPropertyName("Y")]
    public double Y { get; }
}
