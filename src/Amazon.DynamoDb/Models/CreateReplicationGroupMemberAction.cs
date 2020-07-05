using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public class CreateReplicationGroupMemberAction : ReplicationGroupMemberAction
    {
        public CreateReplicationGroupMemberAction(string regionName)
            : base(regionName)
        {
        }
    }
}
