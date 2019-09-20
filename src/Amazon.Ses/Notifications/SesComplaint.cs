#nullable disable

using System;

namespace Amazon.Ses
{
    public class SesComplaint
    {
        public string UserAgent { get; set; }

        public SesRecipient[] ComplainedRecipients { get; set; }

        public string ComplaintFeedbackType { get; set; }

        public DateTime Timestamp { get; set; }

        public string FeedbackId { get; set; }
    }
}
