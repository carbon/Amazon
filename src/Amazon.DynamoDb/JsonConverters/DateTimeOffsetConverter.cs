using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.JsonConverters
{
    internal class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            double timestampSeconds = reader.GetDouble();
            return DateTimeOffset.FromUnixTimeMilliseconds((long)(timestampSeconds * 1000d));
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset dateTimeValue,
            JsonSerializerOptions options)
        {
            writer.WriteNumberValue(dateTimeValue.ToUnixTimeSeconds());
        }
    }
}
