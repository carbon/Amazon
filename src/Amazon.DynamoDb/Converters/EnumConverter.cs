using Carbon.Data;

namespace Amazon.DynamoDb;

internal sealed class EnumConverter : IDbValueConverter
{
    public static readonly EnumConverter Default = new();

    // ulong?

    public DbValue FromObject(object value, IMember? member)
    {
        return new DbValue(Convert.ToInt32(value));
    }

    public object ToObject(DbValue item, IMember? member)
    {
        return item.Kind is DbValueType.S
            ? Enum.Parse(member!.Type, item.ToString())
            : Enum.ToObject(member!.Type, item.ToInt());
    }
}
