using Carbon.Data;
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb
{
    internal sealed class DateTimeOffsetConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var date = (DateTimeOffset)value;

            var precision = (TimePrecision)(member?.Precision ?? 0);

            return precision switch
            {
                TimePrecision.Millisecond => new DbValue(date.ToUnixTimeMilliseconds()),
                _                         => new DbValue(date.ToUnixTimeSeconds())
            };
        }

        public object ToObject(DbValue item, IMember member)
        {
            var precision = (TimePrecision)(member?.Precision ?? 0);

            return precision switch
            {
                TimePrecision.Millisecond => DateTimeOffset.FromUnixTimeMilliseconds(item.ToInt64()),
                _                         => DateTimeOffset.FromUnixTimeSeconds(item.ToInt64())
            };
        }
    }
}