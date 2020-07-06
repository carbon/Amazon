using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class SSESpecification
    {
        public bool? Enabled { get; set; }
        public string? KMSMasterKeyId { get; set; }
        public SSEType? SSEType { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject();

            if (Enabled.HasValue) json.Add("Enabled", Enabled.Value);
            if (KMSMasterKeyId != null) json.Add("KMSMasterKeyId", KMSMasterKeyId);
            if (SSEType.HasValue) json.Add("SSEType", SSEType.Value.ToQuickString());

            return json;
        }
    }
}
