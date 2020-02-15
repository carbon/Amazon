#nullable disable

namespace Amazon.Kinesis
{
    public class HashKeyRange
    {
        public string StartingHashKey { get; set; }

        public string EndingHashKey { get; set; }
    }
}