using System;

namespace Amazon.DynamoDb
{
    public enum DbValueType : byte
    {
        Unknown = 0,

        /// <summary>
        /// Binary
        /// </summary>
        B,

        /// <summary>
        /// BinarySet
        /// </summary>
        BS,

        /// <summary>
        /// Number
        /// </summary>
        N,

        /// <summary>
        /// String
        /// </summary>
        S,

        /// <summary>
        /// String Set
        /// </summary>
        SS,

        /// <summary>
        /// Number Set
        /// </summary>
        NS,

        /// <summary>
        /// Boolean
        /// </summary>
        BOOL,

        /// <summary>
        /// Null
        /// </summary>
        NULL,

        /// <summary>
        /// List
        /// </summary>
        L,

        /// <summary>
        /// Map
        /// </summary>
        M
    }


    public static class DbValueTypeExtensions
    {
        public static string ToQuickString(this DbValueType type)
        {
            switch (type)
            {
                case DbValueType.B      : return "B";
                case DbValueType.BOOL   : return "BOOL";
                case DbValueType.BS     : return "BS";
                case DbValueType.L      : return "L";
                case DbValueType.M      : return "M";
                case DbValueType.N      : return "N";
                case DbValueType.NS     : return "NS";
                case DbValueType.NULL   : return "NULL";
                case DbValueType.S      : return "S";
                case DbValueType.SS     : return "SS";
                default : throw new Exception("Unexpected type:" + type.ToString());
            }
        }
    }
}