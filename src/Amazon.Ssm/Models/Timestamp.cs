using System.Text.Json.Serialization;

using Amazon.Ssm.Converters;

namespace Amazon.Ssm;

[JsonConverter(typeof(TimestampConverter))]
public readonly struct Timestamp
{
    private readonly double _value;

    public Timestamp(double value)
    {
        _value = value;
    }

    public double Value => _value;

    public static implicit operator DateTime(Timestamp timestamp)
    {
        return DateTime.UnixEpoch.AddSeconds(timestamp.Value);
    }

    public static implicit operator DateTimeOffset(Timestamp timestamp)
    {
        return DateTimeOffset.UnixEpoch.AddSeconds(timestamp.Value);
    }
}

// scientific notation: 1.494825472676E9