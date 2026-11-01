namespace Amazon.Elb;

public sealed class FixedResponseActionConfig
{
    // text/plain | text/css | text/html | application/javascript | application/json
    public string? ContentType { get; init; }

    public string? MessageBody { get; init; }

    // 2XX, 4XX or 5XX
    public required string StatusCode { get; init; }
}
