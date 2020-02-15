using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Ssm.Converters
{
    internal sealed class NullableTimestampConverter : JsonConverter<Timestamp?>
    {
        public override Timestamp? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetDouble(out var value))
            {
                return new Timestamp(value);
            }
            else
            {
                return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, Timestamp? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteNumberValue(value.Value.Value);
            }
        }
    }
}