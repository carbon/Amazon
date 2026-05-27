using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class CreateIndexResult
{
    [JsonPropertyName("indexArn")]
    public string IndexArn { get; set; } = default!;
}
