#nullable disable

using System;
using System.Text.Json.Serialization;

namespace Amazon.Metadata
{
    public sealed class InstanceAction
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
}