using Carbon.Data;

namespace Amazon.DynamoDb
{
    public interface IDbValueConverter
    {
        DbValue FromObject(object value, IMember meta = null!);

        object ToObject(DbValue item, IMember meta);
    }
}