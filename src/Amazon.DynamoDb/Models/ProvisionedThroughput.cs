using Carbon.Json;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public class ProvisionedThroughput : IConvertibleFromJson
    {
        public ProvisionedThroughput() { }
        public ProvisionedThroughput(int readCapacityUnits, int writeCapacityUnits)
        {
            ReadCapacityUnits = readCapacityUnits;
            WriteCapacityUnits = writeCapacityUnits;
        }

        public int ReadCapacityUnits { get; set; }
        public int WriteCapacityUnits { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("ReadCapacityUnits")) ReadCapacityUnits = property.Value.GetInt32();
            else if (property.NameEquals("WriteCapacityUnits")) WriteCapacityUnits = property.Value.GetInt32();
        }

        public JsonObject ToJson()
        {
            return new JsonObject
            {
                { "ReadCapacityUnits", ReadCapacityUnits },
                { "WriteCapacityUnits", WriteCapacityUnits }
            };
        }
    }
}
