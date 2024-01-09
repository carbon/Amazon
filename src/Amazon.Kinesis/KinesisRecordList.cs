using System.Collections;

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class KinesisRecordList(List<Record> records, KinesisIterator? nestIterator) : IRecordList
{
    private readonly List<Record> _records = records;

    public int Count => _records.Count;

    public IIterator? NextIterator { get; } = nestIterator;

    IEnumerator<IRecord> IEnumerable<IRecord>.GetEnumerator() => _records.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _records.GetEnumerator();
}
