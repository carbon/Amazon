using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

[method:JsonConstructor]
public readonly struct SequenceNumberRange(
    string startingSequenceNumber,
    string? endingSequenceNumber = null)
{
    public string StartingSequenceNumber { get; } = startingSequenceNumber;

    public string? EndingSequenceNumber { get; } = endingSequenceNumber;
}