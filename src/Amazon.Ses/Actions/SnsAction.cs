namespace Amazon.Ses;

public sealed class SnsAction
{
    public SnsAction(string topicArn, string? encoding = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(topicArn);

        TopicArn = topicArn;
        Encoding = encoding;
    }

    // UTF-8 | Base64
    public string? Encoding { get; set; }

    public required string TopicArn { get; set; }
}