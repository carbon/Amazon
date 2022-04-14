using System;

namespace Amazon.Ses;

public sealed class LambdaAction
{
    public LambdaAction(string functionArn!!, string? invocationType = null, string? topicArn = null)
    {
        FunctionArn = functionArn;
        InvocationType = invocationType;
        TopicArn = topicArn;
    }

    public string FunctionArn { get; }

    public string? InvocationType { get; }

    public string? TopicArn { get; }
}