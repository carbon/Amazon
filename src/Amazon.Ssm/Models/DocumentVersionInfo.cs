using System;
using System.Text.Json.Serialization;

using Amazon.Ssm.Converters;

namespace Amazon.Ssm
{
    public sealed class DocumentVersionInfo
    {
        public string Name { get; set; }

        [JsonConverter(typeof(TimestampConverter))]
        public Timestamp CreatedDate { get; set; }

        public string DocumentVersion { get; set; }

        public bool IsDefaultVersion { get; set; }
    }
}
