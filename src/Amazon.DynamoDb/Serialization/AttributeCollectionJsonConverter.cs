using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Serialization;

internal sealed class AttributeCollectionJsonConverter : JsonConverter<AttributeCollection>
{
    public AttributeCollectionJsonConverter() { }

    public override AttributeCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return AttributeCollectionJsonSerializer.Read(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, AttributeCollection value, JsonSerializerOptions options)
    {
        AttributeCollectionJsonSerializer.Write(writer, value);
    }
}