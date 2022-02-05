namespace Amazon.Ses;

public sealed class StopAction
{
    public StopAction(string scope, string? topicArn)
    {
        Scope = scope;
        TopicArn = topicArn;
    }

    public string Scope { get; }

    public string? TopicArn { get; }
}
