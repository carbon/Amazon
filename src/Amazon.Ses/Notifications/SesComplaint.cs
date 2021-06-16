#nullable disable

using System;

namespace Amazon.Ses
{
    public sealed class SesComplaint
    {
        public string UserAgent { get; init; }

        public SesRecipient[] ComplainedRecipients { get; init; }

        public string ComplaintFeedbackType { get; init; }

        //2012-05-25T14:59:38.613-07:00

        public DateTimeOffset Timestamp { get; init; }

        public string FeedbackId { get; init; }
    }
}