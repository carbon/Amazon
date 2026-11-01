using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

// The service sends timestamps as a number of seconds since the Unix epoch
internal sealed class UnixTimestampConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds((long)(reader.GetDecimal() * 1000));
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.ToUnixTimeMilliseconds() / 1000m);
    }
}
