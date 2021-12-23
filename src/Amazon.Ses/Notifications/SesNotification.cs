using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesNotification
{
    [JsonPropertyName("notificationType")]
    public SesNotificationType NotificationType { get; init; }

    [JsonPropertyName("complaint")]
    public SesComplaint? Complaint { get; init; }

    [JsonPropertyName("bounce")]
    public SesBounce? Bounce { get; init; }

#nullable disable

    [JsonPropertyName("mail")]
    public SesMail Mail { get; init; }

#nullable enable

	[JsonPropertyName("receipt")]
	public SesReceipt? Receipt { get; init; }


    /// <summary>
    /// This field is present only if the notification was triggered by an SNS action.
    /// Notifications triggered by all other actions do not contain this field.
    /// </summary>
	[JsonPropertyName("content")]
	public string? Content { get; init; }
}