namespace Amazon.Elb;

public sealed class DeleteListenerRequest : IElbRequest
{
    public DeleteListenerRequest(string listenerArn)
    {
        ArgumentException.ThrowIfNullOrEmpty(listenerArn);

        ListenerArn = listenerArn;
    }

    public string Action => "DeleteListener";

    public string ListenerArn { get; }
}