using Carbon.Json;

namespace Amazon.DynamoDb
{
    public class ConsumedCapacity
    {
        public string TableName { get; set; }

        public float CapacityUnits { get; set; }

        public static ConsumedCapacity FromJson(JsonObject json)
        {
            return json.As<ConsumedCapacity>();
        }
    }
}

/*
{
   "TableName": "Thread",
   "CapacityUnits": 1
}
*/
