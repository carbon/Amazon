using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class ElasticsearchDestinationConfiguration
{
    [JsonPropertyName("BufferingHints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BufferingHints? BufferingHints { get; init; }

    [JsonPropertyName("CloudWatchLoggingOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CloudWatchLoggingOptions? CloudWatchLoggingOptions { get; init; }

    [JsonPropertyName("ClusterEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClusterEndpoint { get; init; }

    [JsonPropertyName("DomainARN")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DomainARN { get; init; }

    [JsonPropertyName("IndexName")]
    public required string IndexName { get; init; }

    [JsonPropertyName("IndexRotationPeriod")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IndexRotationPeriod? IndexRotationPeriod { get; init; }
}