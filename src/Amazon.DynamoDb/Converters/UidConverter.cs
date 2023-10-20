using Carbon.Data.Sequences;

namespace Amazon.DynamoDb.Converters;

internal sealed class UidConverter : DbTypeConverter<Uid>
{
    public override Uid Parse(DbValue item) => Uid.Deserialize(item.ToBinary());

    public override DbValue ToDbValue(Uid value) => new DbValue(value.Serialize());
}