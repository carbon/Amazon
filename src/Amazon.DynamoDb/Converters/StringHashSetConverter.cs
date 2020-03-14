using System.Collections.Generic;
using System.Linq;

using Carbon.Data;

namespace Amazon.DynamoDb
{
    internal sealed class StringHashSetConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            return new DbValue(((HashSet<string>)value).ToArray());
        }

        public object ToObject(DbValue item, IMember member) => item.ToStringSet();
    }
}