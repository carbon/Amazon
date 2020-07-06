using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb
{
    public enum TableStatus : byte
    {
        CREATING,
        UPDATING,
        DELETING,
        ACTIVE,
        INACCESSIBLE_ENCRYPTION_CREDENTIALS,
        ARCHIVING,
        ARCHIVED,
    };

    public static class TableStatusExtensions
    {
        public static string ToQuickString(this TableStatus type)
        {
            return type switch
            {
                TableStatus.CREATING => "CREATING",
                TableStatus.UPDATING => "UPDATING",
                TableStatus.DELETING => "DELETING",
                TableStatus.ACTIVE => "ACTIVE",
                TableStatus.INACCESSIBLE_ENCRYPTION_CREDENTIALS => "INACCESSIBLE_ENCRYPTION_CREDENTIALS",
                TableStatus.ARCHIVING => "ARCHIVING",
                TableStatus.ARCHIVED => "ARCHIVED",
                _ => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}
