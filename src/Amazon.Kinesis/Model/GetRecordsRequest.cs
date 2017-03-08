using System;

namespace Amazon.Kinesis
{
    public class GetRecordsRequest : KinesisRequest
    {
        public GetRecordsRequest(string shardIterator, int? limit = null)
        {
            #region Preconditions

            if (limit > 10000)
                throw new ArgumentOutOfRangeException(nameof(limit), limit, "Must be 10,000 or fewer");

            #endregion

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
