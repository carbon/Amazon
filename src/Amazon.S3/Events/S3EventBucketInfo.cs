using System.Text.Json.Serialization;

namespace Amazon.S3.Events;

public sealed class S3EventBucketInfo
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("ownerIdentity")]
    public required S3UserIdentity OwnerIdentity { get; init; }

    [JsonPropertyName("arn")]
    public required string Arn { get; init; }
}