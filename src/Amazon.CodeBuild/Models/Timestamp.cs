using System.Text.Json.Serialization;

using Amazon.CodeBuild.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(TimestampConverter))]
public readonly struct Timestamp(double value)
{
    public double Value { get; } = value;

    public static implicit operator DateTime(Timestamp timestamp)
    {
        return DateTime.UnixEpoch.AddSeconds(timestamp.Value);
    }

    public static implicit operator DateTimeOffset(Timestamp timestamp)
    {
        return DateTimeOffset.UnixEpoch.AddSeconds(timestamp.Value);
    }
}