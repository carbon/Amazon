namespace Amazon.Elb;

public sealed class RedirectActionConfig
{
    public string? Host { get; init; }

    public string? Path { get; init; }

    public string? Port { get; init; }

    public string? Protocol { get; init; }

    public string? Query { get; init; }

    // HTTP_301 | HTTP_302
    public required string StatusCode { get; init; }
}
