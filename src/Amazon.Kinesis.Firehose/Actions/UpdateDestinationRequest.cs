#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Kinesis.Firehose;

public sealed class UpdateDestinationRequest
{
    public string CurrentDeliveryStreamVersionId { get; init; }

    public string DeliveryStreamName { get; init; }

    [StringLength(100)]
    public string DestinationId { get; init; }
}