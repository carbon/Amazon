using System;

using Carbon.Data;

namespace Amazon.DynamoDb
{
    internal sealed class EnumConverter : IDbValueConverter
    {
        public static readonly EnumConverter Default = new ();

        // ulong?

        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue(Convert.ToInt32(value));
        }

        public object ToObject(DbValue item, IMember member)
        {
            // Parse strings to allow graceful migration to integers

            if (item.Kind == DbValueType.S)
            {
                return Enum.Parse(member.Type, item.ToString());
            }

            return Enum.ToObject(member.Type, item.ToInt());
        }
    }
}