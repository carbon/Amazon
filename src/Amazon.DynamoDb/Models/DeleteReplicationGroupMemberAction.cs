using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class DeleteReplicationGroupMemberAction
    {
        public DeleteReplicationGroupMemberAction(string regionName)
        {
            RegionName = regionName;
        }

        public string RegionName { get; }

        public JsonObject ToJson()
        {
            var json = new JsonObject()
            {
                { "RegionName", RegionName }
            };

            return json;
        }
    }
}
