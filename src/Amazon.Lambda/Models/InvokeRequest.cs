namespace Amazon.Lambda;

public sealed class InvokeRequest
{
    public InvokeRequest(string functionName)
    {
        ArgumentNullException.ThrowIfNull(functionName);

        FunctionName = functionName;
    }

    public InvokeRequest(string functionName, string payload)
    {
        FunctionName = functionName;
        Payload = payload;
    }

    public string FunctionName { get; }

    public InvocationType? InvocationType { get; init; }

    public LogType? LogType { get; init; }

    // JSON that you want to provide to your Lambda function as input.
    public string? Payload { get; }
}
