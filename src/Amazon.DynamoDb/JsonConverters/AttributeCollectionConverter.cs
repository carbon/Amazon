using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.JsonConverters;

internal sealed class AttributeCollectionConverter : JsonConverter<AttributeCollection>
{
    public AttributeCollectionConverter() { }

    public override AttributeCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return AttributeCollectionJsonSerializer.Read(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, AttributeCollection value, JsonSerializerOptions options)
    {
        AttributeCollectionJsonSerializer.Write(writer, value);
    }
}
