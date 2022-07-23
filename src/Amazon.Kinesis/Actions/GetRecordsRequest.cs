﻿using System;

namespace Amazon.Kinesis;

public sealed class GetRecordsRequest : KinesisRequest
{
    public GetRecordsRequest(string shardIterator, int? limit = null)
    {
        ArgumentNullException.ThrowIfNull(shardIterator);

        if (limit > 10_000)
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "Must be 10,000 or fewer");

        ShardIterator = shardIterator;
        Limit = limit;
    }

    public int? Limit { get; }

    public string ShardIterator { get; }
}

/*
{
    "Limit": "number",
    "ShardIterator": "string"
}
*/
