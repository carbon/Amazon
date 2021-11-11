using System.ComponentModel.DataAnnotations;

namespace Amazon.Kinesis.Firehose;

public sealed class ListDeliveryStreamsRequest
{
    [StringLength(64)]
    public string? DeliveryStreamType { get; init; }

    public string? ExclusiveStartDeliveryStreamName { get; init; }

    public int? Limit { get; init; }
}


public class ListDeliveryStreamsResult
{
}