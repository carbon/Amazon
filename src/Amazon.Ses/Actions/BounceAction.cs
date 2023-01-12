namespace Amazon.Ses;

public sealed class BounceAction
{
    public required string Message { get; set; }

    public required string Sender { get; set; }

    public required string SmtpReplyCode { get; set; }

    public string? StatusCode { get; set; }

    public string? TopicArn { get; set; }
}