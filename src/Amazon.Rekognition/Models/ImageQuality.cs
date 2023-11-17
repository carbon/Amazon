using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class ImageQuality
{
    [JsonPropertyName("Brightness")]
    public double Brightness { get; init; }

    [JsonPropertyName("Sharpness")]
    public double Sharpness { get; init; }
}