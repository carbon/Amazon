namespace Amazon.Ses;

public sealed class ReceiptFilter
{
    public required ReceiptIpFilter IpFilter { get; init; }

    public required string Name { get; init; }
}
