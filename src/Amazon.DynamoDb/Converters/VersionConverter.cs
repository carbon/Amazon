#pragma warning disable IDE0090 // Use 'new(...)'

namespace Amazon.DynamoDb
{
    internal sealed class VersionConverter : DbTypeConverter<Version>
    {
        public override Version Parse(DbValue item) => Version.Parse(item.ToString());

        public override DbValue ToDbValue(Version value) => new DbValue(value.ToString());
    }
}