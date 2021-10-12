using Carbon.Data;
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb
{
    internal sealed class TimeSpanConverter : IDbValueConverter
    {
        public DbValue FromObject(object value, IMember member)
        {
            var time = (TimeSpan)value;
            var precision = (TimePrecision)(member?.Precision ?? 3);

            return precision switch
            {
                TimePrecision.Second => new DbValue((int)time.TotalSeconds),
                _                    => new DbValue((int)time.TotalMilliseconds)
            };
        }

        public object ToObject(DbValue item, IMember member)
        {
            var precision = (TimePrecision)(member.Precision ?? 3);

            return precision switch
            {
                TimePrecision.Second => TimeSpan.FromSeconds(item.ToInt()),
                _                    => TimeSpan.FromMilliseconds(item.ToInt())
            };
        }
    }
}