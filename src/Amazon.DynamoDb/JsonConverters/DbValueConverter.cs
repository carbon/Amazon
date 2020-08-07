using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.JsonConverters
{
    public class DbValueConverter : JsonConverter<DbValue>
    {
        // These are used to prevent additional allocations when reading arrays
        public DbValueConverter() { }

        #region Read
        public override DbValue Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return StaticRead(ref reader, options);
        }

        public static DbValue StaticRead(
            ref Utf8JsonReader reader,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"DbValue must be an object type. Tried to parse {reader.TokenType}");
            }

            DbValue value;

            // get the name of the first property
            reader.Read();
            if (reader.ValueTextEquals("B"))
            {
                value = new DbValue(ReadValueAsString(ref reader), DbValueType.B);
            }
            else if (reader.ValueTextEquals("BS"))
            {
                value = new DbValue(ReadValueAsStringArray(ref reader), DbValueType.BS);
            }
            else if (reader.ValueTextEquals("N"))
            {
                value = new DbValue(ReadValueAsString(ref reader), DbValueType.N);
            }
            else if (reader.ValueTextEquals("S"))
            {
                value = new DbValue(ReadValueAsString(ref reader), DbValueType.S);
            }
            else if (reader.ValueTextEquals("SS"))
            {
                value = new DbValue(ReadValueAsStringArray(ref reader), DbValueType.SS);
            }
            else if (reader.ValueTextEquals("NS"))
            {
                value = new DbValue(ReadValueAsStringArray(ref reader), DbValueType.NS);
            }
            else if (reader.ValueTextEquals("BOOL"))
            {
                reader.Read();
                value = new DbValue(reader.GetBoolean());
            }
            else if (reader.ValueTextEquals("L"))
            {
                value = new DbValue(ReadValueAsDbValueArray(ref reader, options));
            }
            else if (reader.ValueTextEquals("M"))
            {
                reader.Read();
                value = new DbValue(AttributeCollectionConverter.StaticRead(ref reader, typeof(AttributeCollection), options));
            }
            else
            {
                throw new JsonException($"Unrecognized DynamoDb value type: {reader.GetString()}");
            }

            reader.Read();
            return value;
        }

        

        private static string ReadValueAsString(ref Utf8JsonReader reader)
        {
            reader.Read();
            return reader.GetString();
        }

        private static string[] ReadValueAsStringArray(ref Utf8JsonReader reader)
        {
            using var stringListHandle = ObjectListCache<string>.AcquireHandle();

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

                stringListHandle.Value.Add(reader.GetString());
            }

            return stringListHandle.Value.ToArray();
        }

        private static DbValue[] ReadValueAsDbValueArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            using var dbValueListHandle = ObjectListCache<DbValue>.AcquireHandle();

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

                dbValueListHandle.Value.Add(StaticRead(ref reader,  options));
            }

            return dbValueListHandle.Value.ToArray();
        }
        #endregion

        #region Write
        public override void Write(
            Utf8JsonWriter writer,
            DbValue dbValue,
            JsonSerializerOptions options)
        {
            StaticWriteFullObject(writer, dbValue, options);
        }

        public static void StaticWriteFullObject(
            Utf8JsonWriter writer,
            DbValue dbValue,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            StaticWrite(writer, dbValue, options);
            writer.WriteEndObject();
        }

        private static void StaticWrite(Utf8JsonWriter writer, DbValue dbValue, JsonSerializerOptions options)
        {
            switch (dbValue.Kind)
            {
                case DbValueType.B:
                    writer.WriteBase64String("B", dbValue.ToBinary());
                    break;

                case DbValueType.BOOL:
                    writer.WriteBoolean("BOOL", dbValue.ToBoolean());
                    break;

                case DbValueType.BS:
                    writer.WriteStartArray("BS");
                    foreach (var bytes in (IEnumerable<byte[]>)dbValue.Value)
                    {
                        writer.WriteBase64StringValue(bytes);
                    }
                    writer.WriteEndArray();
                    break;

                case DbValueType.L:
                    writer.WriteStartArray("L");
                    foreach (var listDbValue in (IEnumerable<DbValue>)dbValue.Value)
                    {
                        StaticWriteFullObject(writer, listDbValue, options);
                    }
                    writer.WriteEndArray();
                    break;

                case DbValueType.M:
                    writer.WritePropertyName("M");
                    AttributeCollectionConverter.StaticWriteFullObject(writer, (AttributeCollection)dbValue.Value, options);
                    break;

                case DbValueType.N:
                    writer.WriteString("N", dbValue.ToString());
                    break;

                case DbValueType.NS:
                    writer.WriteStartArray("NS");
                    foreach (string s in dbValue.ToArray<string>())
                    {
                        writer.WriteStringValue(s);
                    }
                    writer.WriteEndArray();
                    break;

                case DbValueType.S:
                    writer.WriteString("S", dbValue.ToString());
                    break;

                case DbValueType.SS:
                    writer.WriteStartArray("SS");
                    foreach (string s in dbValue.ToArray<string>())
                    {
                        writer.WriteStringValue(s);
                    }
                    writer.WriteEndArray();
                    break;

                default:
                    throw new JsonException($"Cannot serialize DbValueType {dbValue.Kind}");

            }
        }
        #endregion
    }
}
