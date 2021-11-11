#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class ExtendedS3DestinationConfiguration
{
    [JsonPropertyName("BucketARN")]
    public string BucketARN { get; init; }

#nullable enable

    public BufferingHints? BufferingHints { get; init; }

    public CloudWatchLoggingOptions? CloudWatchLoggingOptions { get; init; }

    // UNCOMPRESSED | GZIP | ZIP | Snappy | HADOOP_SNAPPY
    public string? CompressionFormat { get; init; }

    // DataFormatConversionConfiguration

    public EncryptionConfiguration? EncryptionConfiguration { get; init; }

    [StringLength(1024)]
    public string? ErrorOutputPrefix { get; init; }

    [StringLength(1024)]
    public string? Prefix { get; init; }

#nullable disable

    [JsonPropertyName("RoleARN")]
    public string RoleARN { get; init; }

#nullable enable

    public string? S3BackupMode { get; init; }
}