namespace Amazon.Kinesis;

public sealed class DescribeStreamResult : KinesisResult
{
    public required StreamDescription StreamDescription { get; init; }
}