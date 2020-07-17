using System;

namespace Amazon.DynamoDb
{
    public sealed class SSEDescription
    {
        public DateTimeOffset InaccessibleEncryptionDateTime { get; set; }

        public string? KMSMasterKeyArn { get; set; }

        public SSEType? SSEType { get; set; }

        public string? Status { get; set; }
    }
}