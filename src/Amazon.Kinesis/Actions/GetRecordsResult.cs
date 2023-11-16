using System.Collections;
using System.Text.Json.Serialization;

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class GetRecordsResult : KinesisResult, IRecordList
{
    [JsonPropertyName("NextShardIterator")]
    public required string NextShardIterator { get; set; }

    [JsonPropertyName("Records")]
    public required List<Record> Records { get; set; }

    #region IRecordList

    int IRecordList.Count => Records.Count;

    IIterator? IRecordList.NextIterator
    {
        get
        {
            if (NextShardIterator is null) return null;

            return new KinesisIterator(NextShardIterator);
        }
    }

    IEnumerator<IRecord> IEnumerable<IRecord>.GetEnumerator() => Records.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Records.GetEnumerator();

    #endregion
}

/*
{
    "NextShardIterator": "string",
    "Records": [
        {
            "Data": "blob",
            "PartitionKey": "string",
            "SequenceNumber": "string"
        }
    ]
}
*/
