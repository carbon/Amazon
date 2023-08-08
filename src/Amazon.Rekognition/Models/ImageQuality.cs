using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method:JsonConstructor]
public sealed class ImageQuality(double brightness, double sharpness)
{
    [JsonPropertyName("Brightness")]
    public double Brightness { get; } = brightness;

    [JsonPropertyName("Sharpness")]
    public double Sharpness { get; } = sharpness;
}