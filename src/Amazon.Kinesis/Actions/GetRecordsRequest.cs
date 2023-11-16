using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public sealed class GetRecordsRequest : KinesisRequest
{
    public GetRecordsRequest(string shardIterator, int? limit = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(shardIterator);

        if (limit > 10_000)
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "Must be 10,000 or fewer");

        ShardIterator = shardIterator;
        Limit = limit;
    }

    [JsonPropertyName("Limit")]
    public int? Limit { get; }

    [JsonPropertyName("ShardIterator")]
    public string ShardIterator { get; }
}

/*
{
    "Limit": "number",
    "ShardIterator": "string"
}
*/
