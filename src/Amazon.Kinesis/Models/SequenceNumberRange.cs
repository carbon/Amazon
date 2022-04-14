#nullable disable

namespace Amazon.Kinesis;

public sealed class SequenceNumberRange
{
    public string StartingSequenceNumber { get; init; }

    public string EndingSequenceNumber { get; init; }
}