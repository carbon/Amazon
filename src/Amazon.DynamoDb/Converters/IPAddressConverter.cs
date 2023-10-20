using System.Net;

namespace Amazon.DynamoDb.Converters;

internal sealed class IPAddressConverter : DbTypeConverter<IPAddress>
{
    public override IPAddress Parse(DbValue item) => item.Kind switch
    {
        DbValueType.S => IPAddress.Parse(item.ToString()),
        DbValueType.B => new IPAddress(item.ToBinary()),
        _ => throw new Exception($"Cannot DB type: {item.Kind} to IPAddress")
    };

    // Serialize IP addresses as bytes
    public override DbValue ToDbValue(IPAddress value) => new DbValue(value.GetAddressBytes());
}