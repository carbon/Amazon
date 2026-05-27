using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

public sealed class VectorData
{
    [JsonPropertyName("float32")]
    public float[]? Float32 { get; init; }

    public static implicit operator VectorData(float[] values)
    {
        return new VectorData { Float32 = values };
    }
}
