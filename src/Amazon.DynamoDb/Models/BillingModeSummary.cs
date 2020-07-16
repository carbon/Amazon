using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class BillingModeSummary
    {
        public BillingMode BillingMode { get; set; }
        public DateTimeOffset LastUpdateToPayPerRequestDateTime { get; set; }
    }
}
