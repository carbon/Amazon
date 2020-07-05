using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class ArchivalSummary
    {
        public string? ArchivalBackupArn { get; set; }
        public string? ArchivalDateTime { get; set; }
        public string? ArchivalReason { get; set; }
    }
}
