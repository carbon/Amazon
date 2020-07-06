using Amazon.DynamoDb.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class RestoreSummary : IConvertibleFromJson
    {
        public DateTimeOffset RestoreDateTime { get; set; }
        public bool RestoreInProgress { get; set; }
        public string? SourceBackupArn { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("RestoreDateTime")) RestoreDateTime = property.Value.GetDynamoDateTimeOffset();
            else if (property.NameEquals("RestoreInProgress")) RestoreInProgress = property.Value.GetBoolean();
            else if (property.NameEquals("SourceBackupArn")) SourceBackupArn = property.Value.GetString();
        }
    }
}
