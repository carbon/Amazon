using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class ArchivalSummary
    {
        public string? ArchivalBackupArn { get; set; }
        public string? ArchivalDateTime { get; set; }
        public string? ArchivalReason { get; set; }
    }
}
