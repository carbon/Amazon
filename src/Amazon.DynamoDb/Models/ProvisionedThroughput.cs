using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class ProvisionedThroughput
    {
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
