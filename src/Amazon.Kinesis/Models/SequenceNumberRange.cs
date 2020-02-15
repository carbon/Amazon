#nullable disable

namespace Amazon.Kinesis
{
    public class SequenceNumberRange
    {
        public string StartingSequenceNumber { get; set; }

        public string EndingSequenceNumber { get; set; }
    }
}