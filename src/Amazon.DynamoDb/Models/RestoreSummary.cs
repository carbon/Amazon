using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class RestoreSummary
    {
        public DateTimeOffset RestoreDateTime { get; set; }
        public bool RestoreInProgress { get; set; }
        public string? SourceBackupArn { get; set; }

    }
}
