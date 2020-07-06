using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public class ArchivalSummary : IConvertibleFromJson
    {
        public string? ArchivalBackupArn { get; set; }
        public string? ArchivalDateTime { get; set; }
        public string? ArchivalReason { get; set; }

        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("ArchivalBackupArn"))
            {
                ArchivalBackupArn = property.Value.GetString();
            }
            else if (property.NameEquals("ArchivalDateTime"))
            {
                ArchivalDateTime = property.Value.GetString();
            }
            else if (property.NameEquals("ArchivalReason"))
            {
                ArchivalReason = property.Value.GetString();
            }
        }
    }
}
