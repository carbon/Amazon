using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public enum TimeToLiveStatus : byte
    {
        ENABLING,
        DISABLING,
        ENABLED,
        DISABLED,
    };

    public static class TimeToLiveStatusExtensions
    {
        public static string ToQuickString(this TimeToLiveStatus type)
        {
            return type switch
            {
                TimeToLiveStatus.ENABLING => "ENABLING",
                TimeToLiveStatus.DISABLING => "DISABLING",
                TimeToLiveStatus.ENABLED => "ENABLED",
                TimeToLiveStatus.DISABLED => "DISABLED",
                _ => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}
