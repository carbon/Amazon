using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public sealed class AgeRange(int high, int low)
{
    public int High { get; } = high;

    public int Low { get; } = low;
}
