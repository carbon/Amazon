#nullable disable

namespace Amazon.Translate
{
    public class TranslateTextResult
    {
        public AppliedTerminology[] AppliedTerminologies { get; init; }

        public string SourceLanguageCode { get; init; }

        public string TargetLanguageCode { get; init; }

        public string TranslatedText { get; init; }
    }
}