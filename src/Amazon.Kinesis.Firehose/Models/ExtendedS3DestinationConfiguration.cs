using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class ExtendedS3DestinationConfiguration
{
    [JsonPropertyName("BucketARN")]
    public required string BucketARN { get; init; }

    [JsonPropertyName("RoleARN")]
    public required string RoleARN { get; init; }

    [JsonPropertyName("BufferingHints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BufferingHints? BufferingHints { get; init; }

    [JsonPropertyName("CloudWatchLoggingOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CloudWatchLoggingOptions? CloudWatchLoggingOptions { get; init; }

    [JsonPropertyName("CompressionFormat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CompressionFormat? CompressionFormat { get; init; }

    [JsonPropertyName("CustomTimeZone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CustomTimeZone { get; set; }

    // DataFormatConversionConfiguration

    [JsonPropertyName("EncryptionConfiguration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public EncryptionConfiguration? EncryptionConfiguration { get; init; }

    [JsonPropertyName("ErrorOutputPrefix")]
    [StringLength(1024)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorOutputPrefix { get; init; }

    [JsonPropertyName("Prefix")]
    [StringLength(1024)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Prefix { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? S3BackupMode { get; init; }
}