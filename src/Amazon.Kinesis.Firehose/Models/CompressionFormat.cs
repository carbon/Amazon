using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

[JsonConverter(typeof(JsonStringEnumConverter<CompressionFormat>))]
public enum CompressionFormat
{
    [JsonStringEnumMemberName("UNCOMPRESSED")]
    Uncompressed,

    [JsonStringEnumMemberName("GZIP")]
    Gzip,

    [JsonStringEnumMemberName("ZIP")]
    Zip,

    [JsonStringEnumMemberName("Snappy")]
    Snappy,

    [JsonStringEnumMemberName("HADOOP_SNAPPY")]
    HadoopSnappy
}