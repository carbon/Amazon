#nullable disable

using System.Collections.Generic;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class TransactionConsumedCapacity
    {
        public float CapacityUnits { get; set; }
        public Dictionary<string, Capacity> GlobalSecondaryIndexes { get; set; }
        public Dictionary<string, Capacity> LocalSecondaryIndexes { get; set; }
        public float ReadCapacityUnits { get; set; }
        public Capacity Table { get; set; }
        public string TableNAme { get; set; }
        public float WriteCapacityUnits { get; set; }

    }

    public sealed class Capacity
    {
        public float CapacityUnits { get; set; }
        public float ReadCapacityUnits { get; set; }
        public float WriteCapacityUnits { get; set; }
    }
}

/*
{
   "TableName": "Thread",
   "CapacityUnits": 1
}
*/
