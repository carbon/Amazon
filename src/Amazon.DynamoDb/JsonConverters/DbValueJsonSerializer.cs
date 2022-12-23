using System.Text.Json;

using Microsoft.Extensions.ObjectPool;

namespace Amazon.DynamoDb.JsonConverters;

using static DbValueTypeNames.Utf8;

internal static class DbValueJsonSerializer
{
    private static readonly ObjectPool<List<string>> StringListPool   = ObjectPool.Create(new ObjectPoolListPolicy<string>());
    private static readonly ObjectPool<List<DbValue>> DbValueListPool = ObjectPool.Create(new ObjectPoolListPolicy<DbValue>());

    public static DbValue Read(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"DbValue must start with {{. Was {reader.TokenType}");
        }

        DbValue value;

        // get the name of the first property
        reader.Read();

        if (reader.ValueTextEquals(B))
        {
            value = new DbValue(ReadString(ref reader), DbValueType.B);
        }
        else if (reader.ValueTextEquals(BS))
        {
            value = new DbValue(ReadStringArray(ref reader), DbValueType.BS);
        }
        else if (reader.ValueTextEquals(N))
        {
            value = new DbValue(ReadString(ref reader), DbValueType.N);
        }
        else if (reader.ValueTextEquals(S))
        {
            value = new DbValue(ReadString(ref reader), DbValueType.S);
        }
        else if (reader.ValueTextEquals(SS))
        {
            value = new DbValue(ReadStringArray(ref reader), DbValueType.SS);
        }
        else if (reader.ValueTextEquals(NS))
        {
            value = new DbValue(ReadStringArray(ref reader), DbValueType.NS);
        }
        else if (reader.ValueTextEquals(BOOL))
        {
            reader.Read();
            value = new DbValue(reader.GetBoolean());
        }
        else if (reader.ValueTextEquals(L))
        {
            value = new DbValue(ReadDbValueArray(ref reader));
        }
        else if (reader.ValueTextEquals(M))
        {
            reader.Read();
            value = new DbValue(AttributeCollectionJsonSerializer.Read(ref reader));
        }
        else
        {
            throw new JsonException("Invalid DynamoDB value type. Was " + reader.GetString());
        }

        reader.Read();

        return value;
    }

    private static string ReadString(ref Utf8JsonReader reader)
    {
        reader.Read();

        return reader.GetString()!;
    }

    private static string[] ReadStringArray(ref Utf8JsonReader reader)
    {
        var rentedStringList = StringListPool.Get();

        try
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException($"DbValue expected string array. Found {reader.TokenType}");
            }

            while (reader.Read())
            {
                if (reader.TokenType is JsonTokenType.EndArray)
                {
                    break;
                }

                rentedStringList.Add(reader.GetString()!);
            }

            return rentedStringList.ToArray();
        }
        finally
        {
            StringListPool.Return(rentedStringList);
        }
    }

    private static DbValue[] ReadDbValueArray(ref Utf8JsonReader reader)
    {
        var rentedDbValueList = DbValueListPool.Get();

        try
        {
            reader.Read();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException($"DbValue expected string array. Found {reader.TokenType}");
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                rentedDbValueList.Add(Read(ref reader));
            }

            return rentedDbValueList.ToArray();
        }
        finally
        {
            DbValueListPool.Return(rentedDbValueList);
        }
    }

    public static void Write(Utf8JsonWriter writer, DbValue dbValue)
    {
        writer.WriteStartObject();

        switch (dbValue.Kind)
        {
            case DbValueType.B:
                writer.WriteBase64String(B, dbValue.ToBinary());
                break;

            case DbValueType.BOOL:
                writer.WriteBoolean(BOOL, dbValue.ToBoolean());
                break;

            case DbValueType.BS:
                writer.WriteStartArray(BS);
                foreach (var bytes in (IEnumerable<byte[]>)dbValue.Value)
                {
                    writer.WriteBase64StringValue(bytes);
                }
                writer.WriteEndArray();
                break;

            case DbValueType.L:
                writer.WriteStartArray(L);
                foreach (var listDbValue in (IEnumerable<DbValue>)dbValue.Value)
                {
                    Write(writer, listDbValue);
                }
                writer.WriteEndArray();
                break;

            case DbValueType.M:
                writer.WritePropertyName(M);
                AttributeCollectionJsonSerializer.Write(writer, (AttributeCollection)dbValue.Value);
                break;

            case DbValueType.N:
                writer.WriteString(N, dbValue.ToString());
                break;

            case DbValueType.NS:
                writer.WriteStartArray(NS);
                foreach (string s in dbValue.ToArray<string>())
                {
                    writer.WriteStringValue(s);
                }
                writer.WriteEndArray();
                break;

            case DbValueType.S:
                writer.WriteString(S, dbValue.ToString());
                break;

            case DbValueType.SS:
                writer.WriteStartArray(SS);
                foreach (string s in dbValue.ToArray<string>())
                {
                    writer.WriteStringValue(s);
                }
                writer.WriteEndArray();
                break;

            default:
                throw new JsonException($"Cannot serialize DbValueType {dbValue.Kind}");

        }

        writer.WriteEndObject();
    }
}