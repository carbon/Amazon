namespace Amazon.Translate;

public sealed class TranslateTextResult
{
    public AppliedTerminology[]? AppliedTerminologies { get; init; }

    public required string SourceLanguageCode { get; init; }

    public required string TargetLanguageCode { get; init; }

    public required string TranslatedText { get; init; }
}