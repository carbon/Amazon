using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

[JsonConverter(typeof(JsonStringEnumConverter<DistanceMetric>))]
public enum DistanceMetric
{
    [JsonStringEnumMemberName("cosine")]
    Cosine = 1,

    [JsonStringEnumMemberName("euclidean")]
    Euclidean = 2
}


[JsonConverter(typeof(JsonStringEnumConverter<VectorDataType>))]
public enum VectorDataType
{
    [JsonStringEnumMemberName("float32")]
    Float32 = 1
}