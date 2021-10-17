using System.Text.Json.Serialization;

using Amazon.DynamoDb.JsonConverters;

namespace Amazon.DynamoDb;

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
        long unixTimeMillseconds = (long)(timestamp.Value * 1000d);

        return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMillseconds).UtcDateTime;
    }

    public static implicit operator DateTimeOffset(Timestamp timestamp)
    {
        long unixTimeMillseconds = (long)(timestamp.Value * 1000d);

        return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMillseconds);
    }
}
