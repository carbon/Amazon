namespace Amazon.Ses
{
    public enum SesBounceType
    {
        Undetermined = 1,
        Permanent = 2,
        Transient = 3,
    }

    public enum SesBounceSubtype
    {
        Undetermined,
        General,
        NoEmail,
        Suppressed,
        MailboxFull,
        MessageToolarge,
        ContentRejected,
        AttachmentRejected
    }
}