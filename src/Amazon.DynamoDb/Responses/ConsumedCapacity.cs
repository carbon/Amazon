#nullable disable

namespace Amazon.DynamoDb
{
    public class ConsumedCapacity
    {
        public string TableName { get; set; }

        public float CapacityUnits { get; set; }
    }
}

/*
{
   "TableName": "Thread",
   "CapacityUnits": 1
}
*/
