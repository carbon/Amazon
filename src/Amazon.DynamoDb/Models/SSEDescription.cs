using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class SSEDescription
    {
        public DateTimeOffset InaccessibleEncryptionDateTime { get; set; }
        public string? KMSMasterKeyArn { get; set; }
        public SSEType? SSEType { get; set; }
        public string? Status { get; set; }
    }
}
