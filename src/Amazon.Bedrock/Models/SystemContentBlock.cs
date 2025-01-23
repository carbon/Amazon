namespace Amazon.Bedrock.Models;

using System.Text.Json.Serialization;

public sealed class SystemContentBlock
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    public static implicit operator SystemContentBlock(string text)
    {
        return new SystemContentBlock { Text = text };
    }
}