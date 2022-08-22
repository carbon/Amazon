using Carbon.Data;

namespace Amazon.DynamoDb;

internal sealed class DateTimeConverter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember member)
    {
        var date = new DateTimeOffset((DateTime)value);

        if (member?.Precision == 4)
        {
            return new DbValue(date.ToUnixTimeMilliseconds());
        }

        return new DbValue(date.ToUnixTimeSeconds());
    }

    public object ToObject(DbValue item, IMember member)
    {
        if (member?.Precision == 4)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(item.ToInt64()).UtcDateTime;
        }

        return DateTime.UnixEpoch.AddTicks(item.ToInt64() * TimeSpan.TicksPerSecond);
    }
}