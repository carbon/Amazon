using System.ComponentModel.DataAnnotations;

namespace Amazon.Kinesis.Firehose;

public sealed class UpdateDestinationRequest
{
    public required string CurrentDeliveryStreamVersionId { get; init; }

    [StringLength(64)]
    public required string DeliveryStreamName { get; init; }

    [StringLength(100)]
    public required string DestinationId { get; init; }
}