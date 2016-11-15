using System;

namespace Amazon.Ses
{
    public class SesBounce
    {
        public SesBounceType BounceType { get; set; }

        public SesBounceSubtype BounceSubtype { get; set; }

        public BouncedRecipient[] BouncedRecipients { get; set; }

        public DateTime Timestamp { get; set; }

        public string ReportingMta { get; set; }
    }

    public class BouncedRecipient
    {
        public string Status { get; set; }

        public string Action { get; set; }

        public string DiagnosticCode { get; set; }

        public string EmailAddress { get; set; }
    }
}
