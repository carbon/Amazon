using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class Beard
{
    [JsonConstructor]
    public Beard(int confidence, bool value)
    {
        Confidence = confidence;
        Value = value;
    }

    public double Confidence { get; }

    public bool Value { get; }
}
