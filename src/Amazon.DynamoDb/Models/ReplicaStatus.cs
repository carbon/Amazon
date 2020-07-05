using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public enum ReplicaStatus : byte
    {
        CREATING,
        UPDATING,
        DELETING,
        ACTIVE,
    };

    public static class ReplicaStatusExtensions
    {
        public static string ToQuickString(this ReplicaStatus type)
        {
            return type switch
            {
                ReplicaStatus.CREATING => "CREATING",
                ReplicaStatus.UPDATING => "UPDATING",
                ReplicaStatus.DELETING => "DELETING",
                ReplicaStatus.ACTIVE => "ACTIVE",
                _ => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}
