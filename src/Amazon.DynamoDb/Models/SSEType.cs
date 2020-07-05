using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public enum SSEType : byte
    {
        AES256,
        KMS,
    };

    public static class SSETypeExtensions
    {
        public static string ToQuickString(this SSEType type)
        {
            return type switch
            {
                SSEType.AES256 => "AES256",
                SSEType.KMS => "KMS",
                _ => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}
