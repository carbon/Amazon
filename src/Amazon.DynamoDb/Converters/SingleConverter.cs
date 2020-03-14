using System;

using Carbon.Data;

namespace Amazon.DynamoDb
{
    internal sealed class SingleConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) => new DbValue((Single)value);

        public object ToObject(DbValue item, IMember member) => item.ToSingle();
    }
}