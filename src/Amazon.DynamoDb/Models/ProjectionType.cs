using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public enum ProjectionType : byte
    {
        KEYS_ONLY,
        INCLUDE,
        ALL,
    };

    public static class ProjectionTypeExtensions
    {
        public static string ToQuickString(this ProjectionType type)
        {
            return type switch
            {
                ProjectionType.KEYS_ONLY => "KEYS_ONLY",
                ProjectionType.INCLUDE => "INCLUDE",
                ProjectionType.ALL => "ALL",
                _ => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}
