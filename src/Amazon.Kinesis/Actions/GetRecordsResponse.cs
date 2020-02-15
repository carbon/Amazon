#nullable disable

using System.Collections;
using System.Collections.Generic;

using Carbon.Data.Streams;

namespace Amazon.Kinesis
{
    public sealed class GetRecordsResponse : KinesisResponse, IRecordList
    {
        public string NextShardIterator { get; set; }

        public List<Record> Records { get; set; }

        public int Count => Records?.Count ?? 0;

#nullable enable

        #region IDataRecordList

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
