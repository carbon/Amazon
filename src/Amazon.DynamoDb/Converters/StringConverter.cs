using Carbon.Data;

namespace Amazon.DynamoDb;

internal sealed class StringConverter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember member)
    {
        string text = (string)value;

        if (text.Length == 0) return DbValue.Empty;

        return new DbValue(text);
    }

    public object ToObject(DbValue item, IMember member)
    {
        return item.ToString();
    }
}
