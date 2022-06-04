﻿#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesComplaint
{
    [JsonPropertyName("userAgent")]
    public string UserAgent { get; init; }

    [JsonPropertyName("complainedRecipients")]
    public SesRecipient[] ComplainedRecipients { get; init; }

    [JsonPropertyName("complaintFeedbackType")]
    public string ComplaintFeedbackType { get; init; }

    // 2012-05-25T14:59:38.613-07:00

    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; init; }

    [JsonPropertyName("feedbackId")]
    public string FeedbackId { get; init; }
}