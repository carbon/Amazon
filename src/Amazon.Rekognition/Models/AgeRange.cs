using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class AgeRange
{
    [JsonConstructor]
    public AgeRange(int high, int low)
    {
        High = high;
        Low = low;
    }

    public int High { get; }

    public int Low { get; }
}
