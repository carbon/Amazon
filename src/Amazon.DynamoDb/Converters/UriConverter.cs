using System;

namespace Amazon.DynamoDb
{
    internal sealed class UriConverter : DbTypeConverter<Uri>
    {
        public override Uri Parse(DbValue item) => new Uri(item.ToString());

        public override DbValue ToDbValue(Uri value) => new DbValue(value.ToString());
    }
}