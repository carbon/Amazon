using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.CodeBuild.Converters;

internal sealed class TimestampConverter : JsonConverter<Timestamp>
{
    public override Timestamp Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new Timestamp(reader.GetDouble());
    }

    public override void Write(Utf8JsonWriter writer, Timestamp value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}