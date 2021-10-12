namespace Amazon.Elb;

public sealed class DeleteListenerRequest : IElbRequest
{
    public DeleteListenerRequest(string listenerArn)
    {
        ListenerArn = listenerArn ?? throw new ArgumentNullException(nameof(listenerArn));
    }

    public string Action => "DeleteListener";

    public string ListenerArn { get; }
}