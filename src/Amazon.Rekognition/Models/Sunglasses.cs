using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public sealed class Sunglasses(bool value, double confidence)
{
    public bool Value { get; } = value;

    public double Confidence { get; } = confidence;
}