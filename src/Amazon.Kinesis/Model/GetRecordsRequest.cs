using System;

namespace Amazon.Kinesis
{
    public class GetRecordsRequest : KinesisRequest
    {
        public GetRecordsRequest(string shardIterator, int? limit = null)
        {
            #region Preconditions

            if (shardIterator == null) throw new ArgumentNullException(nameof(shardIterator));

            if (limit > 10000) throw new ArgumentException("Must be less than 1000", "limit");

            #endregion

            ShardIterator = shardIterator;
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
