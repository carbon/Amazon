using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class SSEDescription
    {
        public DateTimeOffset InaccessibleEncryptionDateTime { get; set; }
        public string? KMSMasterKeyArn { get; set; }
        public SSEType? SSEType { get; set; }
        public string? Status { get; set; }
    }
}
