using Carbon.Data;
using Carbon.Json;

namespace Amazon.DynamoDb
{
    internal sealed class JsonArrayConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member) => new DbValue(value.ToString());

        public object ToObject(DbValue item, IMember member)
        {
            var text = item.ToString();

            return JsonArray.Parse(text);
        }
    }
}