namespace Amazon.DynamoDb.Converters;

internal sealed class StringArrayConverter : DbTypeConverter<string[]>
{
    public override string[] Parse(DbValue dbValue) => dbValue.ToArray<string>();

    public override DbValue ToDbValue(string[] value) => new DbValue(value);
}