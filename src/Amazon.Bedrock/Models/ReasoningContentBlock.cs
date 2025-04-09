﻿using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public class ReasoningContentBlock
{
    [JsonPropertyName("reasoningText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ReasoningTextBlock? ReasoningText { get; set; }

    [JsonPropertyName("redactedContent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RedactedContent { get; set; }
}
