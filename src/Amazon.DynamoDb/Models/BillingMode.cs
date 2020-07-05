using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.DynamoDb.Models
{
    public enum BillingMode : byte
    {
        PROVISIONED,
        PAY_PER_REQUEST,
    };

    public static class BillingModeExtensions
    {
        public static string ToQuickString(this BillingMode type)
        {
            return type switch
            {
                BillingMode.PROVISIONED => "PROVISIONED",
                BillingMode.PAY_PER_REQUEST => "PAY_PER_REQUEST",
                _ => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}
