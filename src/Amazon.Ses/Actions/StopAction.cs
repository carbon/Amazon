namespace Amazon.Ses;

public sealed class StopAction(string scope, string? topicArn)
{
    public string Scope { get; } = scope;

    public string? TopicArn { get; } = topicArn;
}