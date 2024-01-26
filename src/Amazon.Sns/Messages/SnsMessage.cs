using System.Text.Json.Serialization;

namespace Amazon.Sns;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(SubscriptionConfirmation), "SubscriptionConfirmation")]
[JsonDerivedType(typeof(UnsubscribeConfirmation), "UnsubscribeConfirmation")]
[JsonDerivedType(typeof(Notification), "Notification")]
public abstract class SnsMessage
{
    public required string MessageId { get; init; }

    public required string TopicArn { get; init; }

    public string? Subject { get; init; }

    public required string Message { get; init; }

    public required DateTimeOffset Timestamp { get; init; }

    public required string SignatureVersion { get; init; }

    public required string Signature { get; init; }

    public required string SigningCertURL { get; init; }
}