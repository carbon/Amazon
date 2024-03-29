﻿namespace Amazon.DynamoDb.Models;

public sealed class ReplicationGroupUpdate
{
    public CreateReplicationGroupMemberAction? Create { get; set; }

    public DeleteReplicationGroupMemberAction? Delete { get; set; }

    public UpdateReplicationGroupMemberAction? Update { get; set; }
}