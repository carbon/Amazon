using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

[method:JsonConstructor]
public readonly struct HashKeyRange(
    string startingHashKey,
    string endingHashKey)
{
    public string StartingHashKey { get; } = startingHashKey;

    public string EndingHashKey { get; } = endingHashKey;
}