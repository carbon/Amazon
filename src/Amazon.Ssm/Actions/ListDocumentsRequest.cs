using System.Text.Json.Serialization;

namespace Amazon.Ssm
{
    public sealed class ListDocumentsRequest : ISsmRequest
    {
        public DocumentFilter[] DocumentFilterList { get; set; }

        public int? MaxResults { get; set; }

        public string NextToken { get; set; }
    }

    public class DocumentFilter
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}