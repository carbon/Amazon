using System;
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
        long unixTimeMillseconds = (long)(timestamp.Value * 1000d);

        return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMillseconds).UtcDateTime;
    }

    public static implicit operator DateTimeOffset(Timestamp timestamp)
    {
        long unixTimeMillseconds = (long)(timestamp.Value * 1000d);

        return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMillseconds);
    }
}

// scientific notation: 1.494825472676E9 
