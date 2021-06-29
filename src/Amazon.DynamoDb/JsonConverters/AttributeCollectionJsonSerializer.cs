using System.Collections.Generic;
using System.Text.Json;

namespace Amazon.DynamoDb.JsonConverters
{
    internal static class AttributeCollectionJsonSerializer
    {
        public static AttributeCollection Read(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"AttributeCollection must start with {{. Was {reader.TokenType}");
            }

            var attributes = new AttributeCollection();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                string attributeName = reader.GetString()!;

                reader.Read();

                attributes.Add(attributeName, DbValueJsonSerializer.Read(ref reader));
            }

            return attributes;
        }

        public static void Write(Utf8JsonWriter writer, AttributeCollection value)
        {
            writer.WriteStartObject();

            foreach (KeyValuePair<string, DbValue> kvp in value)
            {
                writer.WritePropertyName(kvp.Key);

                DbValueJsonSerializer.Write(writer, kvp.Value);
            }

            writer.WriteEndObject();
        }
    }
}