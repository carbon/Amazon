#nullable disable

namespace Amazon.Translate;

public sealed class TranslateTextRequest
{
    public string SourceLanguageCode { get; init; }

    public string TargetLanguageCode { get; init; }

    public string[] TerminologyNames { get; init; }

    public string Text { get; init; }
}