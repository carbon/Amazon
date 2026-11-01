#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesComplaint
{
    [JsonPropertyName("feedbackId")]
    public string FeedbackId { get; init; }

    [JsonPropertyName("complainedRecipients")]
    public SesRecipient[] ComplainedRecipients { get; init; }

    [JsonPropertyName("complaintSubType")]
    public string ComplaintSubType { get; init; }

    // 2012-05-25T14:59:38.613-07:00

    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; }

#nullable enable

    // If a feedback report is attached to the complaint, the following fields may be present.

    [JsonPropertyName("userAgent")]
    public string? UserAgent { get; init; }

    [JsonPropertyName("complaintFeedbackType")]
    public string? ComplaintFeedbackType { get; init; }
}

/*
abuse — Indicates unsolicited email or some other kind of email abuse.
auth -failure—Email authentication failure report.
fraud — Indicates some kind of fraud or phishing activity.
not-spam — Indicates that the entity providing the report does not consider the message to be spam.This may be used to correct a message that was incorrectly tagged or categorized as spam.
other — Indicates any other feedback that does not fit into other registered types.
virus — Reports that a virus is found in the originating message.
*/