#nullable disable

using System;

namespace Amazon.Ses
{
    public sealed class SesComplaint
    {
        public string UserAgent { get; set; }

        public SesRecipient[] ComplainedRecipients { get; set; }

        public string ComplaintFeedbackType { get; set; }

        //2012-05-25T14:59:38.613-07:00

        public DateTimeOffset Timestamp { get; set; }

        public string FeedbackId { get; set; }
    }
}
