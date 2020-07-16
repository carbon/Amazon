using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class ProvisionedThroughputOverride
    {
        public int ReadCapacityUnits { get; set; }
    }
}
