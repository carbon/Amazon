using System.Text.Json.Serialization;

using Amazon.S3.Vectors.Models;

namespace Amazon.S3.Vectors;

public sealed class S3VectorsErrorResult
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("fieldList")]
    public List<ValidationExceptionField>? FieldList { get; init; }
}