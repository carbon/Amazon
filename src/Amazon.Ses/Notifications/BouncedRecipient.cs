#nullable disable

namespace Amazon.Ses
{
    public sealed class BouncedRecipient
    {
        public string Status { get; init; }

        public string Action { get; init; }

        public string DiagnosticCode { get; init; }

        public string EmailAddress { get; init; }
    }
}
