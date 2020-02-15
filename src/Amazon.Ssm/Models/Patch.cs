using System.Text.Json.Serialization;
using Amazon.Ssm.Converters;

namespace Amazon.Ssm
{
    public class Patch
    {
#nullable disable

        public string Id { get; set; }

        public string Classification { get; set; }

#nullable enable

        public string? ContentUrl { get; set; }

        public string? Description { get; set; }

        public string? KbNumber { get; set; }

        public string? Language { get; set; }

        public string? MsrcNumber { get; set; }

        public string? MsrcSeverity { get; set; }

        public string? Product { get; set; }

        public string? ProductFamily { get; set; }

        [JsonConverter(typeof(NullableTimestampConverter))]
        public Timestamp? ReleaseDate { get; set; }

        public string? Title { get; set; }

        public string? Vender { get; set; }
    }
}