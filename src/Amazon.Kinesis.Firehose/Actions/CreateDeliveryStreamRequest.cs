#nullable disable

namespace Amazon.Kinesis.Firehose;

public sealed class CreateDeliveryStreamRequest
{
    public string DeliveryStreamName { get; init; }

    public DeliveryStreamType DeliveryStreamType { get; init; }

#nullable enable

    public ExtendedS3DestinationConfiguration? ExtendedS3DestinationConfiguration { get; init; }
}