using System.Text.Json.Serialization;

namespace Amazon.Translate;

public sealed class TranslateTextRequest
{
    public required string SourceLanguageCode { get; init; }

    public required string TargetLanguageCode { get; init; }

    public string[]? TerminologyNames { get; init; }

    public required string Text { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TranslationSettings? Settings { get; init; }
}