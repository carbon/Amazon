using Carbon.Data;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    internal sealed class JsonObjectConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) => new DbValue(value.ToString());

        public object ToObject(DbValue item, IMember member) => JsonObject.Parse(item.ToString());
    }
}