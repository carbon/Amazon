using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

[JsonConverter(typeof(JsonStringEnumConverter<StopReason>))]
public enum StopReason
{
    [JsonStringEnumMemberName("end_turn")]
    EndTurn = 1,

    [JsonStringEnumMemberName("tool_use")]
    ToolUse = 2,

    [JsonStringEnumMemberName("max_tokens")]
    MaxTokens = 3,

    [JsonStringEnumMemberName("stop_sequence")]
    StopSequence = 4,

    [JsonStringEnumMemberName("guardrail_intervened")]
    GuardrailIntervened = 5,

    [JsonStringEnumMemberName("content_filtered")]
    ContentFiltered = 6
}