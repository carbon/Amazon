using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class ProvisionedThroughputOverride
    {
        public int ReadCapacityUnits { get; set; }

        public JsonObject ToJson()
        {
            return new JsonObject()
            {
                { "ReadCapacityUnits", ReadCapacityUnits }
            };
        }
    }
}
