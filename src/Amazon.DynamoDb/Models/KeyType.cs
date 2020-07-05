using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public enum KeyType : byte
    {
        HASH,
        RANGE,
    };

    public static class KeyTypeExtensions
    {
        public static string ToQuickString(this KeyType type)
        {
            return type switch
            {
                KeyType.HASH => "HASH",
                KeyType.RANGE => "RANGE",
                _ => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}
