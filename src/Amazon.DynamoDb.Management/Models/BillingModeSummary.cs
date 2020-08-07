using System;

namespace Amazon.DynamoDb.Models
{
    public sealed class BillingModeSummary
    {
        public BillingMode BillingMode { get; set; }

        public DateTimeOffset LastUpdateToPayPerRequestDateTime { get; set; }
    }
}