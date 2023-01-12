namespace Amazon.Route53;

public sealed class CreateHostedZoneRequest
{
    public CreateHostedZoneRequest() { }

    public CreateHostedZoneRequest(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
    }

    public string? CallerReference { get; set; }

    public string? DelegationSetId { get; set; }

    public HostedZoneConfig? HostedZoneConfig { get; set; }

    public required string Name { get; set; }

    public VPC? VPC { get; set; }
}