﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Serialization;

internal sealed class BatchGetItemRequestConverter : JsonConverter<BatchGetItemRequest>
{
    public BatchGetItemRequestConverter() { }

    public override BatchGetItemRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, BatchGetItemRequest value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteStartObject("RequestItems"u8);

        foreach (TableKeys set in value.Sets)
        {
            writer.WriteStartObject(set.TableName);
            writer.WriteStartArray("Keys"u8);

            foreach (var keySet in set.Keys)
            {
                writer.WriteStartObject();

                foreach (var kvp in keySet)
                {
                    writer.WritePropertyName(kvp.Key);
                    DbValueJsonSerializer.Write(writer, kvp.Value);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            if (set.AttributesToGet is { Length: > 0 })
            {
                writer.WriteStartArray("AttributesToGet"u8);

                foreach (var attr in set.AttributesToGet)
                {
                    writer.WriteStringValue(attr);
                }

                writer.WriteEndArray();
            }

            if (set.ConsistentRead)
            {
                writer.WriteBoolean("ConsistentRead"u8, set.ConsistentRead);
            }

            writer.WriteEndObject();
        }

        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}
