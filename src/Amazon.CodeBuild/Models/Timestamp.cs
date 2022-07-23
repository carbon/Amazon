using System.Text.Json.Serialization;

using Amazon.CodeBuild.Converters;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(TimestampConverter))]
public readonly struct Timestamp
{
    public Timestamp(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static implicit operator DateTime(Timestamp timestamp)
    {
        return DateTime.UnixEpoch.AddSeconds(timestamp.Value);
    }

    public static implicit operator DateTimeOffset(Timestamp timestamp)
    {
        return DateTimeOffset.UnixEpoch.AddSeconds(timestamp.Value);
    }
}