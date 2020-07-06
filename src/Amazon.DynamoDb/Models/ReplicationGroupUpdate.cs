using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class ReplicationGroupUpdate
    {
        public CreateReplicationGroupMemberAction? Create { get; set; }
        public DeleteReplicationGroupMemberAction? Delete { get; set; }
        public UpdateReplicationGroupMemberAction? Update { get; set; }

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
