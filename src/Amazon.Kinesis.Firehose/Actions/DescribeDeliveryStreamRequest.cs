#nullable disable

namespace Amazon.Kinesis.Firehose;

public sealed class DescribeDeliveryStreamRequest
{
    public string DeliveryStreamName { get; init; }

#nullable enable

    public string? ExclusiveStartDestinationId { get; init; }

    public int? Limit { get; init; }
}