using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class GlobalSecondaryIndexUpdate
    {
        public GlobalSecondaryIndexUpdate()
        {
        }

        public CreateGlobalSecondaryIndexAction? Create { get; set; }
        public DeleteGlobalSecondaryIndexAction? Delete { get; set; }
        public UpdateGlobalSecondaryIndexAction? Update { get; set; }

        public JsonObject ToJson()
        {
            var json = new JsonObject();

            if (Create != null) json.Add("Create", Create.ToJson());
            if (Delete != null) json.Add("Delete", Delete.ToJson());
            if (Update != null) json.Add("Update", Update.ToJson());

            return json;
        }
    }
}
