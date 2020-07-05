using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb.Models
{
    public class SSEDescription : IConvertibleFromJson
    {
        public DateTimeOffset InaccessibleEncryptionDateTime { get; set; }
        public string? KMSMasterKeyArn { get; set; }
        public SSEType? SSEType { get; set; }
        public string? Status { get; set; }


        public void FillField(JsonProperty property)
        {
            if (property.NameEquals("InaccessibleEncryptionDateTime")) InaccessibleEncryptionDateTime = property.Value.GetDynamoDateTimeOffset();
            else if (property.NameEquals("KMSMasterKeyArn")) KMSMasterKeyArn = property.Value.GetString();
            else if (property.NameEquals("SSEType")) SSEType = property.Value.GetEnum<SSEType>();
            else if (property.NameEquals("Status")) Status = property.Value.GetString();
        }
    }
}
