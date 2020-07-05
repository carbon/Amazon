using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class TimeToLiveDescription
    {
        public string? AttributeName { get; set; }
        public TimeToLiveStatus TimeToLiveStatus { get; set; }
    }
}
