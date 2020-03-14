#nullable disable

using System;
using System.Text.Json.Serialization;

namespace Amazon.Ses
{
    public sealed class SesBounce
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SesBounceType BounceType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SesBounceSubtype BounceSubType { get; set; }

        public BouncedRecipient[] BouncedRecipients { get; set; }

        public DateTime Timestamp { get; set; }

        public string ReportingMta { get; set; }
    }
}
