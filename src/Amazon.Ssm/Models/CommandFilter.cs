using System.Text.Json.Serialization;

namespace Amazon.Ssm
{
    public sealed class CommandFilter
    {
#nullable disable
        public CommandFilter() { }
#nullable enable

        public CommandFilter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}