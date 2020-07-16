using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class SSESpecification
    {
        public bool? Enabled { get; set; }
        public string? KMSMasterKeyId { get; set; }
        public SSEType? SSEType { get; set; }
    }
}
