using System.Text.Json.Serialization;

using Amazon.Bedrock.Models;

namespace Amazon.Nova;

public sealed class NovaMessage(Role role, params ContentBlock[] content)
{
    [JsonPropertyName("role")]
    public Role Role { get; } = role;

    [JsonPropertyName("content")]
    public ContentBlock[] Content { get; } = content;
}