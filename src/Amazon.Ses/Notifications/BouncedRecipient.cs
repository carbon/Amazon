#nullable disable


namespace Amazon.Ses
{
    public sealed class BouncedRecipient
    {
        public string Status { get; set; }

        public string Action { get; set; }

        public string DiagnosticCode { get; set; }

        public string EmailAddress { get; set; }
    }
}
