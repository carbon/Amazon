using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Serialization;

internal sealed class DbValueJsonConverter : JsonConverter<DbValue>
{
    public DbValueJsonConverter() { }

    public override DbValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DbValueJsonSerializer.Read(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, DbValue dbValue, JsonSerializerOptions options)
    {
        DbValueJsonSerializer.Write(writer, dbValue);
    }
}