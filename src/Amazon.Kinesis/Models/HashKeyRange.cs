#nullable disable

namespace Amazon.Kinesis;

public sealed class HashKeyRange
{
    public string StartingHashKey { get; init; }

    public string EndingHashKey { get; init; }
}