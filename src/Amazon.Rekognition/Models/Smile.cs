using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class Smile
{
    [JsonConstructor]
    public Smile(bool value, double confidence)
    {
        Value = value;
        Confidence = confidence;
    }

    public bool Value { get; }

    public double Confidence { get; }
}