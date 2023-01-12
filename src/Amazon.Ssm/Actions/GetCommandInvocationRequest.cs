namespace Amazon.Ssm;

public sealed class GetCommandInvocationRequest : ISsmRequest
{
    public GetCommandInvocationRequest() { }

    public GetCommandInvocationRequest(string commandId, string instanceId)
    {
        CommandId = commandId;
        InstanceId = instanceId;
    }

    public required string CommandId { get; init; }

    public required string InstanceId { get; init; }

    public string? PluginName { get; init; }
}
