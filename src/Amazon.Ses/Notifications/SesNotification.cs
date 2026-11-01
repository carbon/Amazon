using System.Text.Json.Serialization;

namespace Amazon.Ses;

public class SesNotification
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

    [JsonPropertyName("delivery")]
    public SesDelivery? Delivery { get; init; }

    /// <summary>
    /// This field is present only if the notification was triggered by an SNS action.
    /// Notifications triggered by all other actions do not contain this field.
    /// </summary>
	[JsonPropertyName("content")]
	public string? Content { get; init; }
}



public sealed class SesEvent : SesNotification
{
    // If you set up event publishing, this field is named eventType.
    [JsonPropertyName("eventType")]
    public SesEventType EventType { get; set; }
}


[JsonConverter(typeof(JsonStringEnumConverter<SesEventType>))]
public enum SesEventType
{
    Send = 1,
    Reject = 2,
    Delivery = 3,
    Bounce = 4,
    DeliveryDelay = 5,
    Complaint = 6,
    Open = 7,
    Click = 8,
    RenderingFailure = 9,
    Subscription = 10
}

// https://docs.aws.amazon.com/ses/latest/dg/event-publishing-retrieving-sns-examples.html