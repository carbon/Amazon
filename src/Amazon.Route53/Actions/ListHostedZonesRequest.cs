namespace Amazon.Route53;

public sealed class ListHostedZonesRequest
{
    public string? DelegationSetId { get; set; }

    public string? Marker { get; set; }

    public int? MaxItems { get; set; }
}


// GET /2013-04-01/hostedzone?delegationsetid=DelegationSetId&marker=Marker&maxitems=MaxItems HTTP/1.1