namespace Amazon.Bedrock.Models;
using System.Text.Json.Serialization;

public class RegexMatch
{
    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("match")]
    public string Match { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("regex")]
    public string Regex { get; set; }
}
