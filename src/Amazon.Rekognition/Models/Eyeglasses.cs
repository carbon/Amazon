using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class Eyeglasses
{
    [JsonConstructor]
    public Eyeglasses(bool value, double confidence)
    {
        Value = value;
        Confidence = confidence;
    }

    public bool Value { get; }

    public double Confidence { get; }
}