using Carbon.Json;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class ProvisionedThroughput
    {
        public ProvisionedThroughput() { }
        public ProvisionedThroughput(int readCapacityUnits, int writeCapacityUnits)
        {
            ReadCapacityUnits = readCapacityUnits;
            WriteCapacityUnits = writeCapacityUnits;
        }

        public int ReadCapacityUnits { get; set; }
        public int WriteCapacityUnits { get; set; }

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
