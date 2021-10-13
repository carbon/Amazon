namespace Amazon.DynamoDb;

internal sealed class GuidConverter : DbTypeConverter<Guid>
{
    public override Guid Parse(DbValue item) => new Guid(item.ToBinary());

    public override DbValue ToDbValue(Guid value) => new DbValue(value.ToByteArray());
}
