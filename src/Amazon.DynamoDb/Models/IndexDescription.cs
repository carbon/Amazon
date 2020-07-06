using Amazon.DynamoDb.Extensions;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class IndexDescription : IConvertibleFromJson
    {
        public string? IndexName { get; set; }
        public long IndexSizeBytes { get; set; }
        public long ItemCount { get; set; }
        public KeySchemaElement[]? KeySchema { get; set; }
        public Projection? Projection { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("IndexName")) IndexName = property.Value.GetString();
            else if (property.NameEquals("IndexSizeBytes")) IndexSizeBytes = property.Value.GetInt64();
            else if (property.NameEquals("ItemCount")) ItemCount = property.Value.GetInt64();
            else if (property.NameEquals("KeySchema")) KeySchema = property.Value.GetObjectArray<KeySchemaElement>();
            else if (property.NameEquals("Projection")) Projection = Projection.FromJsonElement(property.Value);
        }
    }
}
