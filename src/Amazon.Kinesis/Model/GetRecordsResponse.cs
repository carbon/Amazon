using System.Collections;
using System.Collections.Generic;

using Carbon.Data.Streams;

namespace Amazon.Kinesis
{
    public class GetRecordsResponse : IRecordList
    {
        public string NextShardIterator { get; set; }

        public List<Record> Records { get; set; }


        public int Count => Records.Count;

        #region IDataRecordList

        IIterator IRecordList.NextIterator
        {
            get
            {
                if (NextShardIterator == null) return null;

                return new KinesisIterator(NextShardIterator);
            }
        }

        IEnumerator<IRecord> IEnumerable<IRecord>.GetEnumerator()
            => Records.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Records.GetEnumerator();

        #endregion
    }

    public struct KinesisIterator : IIterator
    {
        public KinesisIterator(string value)
        {
            Value = value;
        }

        public string Value { get; }
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
