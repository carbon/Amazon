using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class RestoreSummary
    {
        public DateTimeOffset RestoreDateTime { get; set; }
        public bool RestoreInProgress { get; set; }
        public string? SourceBackupArn { get; set; }
    }
}
