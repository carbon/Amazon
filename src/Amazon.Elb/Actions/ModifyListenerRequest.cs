namespace Amazon.Elb;

public sealed class ModifyListenerRequest : IElbRequest
{
    public string Action => "ModifyListener";

    public Certificate[]? Certificates { get; init; }

    public Action[]? DefaultActions { get; init; }

    public required string ListenerArn { get; init; }

    public int? Port { get; init; }

    public Protocol? Protocol { get; init; }

    public string? SslPolicy { get; init; }
}