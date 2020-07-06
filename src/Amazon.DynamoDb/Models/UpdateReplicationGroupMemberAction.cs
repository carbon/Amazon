﻿using Amazon.DynamoDb.Extensions;
using Carbon.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public class UpdateReplicationGroupMemberAction : ReplicationGroupMemberAction
    {
        public UpdateReplicationGroupMemberAction(string regionName)
            : base(regionName)
        {
        }
    }
}