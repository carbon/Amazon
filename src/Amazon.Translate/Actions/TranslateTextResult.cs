#nullable disable

namespace Amazon.Translate
{
    public class TranslateTextResult
    {
        public AppliedTerminology[] AppliedTerminologies { get; set; }

        public string SourceLanguageCode { get; set; }

        public string TargetLanguageCode { get; set; }

        public string TranslatedText { get; set; }
    }
}