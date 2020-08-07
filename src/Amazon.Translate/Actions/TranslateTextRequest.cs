#nullable disable

namespace Amazon.Translate
{
    public sealed class TranslateTextRequest
    {
        public string SourceLanguageCode { get; set; }

        public string TargetLanguageCode { get; set; }

        public string[] TerminologyNames { get; set; }

        public string Text { get; set; }
    }
}