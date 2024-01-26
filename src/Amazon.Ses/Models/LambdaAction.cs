namespace Amazon.Ses;

public sealed class LambdaAction(
    string functionArn,
    string? invocationType = null,
    string? topicArn = null)
{
    public string FunctionArn { get; } = functionArn ?? throw new ArgumentNullException(nameof(functionArn));

    public string? InvocationType { get; } = invocationType;

    public string? TopicArn { get; } = topicArn;
}