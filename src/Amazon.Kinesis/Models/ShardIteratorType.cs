using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ShardIteratorType
{
    /// <summary>
    /// Start reading exactly from the position denoted by a specific sequence number.
    /// </summary>
    AT_SEQUENCE_NUMBER = 1,

    /// <summary>
    /// Start reading right after the position denoted by a specific sequence number.
    /// </summary>
    AFTER_SEQUENCE_NUMBER = 2,

    /// <summary>
    /// Start reading at the last untrimmed record in the shard in the system, which is the oldest data record in the shard.
    /// </summary>
    TRIM_HORIZON = 3,

    /// <summary>
    /// Start reading just after the most recent record in the shard, so that you always read the most recent data in the shard.
    /// </summary>
    LATEST = 4
}