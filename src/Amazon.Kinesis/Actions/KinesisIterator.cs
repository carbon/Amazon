using System;

using Carbon.Data.Streams;

namespace Amazon.Kinesis
{
    public sealed class KinesisIterator : IIterator
    {
        public KinesisIterator(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }
    }
}