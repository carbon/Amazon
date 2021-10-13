using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.JsonConverters;

internal sealed class DbValueConverter : JsonConverter<DbValue>
{
    public DbValueConverter() { }

    public override DbValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DbValueJsonSerializer.Read(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, DbValue dbValue, JsonSerializerOptions options)
    {
        DbValueJsonSerializer.Write(writer, dbValue);
    }
}
