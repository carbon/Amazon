using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.JsonConverters
{
    public class AttributeCollectionConverter : JsonConverter<AttributeCollection>
    {
        public AttributeCollectionConverter() { }

        public static AttributeCollection StaticRead(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"AttributeCollection must be an object type. Tried to parse {reader.TokenType}");
            }

            AttributeCollection attributes = new AttributeCollection();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                string attrName = reader.GetString();

                reader.Read();

                attributes.Add(attrName, DbValueConverter.StaticRead(ref reader, typeof(DbValue), options));
            }

            return attributes;
        }

        public override AttributeCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return StaticRead(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, AttributeCollection value, JsonSerializerOptions options)
        {
            StaticWriteFullObject(writer, value, options);
        }

        public static void StaticWriteFullObject(Utf8JsonWriter writer, AttributeCollection value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            StaticWriteChildren(writer, value, options);
            writer.WriteEndObject();
        }

        private static void StaticWriteChildren(Utf8JsonWriter writer, AttributeCollection value, JsonSerializerOptions options)
        {
            foreach (KeyValuePair<string, DbValue> kvp in value)
            {
                writer.WritePropertyName(kvp.Key);
                DbValueConverter.StaticWriteFullObject(writer, kvp.Value, options);
            }
        }
    }
}
