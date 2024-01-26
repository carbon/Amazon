namespace Amazon.Ses;

public sealed class ReceiptIpFilter
{
    public required string Cidr { get; init; }

    public required string Policy { get; init; }
}
