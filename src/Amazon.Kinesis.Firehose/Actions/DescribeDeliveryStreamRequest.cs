namespace Amazon.Kinesis.Firehose;

public sealed class DescribeDeliveryStreamRequest
{
    public required string DeliveryStreamName { get; init; }

    public string? ExclusiveStartDestinationId { get; init; }

    public int? Limit { get; init; }
}