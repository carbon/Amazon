using System;

namespace Amazon.Kinesis.Firehose
{
    public readonly struct Record
    {
        public const int MaxSize = 1_000_000; // 1,000 KB

        public Record(byte[] data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                throw new ArgumentException("Must not be empty", nameof(data));
            }

            if (data.Length > MaxSize)
            {
                throw new ArgumentException(nameof(data), "Must be less than 1MB");
            }

            Data = data;
        }

        public byte[] Data { get; }
    }
}

// The data blob, which is base64-encoded when the blob is serialized. 
// The maximum size of the data blob, before base64-encoding, is 1,000 KB.