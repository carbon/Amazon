using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Sqs.Serialization;

// A data type may carry a custom label, such as Number.int or Binary.png
// https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-message-metadata.html#message-attribute-data-types
internal sealed class MessageAttributeDataTypeConverter : JsonConverter<MessageAttributeDataType>
{
    public override MessageAttributeDataType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ReadOnlySpan<char> name = reader.GetString();

        int labelIndex = name.IndexOf('.');

        if (labelIndex > -1)
        {
            name = name[..labelIndex];
        }

        return name switch {
            "String" => MessageAttributeDataType.String,
            "Number" => MessageAttributeDataType.Number,
            "Binary" => MessageAttributeDataType.Binary,
            _        => throw new JsonException($"Unknown message attribute data type: {name}")
        };
    }

    public override void Write(Utf8JsonWriter writer, MessageAttributeDataType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
