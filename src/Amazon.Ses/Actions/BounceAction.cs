#nullable disable

namespace Amazon.Ses;

public sealed class BounceAction
{
    public string Message { get; set; }

    public string Sender { get; set; }

    public string SmtpReplyCode { get; set; }

    public string StatusCode { get; set; }

    public string TopicArn { get; set; }
}