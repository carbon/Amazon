using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public enum StreamViewType : byte
    {
        KEYS_ONLY,
        NEW_IMAGE,
        OLD_IMAGE,
        NEW_AND_OLD_IMAGES,
    };

    public static class StreamViewTypeExtensions
    {
        public static string ToQuickString(this StreamViewType type)
        {
            return type switch
            {
                StreamViewType.KEYS_ONLY => "KEYS_ONLY",
                StreamViewType.NEW_IMAGE => "NEW_IMAGE",
                StreamViewType.OLD_IMAGE => "OLD_IMAGE",
                StreamViewType.NEW_AND_OLD_IMAGES => "NEW_AND_OLD_IMAGES",
                _ => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}
