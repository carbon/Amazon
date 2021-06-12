#nullable disable

namespace Amazon.Kinesis.Firehose
{
    public sealed class PutRecordResult
    {
        public bool Encrypted { get; init; }

        public string RecordId { get; init; }
    }
}