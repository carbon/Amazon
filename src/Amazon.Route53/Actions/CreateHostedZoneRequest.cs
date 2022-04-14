namespace Amazon.Route53;

public sealed class CreateHostedZoneRequest
{
    public CreateHostedZoneRequest() { }

    public CreateHostedZoneRequest(string name!!)
    {
        Name = name;
    }

    public string? CallerReference { get; set; }

    public string? DelegationSetId { get; set; }

    public HostedZoneConfig? HostedZoneConfig { get; set; }

#nullable disable
    public string Name { get; set; }
#nullable enable

    public VPC? VPC { get; set; }
}