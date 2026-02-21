using System.Text.Json.Serialization;

namespace Amazon.Ses;

[JsonConverter(typeof(JsonStringEnumConverter<SesBounceSubtype>))]
public enum SesBounceSubtype
{
    Undetermined             = 1,
    General                  = 2,
    NoEmail                  = 3,
    Suppressed               = 4,
    MailboxFull              = 5,
    MessageToolarge          = 6,
    ContentRejected          = 7,
    AttachmentRejected       = 8,
    OnAccountSuppressionList = 9,
    UnsubscribedRecipient    = 10
}