namespace Amazon.Route53;

public sealed class HealthCheck
{
    public required string Id { get; init; }

    public required string CallerReference { get; init; }

    public required long HealthCheckVersion { get; init; }

    public required HealthCheckConfig HealthCheckConfig { get; init; }

    public LinkedService? LinkedService { get; init; }
}