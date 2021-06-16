#nullable disable

using System;

namespace Amazon.Ses
{
    public sealed class SesBounce
    {
        public SesBounceType BounceType { get; init; }

        public SesBounceSubtype BounceSubType { get; init; }

        public BouncedRecipient[] BouncedRecipients { get; init; }

        public DateTime Timestamp { get; init; }

        public string ReportingMta { get; init; }
    }
}
