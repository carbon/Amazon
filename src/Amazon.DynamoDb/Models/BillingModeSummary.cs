using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class BillingModeSummary
    {
        public BillingMode BillingMode { get; set; }
        public DateTimeOffset LastUpdateToPayPerRequestDateTime { get; set; }
    }
}
