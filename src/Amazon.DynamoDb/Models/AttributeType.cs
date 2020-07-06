using System;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    public enum AttributeType : byte
    {
        Unknown = 0,

        /// <summary>
        /// Binary
        /// </summary>
        B,


        /// <summary>
        /// Number
        /// </summary>
        N,

        /// <summary>
        /// String
        /// </summary>
        S,
    }


    public static class AttributeTypeExtensions
    {
        public static string ToQuickString(this AttributeType type)
        {
            return type switch
            {
                AttributeType.B    => "B",
                AttributeType.N    => "N",
                AttributeType.S    => "S",
                _                => throw new Exception("Unexpected type:" + type.ToString()),
            };
        }
    }
}