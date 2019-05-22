#nullable enable

using System;

namespace Amazon.Kinesis
{
    public sealed class GetRecordsRequest : KinesisRequest
    {
        public GetRecordsRequest(string shardIterator, int? limit = null)
        {
            if (limit > 10_000)
                throw new ArgumentOutOfRangeException(nameof(limit), limit, "Must be 10,000 or fewer");

            ShardIterator = shardIterator ?? throw new ArgumentNullException(nameof(shardIterator));
            Limit = limit;
        }

        public int? Limit { get; }

        public string ShardIterator { get; }
    }
}

/*
{
    "Limit": "number",
    "ShardIterator": "string"
}
*/
