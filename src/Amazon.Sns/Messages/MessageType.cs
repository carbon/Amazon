using System.Text.Json.Serialization;

namespace Amazon.Sns;

[JsonConverter(typeof(JsonStringEnumConverter<MessageType>))]
public enum MessageType
{
    Notification = 1,
    SubscriptionConfirmation = 2,
    UnsubscribeConfirmation = 3
}

// x-amz-sns-message-type: X